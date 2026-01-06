using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TransportSystem
{
    public class Marshrut
    {
        public int Number { get; set; }
        public string StartStop { get; set; }
        public string EndStop { get; set; }
    }

    public class Passenger
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public int CategoryId { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }

    public class Account
    {
        public DateTime Date { get; set; }
        public int PassengerId { get; set; }
        public int MarshrutNumber { get; set; }
    }

    public static class DataLoader
    {
        public static List<Marshrut> LoadMarshrut(string path) =>
            XDocument.Load(path)
                     .Descendants("Marshrut")
                     .Select(m => new Marshrut
                     {
                         Number = m.Element("Number") != null ? (int)m.Element("Number") : 0,
                         StartStop = m.Element("StartStop")?.Value ?? string.Empty,
                         EndStop = m.Element("EndStop")?.Value ?? string.Empty
                     })
                     .ToList();

        public static List<Passenger> LoadPassengers(string path) =>
            XDocument.Load(path)
                     .Descendants("Passenger")
                     .Select(p => new Passenger
                     {
                         Id = p.Element("Id") != null ? (int)p.Element("Id") : 0,
                         Surname = p.Element("Surname")?.Value ?? string.Empty,
                         CategoryId = p.Element("CategoryId") != null ? (int)p.Element("CategoryId") : 0
                     })
                     .ToList();

        public static List<Category> LoadCategories(string path) =>
            XDocument.Load(path)
                     .Descendants("Category")
                     .Select(c => new Category
                     {
                         Id = c.Element("Id") != null ? (int)c.Element("Id") : 0,
                         Name = c.Element("Name")?.Value ?? string.Empty,
                         Cost = c.Element("Cost") != null ? (decimal)c.Element("Cost") : 0m
                     })
                     .ToList();

        public static List<Account> LoadAccounts(string path) =>
            XDocument.Load(path)
                     .Descendants("Account")
                     .Select(a => new Account
                     {
                         Date = a.Element("Date") != null ? (DateTime)a.Element("Date") : DateTime.MinValue,
                         PassengerId = a.Element("PassengerId") != null ? (int)a.Element("PassengerId") : 0,
                         MarshrutNumber = a.Element("MarshrutNumber") != null ? (int)a.Element("MarshrutNumber") : 0
                     })
                     .ToList();
    }

    public static class DataGenerator
    {
        public static void GenerateMarshrutXml()
        {
            var marshrut = new List<XElement>
            {
                new XElement("Marshrut",
                    new XElement("Number", 101),
                    new XElement("StartStop", "Центр"),
                    new XElement("EndStop", "Вокзал")),
                new XElement("Marshrut",
                    new XElement("Number", 102),
                    new XElement("StartStop", "Аеропорт"),
                    new XElement("EndStop", "Парк")),
                new XElement("Marshrut",
                    new XElement("Number", 103),
                    new XElement("StartStop", "Ботсад"),
                    new XElement("EndStop", "Музей"))
            };

            var doc = new XDocument(new XElement("Marshruts", marshrut));
            doc.Save("marshrut.xml");
            Console.WriteLine("marshrut.xml has been generated.");
        }

        public static void GeneratePassengersXml()
        {
            var passengers = new List<XElement>
            {
                new XElement("Passenger",
                    new XElement("Id", 1),
                    new XElement("Surname", "Коваль"),
                    new XElement("CategoryId", 1)),
                new XElement("Passenger",
                    new XElement("Id", 2),
                    new XElement("Surname", "Шевченко"),
                    new XElement("CategoryId", 1)),
                new XElement("Passenger",
                    new XElement("Id", 3),
                    new XElement("Surname", "Петренко"),
                    new XElement("CategoryId", 2))
            };

            var doc = new XDocument(new XElement("Passengers", passengers));
            doc.Save("passengers.xml");
            Console.WriteLine("passengers.xml has been generated.");
        }

        public static void GenerateCategoriesXml()
        {
            var categories = new List<XElement>
            {
                new XElement("Category",
                    new XElement("Id", 1),
                    new XElement("Name", "Пільговий"),
                    new XElement("Cost", 5.0m)),
                new XElement("Category",
                    new XElement("Id", 2),
                    new XElement("Name", "Стандарт"),
                    new XElement("Cost", 10.0m))
            };

            var doc = new XDocument(new XElement("Categories", categories));
            doc.Save("categories.xml");
            Console.WriteLine("categories.xml has been generated.");
        }

        public static void GenerateAccountsXml(string fileName, int set)
        {
            List<XElement> accounts;
            if (set == 1)
            {
                accounts = new List<XElement>
                {
                    new XElement("Account",
                        new XElement("Date", new DateTime(2025, 1, 10)),
                        new XElement("PassengerId", 1),
                        new XElement("MarshrutNumber", 101)),
                    new XElement("Account",
                        new XElement("Date", new DateTime(2025, 1, 15)),
                        new XElement("PassengerId", 2),
                        new XElement("MarshrutNumber", 102)),
                    new XElement("Account",
                        new XElement("Date", new DateTime(2025, 1, 20)),
                        new XElement("PassengerId", 3),
                        new XElement("MarshrutNumber", 103))
                };
            }
            else // set == 2
            {
                accounts = new List<XElement>
                {
                    new XElement("Account",
                        new XElement("Date", new DateTime(2025, 2, 5)),
                        new XElement("PassengerId", 1),
                        new XElement("MarshrutNumber", 102)),
                    new XElement("Account",
                        new XElement("Date", new DateTime(2025, 2, 10)),
                        new XElement("PassengerId", 2),
                        new XElement("MarshrutNumber", 101)),
                    new XElement("Account",
                        new XElement("Date", new DateTime(2025, 2, 15)),
                        new XElement("PassengerId", 3),
                        new XElement("MarshrutNumber", 103))
                };
            }

            var doc = new XDocument(new XElement("Accounts", accounts));
            doc.Save(fileName);
            Console.WriteLine($"{fileName} has been generated.");
        }

        public static void GenerateAllXmlFiles()
        {
            GenerateMarshrutXml();
            GeneratePassengersXml();
            GenerateCategoriesXml();
            GenerateAccountsXml("accounts1.xml", 1);
            GenerateAccountsXml("accounts2.xml", 2);
        }
    }

    public static class Tasks
    {
        public static XElement TaskA(List<Marshrut> marshrut, List<Account> accounts)
        {
            var result = from m in marshrut
                         join a in accounts on m.Number equals a.MarshrutNumber into routeAccounts
                         orderby m.StartStop
                         select new XElement("Info",
                             new XElement("MarshrutNumber", m.Number),
                             new XElement("StartStop", m.StartStop),
                             new XElement("PassengerCount", routeAccounts.Count())
                         );

            return new XElement("TaskA", result);
        }

        public static XElement TaskB(List<Passenger> passengers, List<Category> categories, List<Account> accounts)
        {
            var fullData = from a in accounts
                           join p in passengers on a.PassengerId equals p.Id
                           join c in categories on p.CategoryId equals c.Id
                           group new { a.Date, c.Cost } by new { p.Id, p.Surname, a.Date.Year, a.Date.Month } into g
                           select new
                           {
                               PassengerId = g.Key.Id,
                               Surname = g.Key.Surname,
                               Year = g.Key.Year,
                               Month = g.Key.Month,
                               TotalCost = g.Sum(x => x.Cost)
                           };

            var result = from entry in fullData
                         group entry by new { entry.PassengerId, entry.Surname } into pg
                         let maxMonthlyCost = pg.Max(x => x.TotalCost)
                         let bestMonth = pg.First(x => x.TotalCost == maxMonthlyCost)
                         orderby pg.Key.Surname
                         select new XElement("Info",
                             new XElement("PassengerId", pg.Key.PassengerId),
                             new XElement("Surname", pg.Key.Surname),
                             new XElement("Month", $"{bestMonth.Year}-{bestMonth.Month:D2}"),
                             new XElement("MaxTotalCost", maxMonthlyCost)
                         );

            return new XElement("TaskB", result);
        }
    }

    class Program
    {
        static void Main()
        {
            DataGenerator.GenerateAllXmlFiles();
            var marshrut = DataLoader.LoadMarshrut("marshrut.xml");
            var passengers = DataLoader.LoadPassengers("passengers.xml");
            var categories = DataLoader.LoadCategories("categories.xml");
            var accounts = DataLoader.LoadAccounts("accounts1.xml")
                          .Concat(DataLoader.LoadAccounts("accounts2.xml"))
                          .ToList();

            var resultA = Tasks.TaskA(marshrut, accounts);
            resultA.Save("taskA_result.xml");

            var resultB = Tasks.TaskB(passengers, categories, accounts);
            resultB.Save("taskB_result.xml");

            Console.WriteLine("Звіт taskA_result.xml і taskB_result.xml збережено.");
        }
    }
}