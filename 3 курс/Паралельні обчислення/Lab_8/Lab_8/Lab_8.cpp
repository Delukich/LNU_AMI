#define CL_TARGET_OPENCL_VERSION 200
#include <CL/cl.h>
#include <iostream>
#include <vector>
#include <chrono>
#include <random>
#include <cmath>

const char* kernelSource = R"CLC(
__kernel void matMul(__global float* A, __global float* B, __global float* C, const int N) {
    int row = get_global_id(1);
    int col = get_global_id(0);
    if (row < N && col < N) {
        float sum = 0.0f;
        for (int k = 0; k < N; k++)
            sum += A[row * N + k] * B[k * N + col];
        C[row * N + col] = sum;
    }
}
)CLC";

int main() {
    int N = 1000;
    std::vector<float> A(N * N), B(N * N), C(N * N), Ccpu(N * N);
    std::mt19937 rng(0);
    std::uniform_real_distribution<float> dist(1.0f, 10.0f);
    for (int i = 0; i < N * N; i++) { A[i] = dist(rng); B[i] = dist(rng); }

    auto t1 = std::chrono::high_resolution_clock::now();
    for (int i = 0; i < N; i++)
        for (int j = 0; j < N; j++) {
            float sum = 0.0f;
            for (int k = 0; k < N; k++) sum += A[i * N + k] * B[k * N + j];
            Ccpu[i * N + j] = sum;
        }
    auto t2 = std::chrono::high_resolution_clock::now();
    double cpu_time = std::chrono::duration<double, std::milli>(t2 - t1).count();

    cl_int err;
    cl_platform_id platform = NULL;
    cl_device_id device = NULL;
    cl_uint numPlatforms;
    err = clGetPlatformIDs(1, &platform, &numPlatforms);
    err = clGetDeviceIDs(platform, CL_DEVICE_TYPE_GPU, 1, &device, NULL);
    if (err != CL_SUCCESS) err = clGetDeviceIDs(platform, CL_DEVICE_TYPE_CPU, 1, &device, NULL);

    char device_name[128];
    clGetDeviceInfo(device, CL_DEVICE_NAME, sizeof(device_name), device_name, NULL);
    cl_uint compute_units;
    clGetDeviceInfo(device, CL_DEVICE_MAX_COMPUTE_UNITS, sizeof(compute_units), &compute_units, NULL);

    cl_context context = clCreateContext(NULL, 1, &device, NULL, NULL, &err);
    const cl_queue_properties props[] = { 0 };
    cl_command_queue queue = clCreateCommandQueueWithProperties(context, device, props, &err);
    cl_program program = clCreateProgramWithSource(context, 1, &kernelSource, NULL, &err);
    err = clBuildProgram(program, 0, NULL, NULL, NULL, NULL);
    cl_kernel kernel = clCreateKernel(program, "matMul", &err);

    size_t bytes = N * N * sizeof(float);
    cl_mem bufA = clCreateBuffer(context, CL_MEM_READ_ONLY, bytes, NULL, &err);
    cl_mem bufB = clCreateBuffer(context, CL_MEM_READ_ONLY, bytes, NULL, &err);
    cl_mem bufC = clCreateBuffer(context, CL_MEM_WRITE_ONLY, bytes, NULL, &err);

    clEnqueueWriteBuffer(queue, bufA, CL_TRUE, 0, bytes, A.data(), 0, NULL, NULL);
    clEnqueueWriteBuffer(queue, bufB, CL_TRUE, 0, bytes, B.data(), 0, NULL, NULL);
    clSetKernelArg(kernel, 0, sizeof(cl_mem), &bufA);
    clSetKernelArg(kernel, 1, sizeof(cl_mem), &bufB);
    clSetKernelArg(kernel, 2, sizeof(cl_mem), &bufC);
    clSetKernelArg(kernel, 3, sizeof(int), &N);

    size_t global[2] = { (size_t)N, (size_t)N };
    auto t3 = std::chrono::high_resolution_clock::now();
    err = clEnqueueNDRangeKernel(queue, kernel, 2, NULL, global, NULL, 0, NULL, NULL);
    clFinish(queue);
    auto t4 = std::chrono::high_resolution_clock::now();
    clEnqueueReadBuffer(queue, bufC, CL_TRUE, 0, bytes, C.data(), 0, NULL, NULL);

    double gpu_time = std::chrono::duration<double, std::milli>(t4 - t3).count();
    double speedup = cpu_time / gpu_time;

    std::cout << "OpenCL Matrix Multiplication (N=" << N << ")\n";
    std::cout << "Device: " << device_name << "\n";
    std::cout << "CPU time: " << cpu_time << " ms\n";
    std::cout << "GPU time: " << gpu_time << " ms\n";
    std::cout << "Compute Units: " << compute_units << "\n";
    std::cout << "Speedup: " << speedup << "x\n";

    int errors = 0;
    for (int i = 0; i < N * N; i++) {
        float diff = fabs(C[i] - Ccpu[i]);
        float tol = 1e-2f * std::fabs(Ccpu[i]);
        if (diff > tol) errors++;
    }
    std::cout << "Validation: " << (errors ? "FAIL" : "OK") << "\n";

    clReleaseMemObject(bufA);
    clReleaseMemObject(bufB);
    clReleaseMemObject(bufC);
    clReleaseKernel(kernel);
    clReleaseProgram(program);
    clReleaseCommandQueue(queue);
    clReleaseContext(context);
    return 0;
}
