using System.Diagnostics;

class MatrixOperations
{
    static Random rand = new Random();
    
    static int[,] GenerateMatrix(int n, int m)
    {
        int[,] matrix = new int[n, m];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                matrix[i, j] = rand.Next(0, 10); 
        return matrix;
    }

    // Послідовне додавання матриць
    static int[,] AddSequential(int[,] A, int[,] B)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int[,] C = new int[n, m];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                C[i, j] = A[i, j] + B[i, j];
        return C;
    }

    // Послідовне віднімання матриць
    static int[,] SubtractSequential(int[,] A, int[,] B)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int[,] C = new int[n, m];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                C[i, j] = A[i, j] - B[i, j];
        return C;
    }

    // Паралельне додавання матриць
    static int[,] AddParallel(int[,] A, int[,] B, int k)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int[,] C = new int[n, m];

        int rowsPerThread = n / k;
        int remainingRows = n % k;

        Task[] tasks = new Task[k];
        int startRow = 0;

        for (int t = 0; t < k; t++)
        {
            int endRow = startRow + rowsPerThread + (t < remainingRows ? 1 : 0);
            int localStart = startRow;
            int localEnd = endRow;
            tasks[t] = Task.Run(() =>
            {
                for (int i = localStart; i < localEnd; i++)
                    for (int j = 0; j < m; j++)
                        C[i, j] = A[i, j] + B[i, j];
            });
            startRow = endRow;
        }

        Task.WaitAll(tasks);
        return C;
    }

    // Паралельне віднімання матриць
    static int[,] SubtractParallel(int[,] A, int[,] B, int k)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        int[,] C = new int[n, m];

        int rowsPerThread = n / k;
        int remainingRows = n % k;

        Task[] tasks = new Task[k];
        int startRow = 0;

        for (int t = 0; t < k; t++)
        {
            int endRow = startRow + rowsPerThread + (t < remainingRows ? 1 : 0);
            int localStart = startRow;
            int localEnd = endRow;
            tasks[t] = Task.Run(() =>
            {
                for (int i = localStart; i < localEnd; i++)
                    for (int j = 0; j < m; j++)
                        C[i, j] = A[i, j] - B[i, j];
            });
            startRow = endRow;
        }

        Task.WaitAll(tasks);
        return C;
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        int n = 1000; 
        int m = 1000; 
        int k = 16;   

        int[,] A = GenerateMatrix(n, m);
        int[,] B = GenerateMatrix(n, m);

        Stopwatch sw = new Stopwatch();

        // Послідовне додавання
        sw.Start();
        int[,] C1 = AddSequential(A, B);
        sw.Stop();
        double seqAddTime = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"Послідовне додавання: {seqAddTime} ms");

        // Паралельне додавання
        sw.Restart();
        int[,] C2 = AddParallel(A, B, k);
        sw.Stop();
        double parAddTime = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"Паралельне додавання ({k} потоків): {parAddTime} ms");

        // Прискорення та ефективність
        double speedupAdd = seqAddTime / parAddTime;
        double efficiencyAdd = speedupAdd / k * 100;
        Console.WriteLine($"Прискорення: {speedupAdd:F2}");
        Console.WriteLine($"Ефективність: {efficiencyAdd:F2}%");

        // Послідовне віднімання
        sw.Restart();
        int[,] D1 = SubtractSequential(A, B);
        sw.Stop();
        double seqSubTime = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"\nПослідовне віднімання: {seqSubTime} ms");

        // Паралельне віднімання
        sw.Restart();
        int[,] D2 = SubtractParallel(A, B, k);
        sw.Stop();
        double parSubTime = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"Паралельне віднімання ({k} потоків): {parSubTime} ms");

        // Прискорення та ефективність
        double speedupSub = seqSubTime / parSubTime;
        double efficiencySub = speedupSub / k * 100;
        Console.WriteLine($"Прискорення: {speedupSub:F2}");
        Console.WriteLine($"Ефективність: {efficiencySub:F2}%");
    }
}
