using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public class Computer : IComparable<Computer>
{
    
    public string ProcessorType { get; set; }
    public double Performance { get; set; } 
    public int RamSize { get; set; }       
    
    public Computer() { }
    
    public Computer(string processorType, double performance, int ramSize)
    {
        ProcessorType = processorType;
        Performance = performance;
        RamSize = ramSize;
    }
    
    public int CompareTo(Computer other)
    {
        if (other == null) return 1;
        return Performance.CompareTo(other.Performance);
    }
    
    public override string ToString()
    {
        return $"Processor: {ProcessorType}, Performance: {Performance} GHz, RAM: {RamSize} GB";
    }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Computer other = (Computer)obj;
        return ProcessorType == other.ProcessorType &&
               Performance == other.Performance &&
               RamSize == other.RamSize;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(ProcessorType, Performance, RamSize);
    }
}

public class ComputerCollection : IEnumerable<Computer>
{
    private List<Computer> computers;
    
    public ComputerCollection()
    {
        computers = new List<Computer>();
    }
    
    public void Add(Computer computer)
    {
        computers.Add(computer);
    }
    
    public bool Remove(Computer computer)
    {
        return computers.Remove(computer);
    }
    
    public void LoadFromFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл {filePath} не знайдено!");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 3)
                {
                    string procType = parts[0].Trim();
                    
                    if (!double.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double performance))
                    {
                        Console.WriteLine($"Помилка парсингу швидкодії в рядку: {line}");
                        continue;
                    }
                    
                    if (!int.TryParse(parts[2].Trim(), out int ramSize))
                    {
                        Console.WriteLine($"Помилка парсингу RAM в рядку: {line}");
                        continue;
                    }

                    computers.Add(new Computer(procType, performance, ramSize));
                }
                else
                {
                    Console.WriteLine($"Невірний формат рядка: {line}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при читанні файлу: {ex.Message}");
        }
    }
    
    public void SortByPerformance()
    {
        computers.Sort();
    }
    
    public void SortByRam()
    {
        computers.Sort((x, y) => x.RamSize.CompareTo(y.RamSize));
    }
    
    public void SortByProcessorType()
    {
        computers.Sort((x, y) => string.Compare(x.ProcessorType, y.ProcessorType));
    }
    
    public List<Computer> FindByPerformance(double minPerformance)
    {
        return computers.FindAll(c => c.Performance >= minPerformance);
    }

    
    public List<Computer> FindByRam(int minRam)
    {
        return computers.FindAll(c => c.RamSize >= minRam);
    }
    
    public IEnumerator<Computer> GetEnumerator()
    {
        return computers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public int Count => computers.Count;
}

class Program
{
    static void Main()
    {
        ComputerCollection collection = new ComputerCollection();
        
        collection.LoadFromFile(@"C:\Виробнича практика\Task_4\computers.txt");
        
        Console.WriteLine("Всі комп'ютери:");
        if (collection.Count == 0)
        {
            Console.WriteLine("Колекція порожня.");
        }
        else
        {
            foreach (var computer in collection)
            {
                Console.WriteLine(computer);
            }
        }
        
        Console.WriteLine("\nСортування за швидкодією:");
        collection.SortByPerformance();
        foreach (var computer in collection)
        {
            Console.WriteLine(computer);
        }
        
        Console.WriteLine("\nСортування за розміром RAM:");
        collection.SortByRam();
        foreach (var computer in collection)
        {
            Console.WriteLine(computer);
        }
        Console.WriteLine("\n");
        
        Console.WriteLine("\nСортування за типом процесора:");
        collection.SortByProcessorType();
        foreach (var computer in collection)
        {
            Console.WriteLine(computer);
        }
        
        Console.WriteLine("\nПошук комп'ютерів з RAM >= 12GB:");
        var foundByRam = collection.FindByRam(12);
        if (foundByRam.Count == 0)
        {
            Console.WriteLine("Комп'ютери з RAM >= 12GB не знайдені.");
        }
        else
        {
            foreach (var computer in foundByRam)
            {
                Console.WriteLine(computer);
            }
        }
        
        Console.WriteLine("\nПошук комп'ютерів з швидкодією >= 3.0 GHz:");
        var foundByPerformance = collection.FindByPerformance(3.0);
        if (foundByPerformance.Count == 0)
        {
            Console.WriteLine("Комп'ютери з швидкодією >= 3.0 GHz не знайдені.");
        }
        else
        {
            foreach (var computer in foundByPerformance)
            {
                Console.WriteLine(computer);
            }
        }
    }
}