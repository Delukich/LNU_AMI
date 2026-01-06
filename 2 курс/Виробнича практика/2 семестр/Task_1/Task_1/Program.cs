using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Ship
{
    public string Name { get; set; }
    public string HomePort { get; set; }
    public double Tonnage { get; set; }
    
    public Ship()
    {
        Name = string.Empty;
        HomePort = string.Empty;
        Tonnage = 0.0;
    }

    
    public Ship(string name, string homePort, double tonnage)
    {
        Name = name;
        HomePort = homePort;
        Tonnage = tonnage;
    }

    public override string ToString()
    {
        return $"Ship: {Name}, Home Port: {HomePort}, Tonnage: {Tonnage}";
    }
}

public class ShipManagement
{
    private List<Ship> ships;
    private const string FileName = @"C:\Виробнича практика\Task_1\ships.txt"; // Правильний шлях до файлу

    public ShipManagement()
    {
        ships = new List<Ship>();
    }

    
    public void ReadFromFile()
    {
        try
        {
            if (!File.Exists(FileName))
            {
                Console.WriteLine($"File {FileName} not found. Please check the path and ensure the file exists.");
                return;
            }

            var lines = File.ReadAllLines(FileName);
            int lineNumber = 0;
            foreach (var line in lines)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line)) continue; 

                var parts = line.Split(',', StringSplitOptions.TrimEntries); 
                if (parts.Length != 3)
                {
                    Console.WriteLine($"Skipping invalid line (wrong number of fields) at line {lineNumber}: {line}");
                    continue;
                }

                
                if (double.TryParse(parts[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double tonnage))
                {
                    var ship = new Ship
                    {
                        Name = parts[0],
                        HomePort = parts[1],
                        Tonnage = tonnage
                    };
                    ships.Add(ship);
                    Console.WriteLine($"Successfully parsed line {lineNumber}: {line}");
                }
                else
                {
                    Console.WriteLine($"Skipping invalid line (invalid tonnage) at line {lineNumber}: {line}");
                }
            }
            Console.WriteLine($"Data loaded successfully from {FileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }

    
    public void DisplayAllShips()
    {
        if (!ships.Any())
        {
            Console.WriteLine("No ships in the list.");
            return;
        }

        foreach (var ship in ships)
        {
            Console.WriteLine(ship);
        }
    }

    
    public void SortByName()
    {
        ships = ships.OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase).ToList();
        Console.WriteLine("Sorted by name:");
        DisplayAllShips();
    }

    
    public void SortByHomePort()
    {
        ships = ships.OrderBy(s => s.HomePort, StringComparer.OrdinalIgnoreCase).ToList();
        Console.WriteLine("Sorted by home port:");
        DisplayAllShips();
    }

    
    public void SortByTonnage()
    {
        ships = ships.OrderBy(s => s.Tonnage).ToList();
        Console.WriteLine("Sorted by tonnage:");
        DisplayAllShips();
    }

    
    public void SearchByName(string name)
    {
        var results = ships.Where(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!results.Any())
        {
            Console.WriteLine($"No ship found with name: {name}");
            return;
        }

        Console.WriteLine("Search results:");
        foreach (var ship in results)
        {
            Console.WriteLine(ship);
        }
    }

    // Menu
    public void ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== Menu ===");
            Console.WriteLine("1. Display all ships");
            Console.WriteLine("2. Sort by name");
            Console.WriteLine("3. Sort by home port");
            Console.WriteLine("4. Sort by tonnage");
            Console.WriteLine("5. Search by name");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    DisplayAllShips();
                    break;
                case 2:
                    SortByName();
                    break;
                case 3:
                    SortByHomePort();
                    break;
                case 4:
                    SortByTonnage();
                    break;
                case 5:
                    Console.Write("Enter ship name to search: ");
                    string name = Console.ReadLine()?.Trim() ?? string.Empty;
                    SearchByName(name);
                    break;
                case 6:
                    Console.WriteLine("Exiting program...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public static void Main(string[] args)
    {
        var sm = new ShipManagement();
        sm.ReadFromFile();
        sm.ShowMenu();
    }
}