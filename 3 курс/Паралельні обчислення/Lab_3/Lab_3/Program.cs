using System.Diagnostics;
using System.Threading;

class GaussSolver
{
    static int n = 1000;      
    static int kThreads = 4; 

    static double[,] A;
    static double[] b;
    static double[] x_seq;
    static double[] x_par;

    static void Main()
    {
        Random rand = new Random();
        
        A = new double[n, n];
        b = new double[n];
        x_seq = new double[n];
        x_par = new double[n];

        for (int i = 0; i < n; i++)
        {
            b[i] = rand.Next(1, 10);
            for (int j = 0; j < n; j++)
                A[i, j] = rand.Next(1, 10);
        }
        
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        Console.WriteLine($"Розмір системи: {n} x {n}");
        Console.WriteLine($"Кількість потоків: {kThreads}\n");
        
        // Послідовне розв'язання
        Stopwatch sw = Stopwatch.StartNew();
        GaussSequential(A, b, x_seq);
        sw.Stop();
        double T_seq = sw.Elapsed.TotalSeconds;
        Console.WriteLine($"Послідовне розв'язання: {sw.ElapsedMilliseconds} мс ({T_seq:F2} с)");

        // Паралельне розв'язання
        sw.Restart();
        GaussParallel(A, b, x_par, kThreads);
        sw.Stop();
        double T_par = sw.Elapsed.TotalSeconds;
        Console.WriteLine($"Паралельне розв'язання: {sw.ElapsedMilliseconds} мс ({T_par:F2} с)\n");
        
        // Прискорення та ефективність
        double acceleration = T_seq / T_par;
        double efficiency = acceleration / kThreads * 100;
        Console.WriteLine($"Прискорення: {acceleration:F2}");
        Console.WriteLine($"Ефективність: {efficiency:F2}%");
    }
    // Послідовне розв'язання
    static void GaussSequential(double[,] A, double[] b, double[] x)
    {
        int N = A.GetLength(0);
        double[,] a = (double[,])A.Clone();
        double[] bb = (double[])b.Clone();

        for (int i = 0; i < N; i++)
        {
            for (int k = i + 1; k < N; k++)
            {
                double factor = a[k, i] / a[i, i];
                for (int j = i; j < N; j++)
                    a[k, j] -= factor * a[i, j];
                bb[k] -= factor * bb[i];
            }
        }

        for (int i = N - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < N; j++)
                sum += a[i, j] * x[j];
            x[i] = (bb[i] - sum) / a[i, i];
        }
    }
    // Паралельне розв'язання
    static void GaussParallel(double[,] A, double[] b, double[] x, int kThreads)
    {
        int N = A.GetLength(0);
        double[,] a = (double[,])A.Clone();
        double[] bb = (double[])b.Clone();

        for (int i = 0; i < N; i++)
        {
            Thread[] threads = new Thread[kThreads];
            int rowsLeft = N - i - 1;
            int rowsPerThread = rowsLeft / kThreads;
            int extra = rowsLeft % kThreads;
            int startRow = i + 1;

            for (int t = 0; t < kThreads; t++)
            {
                int rows = rowsPerThread + (t < extra ? 1 : 0);
                int sRow = startRow;
                int eRow = sRow + rows;

                threads[t] = new Thread(() =>
                {
                    for (int k = sRow; k < eRow; k++)
                    {
                        double factor = a[k, i] / a[i, i];
                        for (int j = i; j < N; j++)
                            a[k, j] -= factor * a[i, j];
                        bb[k] -= factor * bb[i];
                    }
                });

                threads[t].Start();
                startRow = eRow;
            }

            foreach (var thread in threads)
                thread.Join();
        }

        for (int i = N - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < N; j++)
                sum += a[i, j] * x[j];
            x[i] = (bb[i] - sum) / a[i, i];
        }
    }
}
