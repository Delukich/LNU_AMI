using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

class Tour
{
    public string TourName { get; set; }
    public string ClientLastName { get; set; }
    public DateTime StartDate { get; set; }
    public decimal PricePerDay { get; set; }
    public int DaysCount { get; set; }
    public int PersonsCount { get; set; }
    public decimal TravelCost { get; set; }

    public decimal TotalCost => (PricePerDay * DaysCount * PersonsCount) + TravelCost;

    public override string ToString()
    {
        return $"{TourName} - {ClientLastName}, {StartDate:yyyy-MM-dd}, " +
               $"Price/Day: {PricePerDay}, Days: {DaysCount}, Persons: {PersonsCount}, Travel: {TravelCost}";
    }
}

class TourDatabase
{
    private List<Tour> tours = new List<Tour>();
    private string filePath = @"C:\Виробнича практика\Task_6\tours.txt";

    public TourDatabase()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
    }

    public void AddTour(Tour tour)
    {
        if (tour == null) throw new ArgumentNullException(nameof(tour));
        tours.Add(tour);
        SaveToFile();
    }

    public void LoadFromFile()
    {
        try
        {
            if (File.Exists(filePath))
            {
                tours.Clear();
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 7)
                    {
                        tours.Add(new Tour
                        {
                            TourName = parts[0],
                            ClientLastName = parts[1],
                            StartDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            PricePerDay = decimal.Parse(parts[3], CultureInfo.InvariantCulture),
                            DaysCount = int.Parse(parts[4]),
                            PersonsCount = int.Parse(parts[5]),
                            TravelCost = decimal.Parse(parts[6], CultureInfo.InvariantCulture)
                        });
                    }
                }
                Console.WriteLine("Дані успішно завантажено з файлу");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при завантаженні файлу: {ex.Message}");
        }
    }

    public void SaveToFile()
    {
        try
        {
            var lines = tours.Select(t => string.Join(";",
                t.TourName,
                t.ClientLastName,
                t.StartDate.ToString("yyyy-MM-dd"),
                t.PricePerDay.ToString(CultureInfo.InvariantCulture),
                t.DaysCount,
                t.PersonsCount,
                t.TravelCost.ToString(CultureInfo.InvariantCulture)));
            File.WriteAllLines(filePath, lines);
            Console.WriteLine("Дані успішно збережено у файл");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при збереженні файлу: {ex.Message}");
        }
    }

    public void ShowAll()
    {
        if (!tours.Any())
        {
            Console.WriteLine("База даних порожня");
            return;
        }
        foreach (var tour in tours)
        {
            Console.WriteLine(tour);
        }
    }

    public void RemoveTour(string clientLastName)
    {
        int removedCount = tours.RemoveAll(t => t.ClientLastName.Equals(clientLastName, StringComparison.OrdinalIgnoreCase));
        if (removedCount > 0)
        {
            SaveToFile();
            Console.WriteLine($"Видалено {removedCount} записів");
        }
        else
        {
            Console.WriteLine("Тур не знайдено");
        }
    }

    public void ModifyTour(string clientLastName)
    {
        var tour = tours.FirstOrDefault(t => t.ClientLastName.Equals(clientLastName, StringComparison.OrdinalIgnoreCase));
        if (tour == null)
        {
            Console.WriteLine("Тур не знайдено");
            return;
        }

        Console.WriteLine($"Знайдено: {tour}");
        Console.WriteLine("Введіть нові значення (Enter для збереження поточного):");

        tour.TourName = GetInput("Назва туру", tour.TourName);
        tour.ClientLastName = GetInput("Прізвище клієнта", tour.ClientLastName);
        tour.StartDate = DateTime.ParseExact(GetInput("Дата (yyyy-MM-dd)", tour.StartDate.ToString("yyyy-MM-dd")), 
            "yyyy-MM-dd", CultureInfo.InvariantCulture);
        tour.PricePerDay = decimal.Parse(GetInput("Вартість дня", tour.PricePerDay.ToString(CultureInfo.InvariantCulture)), 
            CultureInfo.InvariantCulture);
        tour.DaysCount = int.Parse(GetInput("Кількість днів", tour.DaysCount.ToString()));
        tour.PersonsCount = int.Parse(GetInput("Кількість осіб", tour.PersonsCount.ToString()));
        tour.TravelCost = decimal.Parse(GetInput("Вартість проїзду", tour.TravelCost.ToString(CultureInfo.InvariantCulture)), 
            CultureInfo.InvariantCulture);

        SaveToFile();
        Console.WriteLine("Тур успішно оновлено");
    }

    public void SearchByLastName(string lastName) => Search(t => t.ClientLastName.Contains(lastName, StringComparison.OrdinalIgnoreCase));
    public void SearchByTourName(string tourName) => Search(t => t.TourName.Contains(tourName, StringComparison.OrdinalIgnoreCase));
    public void SearchByStartDate(DateTime startDate) => Search(t => t.StartDate.Date == startDate.Date);

    private void Search(Func<Tour, bool> predicate)
    {
        var results = tours.Where(predicate).ToList();
        ShowResults(results);
    }

    private void ShowResults(List<Tour> results)
    {
        if (results.Any())
        {
            foreach (var tour in results)
                Console.WriteLine(tour);
        }
        else
        {
            Console.WriteLine("Нічого не знайдено");
        }
    }

    private string GetInput(string prompt, string defaultValue)
    {
        Console.Write($"{prompt} [{defaultValue}]: ");
        string input = Console.ReadLine();
        return string.IsNullOrEmpty(input) ? defaultValue : input;
    }
}

