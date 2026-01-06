#include <mpi.h>
#include <stdio.h>
#include <stdlib.h>

#define N 1000   

int main(int argc, char** argv) {
    int rank, size;

    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);


    int (*A)[N] = NULL;
    int (*B)[N] = malloc(N * N * sizeof(int));
    int (*C)[N] = NULL;

    if (rank == 0) {
        A = malloc(N * N * sizeof(int));
        C = malloc(N * N * sizeof(int));

        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++) {
                A[i][j] = rand() % 10;
                B[i][j] = rand() % 10;
            }
    }


    int *sendcounts = malloc(size * sizeof(int));
    int *displs = malloc(size * sizeof(int));

    int base = N / size;       
    int extra = N % size;       

    int offset = 0;

    for (int i = 0; i < size; i++) {
        int rows = base + (i < extra ? 1 : 0);
        sendcounts[i] = rows * N;
        displs[i] = offset * N;
        offset += rows;
    }

    int local_rows = base + (rank < extra ? 1 : 0);
    int (*local_A)[N] = malloc(local_rows * N * sizeof(int));
    int (*local_C)[N] = malloc(local_rows * N * sizeof(int));


    double seq_time = 0.0;

    if (rank == 0) {
        double start = MPI_Wtime();

        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++) {
                C[i][j] = 0;
                for (int k = 0; k < N; k++)
                    C[i][j] += A[i][k] * B[k][j];
            }

        double end = MPI_Wtime();
        seq_time = end - start;
    }


    MPI_Bcast(B, N*N, MPI_INT, 0, MPI_COMM_WORLD);

    MPI_Scatterv(A, sendcounts, displs, MPI_INT,
                 local_A, local_rows*N, MPI_INT,
                 0, MPI_COMM_WORLD);

    MPI_Barrier(MPI_COMM_WORLD);
    double par_start = MPI_Wtime();

    for (int i = 0; i < local_rows; i++)
        for (int j = 0; j < N; j++) {
            local_C[i][j] = 0;
            for (int k = 0; k < N; k++)
                local_C[i][j] += local_A[i][k] * B[k][j];
        }

    MPI_Gatherv(local_C, local_rows*N, MPI_INT,
                C, sendcounts, displs, MPI_INT,
                0, MPI_COMM_WORLD);

    MPI_Barrier(MPI_COMM_WORLD);
    double par_end = MPI_Wtime();
    double par_time = par_end - par_start;


    if (rank == 0) {
        printf("=== MATRIX MULTIPLICATION %dx%d ===\n", N, N);
        printf("Sequential time: %f sec\n", seq_time);
        printf("Parallel time (%d processes): %f sec\n", size, par_time);

        double speedup = seq_time / par_time;
        double efficiency = (speedup / size) * 100.0;

        printf("Speedup: %f\n", speedup);
        printf("Efficiency: %.2f%%\n", efficiency);
    }


    if (rank == 0) {
        free(A);
        free(C);
    }

    free(B);
    free(local_A);
    free(local_C);
    free(sendcounts);
    free(displs);

    MPI_Finalize();
    return 0;
}
