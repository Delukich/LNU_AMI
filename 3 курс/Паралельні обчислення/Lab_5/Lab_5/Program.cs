using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        int n = 1000;          
        int k = 8;             
        int a = 12;            
        int b = 78;            
        var rand = new Random(0); 
        
        double[,] graph = new double[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j) graph[i, j] = 0;
                else
                    graph[i, j] = rand.NextDouble() < 0.2 ? rand.Next(10, 100) : double.PositiveInfinity;
            }
        }

        Console.WriteLine($"Граф {n}x{n} ");
        Console.WriteLine($"Вершини для пошуку шляху: a={a}, b={b}\n");

        //ПОСЛІДОВНИЙ АЛГОРИТМ ФЛОЙДА
        double[,] distSeq = (double[,])graph.Clone();
        Stopwatch sw = Stopwatch.StartNew();
        FloydWarshallSequential(distSeq);
        sw.Stop();
        double t1 = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"Послідовно: {t1:F2} мс");
        Console.WriteLine($"Найкоротший шлях від {a} до {b}: {distSeq[a, b]}\n");

        //ПАРАЛЕЛЬНИЙ АЛГОРИТМ ФЛОЙДА
        double[,] distPar = (double[,])graph.Clone();
        sw.Restart();
        FloydWarshallParallelThreads(distPar, k);
        sw.Stop();
        double tk = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"Паралельно ({k} потоків): {tk:F2} мс");
        Console.WriteLine($"Найкоротший шлях від {a} до {b}: {distPar[a, b]}\n");

        //ПРИСКОРЕННЯ І ЕФЕКТИВНОСТІ
        double speedup = t1 / tk;
        double efficiency = speedup / k;

        Console.WriteLine("Показники продуктивності");
        Console.WriteLine($"Прискорення (S{k}) = {speedup:F3}");
        Console.WriteLine($"Ефективність (E{k}) = {efficiency * 100:F2}%");
    }

    //ПОСЛІДОВНИЙ АЛГОРИТМ ФЛОЙДА
    static void FloydWarshallSequential(double[,] dist)
    {
        int n = dist.GetLength(0);
        for (int k = 0; k < n; k++)
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (dist[i, k] + dist[k, j] < dist[i, j])
                        dist[i, j] = dist[i, k] + dist[k, j];
    }

    //ПАРАЛЕЛЬНИЙ АЛГОРИТМ ФЛОЙДА
    static void FloydWarshallParallelThreads(double[,] dist, int numThreads)
    {
        int n = dist.GetLength(0);

        for (int k = 0; k < n; k++)
        {
            Thread[] threads = new Thread[numThreads];
            int rowsPerThread = n / numThreads;

            for (int t = 0; t < numThreads; t++)
            {
                int start = t * rowsPerThread;
                int end = (t == numThreads - 1) ? n : start + rowsPerThread; 

                threads[t] = new Thread(() =>
                {
                    for (int i = start; i < end; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            if (dist[i, k] + dist[k, j] < dist[i, j])
                                dist[i, j] = dist[i, k] + dist[k, j];
                        }
                    }
                });
                threads[t].Start();
            }
            
            foreach (var thread in threads)
                thread.Join();
        }
    }
}
