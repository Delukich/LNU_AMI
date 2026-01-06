using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DebtorsApp
{
    public class Debtor
    {
        public string Surname { get; set; }
        public int ApartmentNumber { get; set; }
        public double Debt { get; set; }

        public int EntranceNumber => (ApartmentNumber - 1) / 36 + 1;
    }

    class Program
    {
        static void Main()
        {
            string path = Path.Combine("C:", "Виробнича практика", "Task_8", "debtors.txt");

            if (!File.Exists(path))
            {
                Console.WriteLine("Файл не знайдено!");
                return;
            }

            List<Debtor> debtors = new List<Debtor>();
            try
            {
                var lines = File.ReadAllLines(path)
                    .Where(line => !string.IsNullOrWhiteSpace(line));

                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length != 3)
                    {
                        Console.WriteLine($"Невірний формат рядка: {line}");
                        continue;
                    }

                    int apartmentNumber;
                    if (!int.TryParse(parts[1].Trim(), out apartmentNumber) || apartmentNumber < 1 || apartmentNumber > 144)
                    {
                        Console.WriteLine($"Квартира {parts[1].Trim()} не враховується: номер квартири має бути від 1 до 144.");
                        continue;
                    }

                    double debt;
                    if (!double.TryParse(parts[2].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out debt))
                    {
                        Console.WriteLine($"Невірний формат заборгованості: {parts[2].Trim()}");
                        continue;
                    }

                    debtors.Add(new Debtor
                    {
                        Surname = parts[0].Trim(),
                        ApartmentNumber = apartmentNumber,
                        Debt = debt
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при обробці файлу: {ex.Message}");
                return;
            }

            if (!debtors.Any())
            {
                Console.WriteLine("Немає даних для обробки.");
                return;
            }

            double averageDebt = debtors.Average(d => d.Debt);

            Console.WriteLine($"Середня заборгованість: {averageDebt:F2}\n");

            var result = debtors
                .Where(d => d.Debt > averageDebt)
                .OrderBy(d => d.EntranceNumber)
                .ThenBy(d => d.ApartmentNumber);

            Console.WriteLine("Боржники з заборгованістю вище середньої:");

            foreach (var d in result)
            {
                Console.WriteLine($"Під'їзд: {d.EntranceNumber}, Квартира: {d.ApartmentNumber}, Прізвище: {d.Surname}, Заборгованість: {d.Debt:F2}");
            }
        }
    }
}