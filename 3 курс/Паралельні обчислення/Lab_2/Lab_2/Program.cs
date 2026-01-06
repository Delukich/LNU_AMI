using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static int[,] GenerateMatrix(int n, int m)
    {
        Random rand = new Random();
        int[,] matrix = new int[n, m];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                matrix[i, j] = rand.Next(1, 10);
        return matrix;
    }

    // Послідовне множення
    static void MultiplySequential(int[,] A, int[,] B, int[,] C, int n, int m, int p)
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < p; j++)
            {
                int sum = 0;
                for (int k = 0; k < m; k++)
                    sum += A[i, k] * B[k, j];
                C[i, j] = sum;
            }
    }

    // Паралельне множення
    static void MultiplyParallelThreads(int[,] A, int[,] B, int[,] C, int n, int m, int p, int k)
    {
        Thread[] threads = new Thread[k];
        int rowsPerThread = n / k;
        int remainingRows = n % k;

        int startRow = 0;

        for (int t = 0; t < k; t++)
        {
            int extra = (t < remainingRows) ? 1 : 0;
            int rows = rowsPerThread + extra;
            int threadStart = startRow;
            int threadEnd = startRow + rows;

            threads[t] = new Thread(() =>
            {
                for (int i = threadStart; i < threadEnd; i++)
                    for (int j = 0; j < p; j++)
                    {
                        int sum = 0;
                        for (int x = 0; x < m; x++)
                            sum += A[i, x] * B[x, j];
                        C[i, j] = sum;
                    }
            });

            threads[t].Start();
            startRow = threadEnd;
        }

        foreach (Thread thread in threads)
            thread.Join();
    }
    
    
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        int n = 2000;  
        int m = 2000;   
        int p = 2000;   
        int k = 16;     

        Console.WriteLine($"Матриця A: {n}x{m}");
        Console.WriteLine($"Матриця B: {m}x{p}");
        Console.WriteLine($"Кількість потоків: {k}\n");
        
        int[,] A = GenerateMatrix(n, m);
        int[,] B = GenerateMatrix(m, p);
        int[,] C1 = new int[n, p];
        int[,] C2 = new int[n, p];

        // Послідовне множення 
        Stopwatch sw = Stopwatch.StartNew();
        MultiplySequential(A, B, C1, n, m, p);
        sw.Stop();
        long sequentialTime = sw.ElapsedMilliseconds;
        Console.WriteLine($"Послідовне множення завершено за {sequentialTime} мс ({sequentialTime / 1000.0:F2} с)");

        // Паралельне множення 
        sw.Restart();
        MultiplyParallelThreads(A, B, C2, n, m, p, k);
        sw.Stop();
        long parallelTime = sw.ElapsedMilliseconds;
        Console.WriteLine($"Паралельне множення завершено за {parallelTime} мс ({parallelTime / 1000.0:F2} с)");
        
        // Прискорення та ефективність
        double speedup = (double)sequentialTime / parallelTime;
        double efficiency = speedup / k;

        Console.WriteLine($"\nПрискорення: {speedup:F2}");
        Console.WriteLine($"Ефективність: {efficiency * 100:F2}%");
    }
}