class Program
{
    static void Main()
    {
        TourDatabase db = new TourDatabase();
        db.LoadFromFile();

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Додати тур");
            Console.WriteLine("2. Показати всі тури");
            Console.WriteLine("3. Видалити тур");
            Console.WriteLine("4. Пошук за прізвищем");
            Console.WriteLine("5. Пошук за назвою туру");
            Console.WriteLine("6. Пошук за датою");
            Console.WriteLine("7. Модифікувати тур");
            Console.WriteLine("8. Вихід"); // Зміщено нумерацію: 8 замість 9
            Console.Write("Виберіть опцію: ");

            switch (Console.ReadLine())
            {
                case "1":
                    try
                    {
                        db.AddTour(CreateTourFromInput());
                        Console.WriteLine("Тур успішно додано");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Помилка при додаванні туру: {ex.Message}");
                    }
                    break;

                case "2": db.ShowAll(); break;

                case "3":
                    Console.Write("Прізвище клієнта: ");
                    db.RemoveTour(Console.ReadLine());
                    break;

                case "4":
                    Console.Write("Прізвище клієнта: ");
                    db.SearchByLastName(Console.ReadLine());
                    break;

                case "5":
                    Console.Write("Назва туру: ");
                    db.SearchByTourName(Console.ReadLine());
                    break;

                case "6":
                    Console.Write("Дата початку (yyyy-MM-dd): ");
                    try
                    {
                        db.SearchByStartDate(DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Помилка формату дати: {ex.Message}");
                    }
                    break;

                case "7":
                    Console.Write("Прізвище клієнта: ");
                    db.ModifyTour(Console.ReadLine());
                    break;

                case "8": // Зміщено з 9 на 8
                    Console.WriteLine("До побачення!");
                    return;

                default:
                    Console.WriteLine("Невірний вибір");
                    break;
            }
        }
    }

    private static Tour CreateTourFromInput()
    {
        Console.Write("Назва туру: ");
        string tourName = Console.ReadLine();
        Console.Write("Прізвище клієнта: ");
        string lastName = Console.ReadLine();
        Console.Write("Дата початку (yyyy-MM-dd): ");
        DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
        Console.Write("Вартість одного дня: ");
        decimal pricePerDay = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        Console.Write("Кількість днів: ");
        int daysCount = int.Parse(Console.ReadLine());
        Console.Write("Кількість осіб: ");
        int personsCount = int.Parse(Console.ReadLine());
        Console.Write("Вартість проїзду: ");
        decimal travelCost = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

        return new Tour
        {
            TourName = tourName,
            ClientLastName = lastName,
            StartDate = startDate,
            PricePerDay = pricePerDay,
            DaysCount = daysCount,
            PersonsCount = personsCount,
            TravelCost = travelCost
        };
    }
}