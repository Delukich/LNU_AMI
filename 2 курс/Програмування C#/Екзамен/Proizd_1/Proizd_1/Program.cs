using System;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

/*застосування xml, ling i xunit 
Гозробити вашей эти і статичий методи, над реклізують завдання У вигини обекти Хеленоnt 
итичних методів за дремогою ховій, виниристовуючи Тия Cam 
Перевірити функаюѣльність 
Виконати поставлені завдання на даних, які швдий кількома хов-файлами, регультати лють статичні методи, перевірити xuпін-те 
1. Розробити тапи для обліку оплати проїзду комунальному транспорeй. 
Маршрут характеризуетьем номером, пастами початковий кінцевої зупинок. 
Пасажир характеризується числовим ідентифікатором, прізвищем та ідентифікатором катетра Категорія характеризується числовим ідентифікатором, наивною та вартістю оплати однієї поїздки. 
Обліковий запис про оплату проїзду мастить дату, Ідентифікатор пасажира та номер маршруту. 
Обліковий записи про оплату подано кількома (не менше 2) окремими хи-файлами. 
2. Отримати: 
(а)об'єкт типу XElement де для кожного пасажира вказано кількість його поїздок за оплаченими маршру тами, цей результат також вивести у хті-файл; переліки впорядкувати за прізвищем пасажира у лексико-
графічному порядку без повторень і за зростанням номера маршруту: 
(6) об'єкт типу XElement де для кожного місяця вказати маршрут з найбільшою сумою оплати, яку також включити у результат; отриманий результат також вивести у хті-файл; переліки впорядкувати за порядком місяців.
*/
namespace ELECTRIFICATION_WORKS
{
    // Клас для маршрутів
    public class Route
    {
        public int Number { get; set; }
        public string StartStop { get; set; }
        public string EndStop { get; set; }
    }

    // Клас для пасажирів
    public class Passenger
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public int CategoryId { get; set; }
    }

    // Клас для категорій
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Fare { get; set; }
    }

    // Клас для записів про оплату
    public class PaymentRecord
    {
        public DateTime Date { get; set; }
        public int PassengerId { get; set; }
        public int RouteNumber { get; set; }
    }

    // Клас для завантаження даних з XML
    public static class DataLoader
    {
        public static List<Route> LoadRoute(string path) =>
            XDocument.Load(path)
                     .Descendants("Route")
                     .Select(r => new Route
                     {
                         Number = (int)r.Element("Number"),
                         StartStop = (string)r.Element("StartStop"),
                         EndStop = (string)r.Element("EndStop"),
                     })
                     .ToList();

        public static List<Passenger> LoadPassenger(string path) =>
            XDocument.Load(path)
                     .Descendants("Passenger")
                     .Select(p => new Passenger
                     {
                         Id = (int)p.Element("Id"),
                         LastName = (string)p.Element("LastName"),
                         CategoryId = (int)p.Element("CategoryId")
                     })
                     .ToList();

        public static List<Category> LoadCategory(string path) =>
            XDocument.Load(path)
                     .Descendants("Category")
                     .Select(c => new Category
                     {
                         Id = (int)c.Element("Id"),
                         Name = (string)c.Element("Name"),
                         Fare = (int)c.Element("Fare")
                     })
                     .ToList();

        public static List<PaymentRecord> LoadPaymentRecord(string path) =>
            XDocument.Load(path)
                     .Descendants("PaymentRecord")
                     .Select(p => new PaymentRecord
                     {
                         Date = (DateTime)p.Element("Date"),
                         PassengerId = (int)p.Element("PassengerId"),
                         RouteNumber = (int)p.Element("RouteNumber")
                     })
                     .ToList();
    }

    // Клас для генерації тестових XML-файлів
    public static class DataGenerator
    {
        public static void GenerateRoutesXML()
        {
            var routes = new List<XElement>
            {
                new XElement("Route",
                    new XElement("Number", 1),
                    new XElement("StartStop", "First"),
                    new XElement("EndStop", "Last")),
                new XElement("Route",
                    new XElement("Number", 2),
                    new XElement("StartStop", "Unik"),
                    new XElement("EndStop", "Suxiv")),
                new XElement("Route",
                    new XElement("Number", 3),
                    new XElement("StartStop", "Center"),
                    new XElement("EndStop", "Air")),
            };

            new XDocument(new XElement("Routes", routes)).Save("routes.xml");
            Console.WriteLine("routes.xml has been generated.");
        }

        public static void GeneratePassengersXML()
        {
            var passengers = new List<XElement>
            {
                new XElement("Passenger",
                    new XElement("Id", 1),
                    new XElement("LastName", "Luki"),
                    new XElement("CategoryId", 2)),
                new XElement("Passenger",
                    new XElement("Id", 2),
                    new XElement("LastName", "Patrul"),
                    new XElement("CategoryId", 1)),
                new XElement("Passenger",
                    new XElement("Id", 3),
                    new XElement("LastName", "Alda"),
                    new XElement("CategoryId", 3)),
            };

            new XDocument(new XElement("Passengers", passengers)).Save("passengers.xml");
            Console.WriteLine("passengers.xml has been generated.");
        }

        public static void GenerateCategorysXML()
        {
            var categories = new List<XElement>
            {
                new XElement("Category",
                    new XElement("Id", 1),
                    new XElement("Name", "Train"),
                    new XElement("Fare", 45)),
                new XElement("Category",
                    new XElement("Id", 3),
                    new XElement("Name", "Plane"),
                    new XElement("Fare", 120)),
                new XElement("Category",
                    new XElement("Id", 2),
                    new XElement("Name", "Car"),
                    new XElement("Fare", 12)),
            };

            new XDocument(new XElement("Categories", categories)).Save("categorys.xml");
            Console.WriteLine("categorys.xml has been generated.");
        }

        public static void GeneratePaymentRecordsXML(string fileName, int set)
        {
            List<XElement> paymentRecords;

            if (set == 1)
            {
                paymentRecords = new List<XElement>
                {
                    new XElement("PaymentRecord",
                        new XElement("Date", new DateTime(2024, 01, 06)),
                        new XElement("PassengerId", 2),
                        new XElement("RouteNumber", 1)),
                    new XElement("PaymentRecord",
                        new XElement("Date", new DateTime(2025, 07, 02)),
                        new XElement("PassengerId", 1),
                        new XElement("RouteNumber", 3)),
                    new XElement("PaymentRecord",
                        new XElement("Date", new DateTime(2022, 10, 10)),
                        new XElement("PassengerId", 3),
                        new XElement("RouteNumber", 2)),
                };
            }
            else
            {
                paymentRecords = new List<XElement>
                {
                    new XElement("PaymentRecord",
                        new XElement("Date", new DateTime(2024, 03, 11)),
                        new XElement("PassengerId", 1),
                        new XElement("RouteNumber", 1)),
                    new XElement("PaymentRecord",
                        new XElement("Date", new DateTime(2023, 12, 09)),
                        new XElement("PassengerId", 3),
                        new XElement("RouteNumber", 3)),
                    new XElement("PaymentRecord",
                        new XElement("Date", new DateTime(2022, 04, 12)),
                        new XElement("PassengerId", 2),
                        new XElement("RouteNumber", 3)),
                };
            }

            var doc = new XDocument(new XElement("PaymentRecords", paymentRecords));
            doc.Save(fileName);
            Console.WriteLine($"{fileName} has been generated.");
        }

        public static void GenerateAllXML()
        {
            GenerateRoutesXML();
            GeneratePassengersXML();
            GenerateCategorysXML();
            GeneratePaymentRecordsXML("paymentRecords1.xml", 1);
            GeneratePaymentRecordsXML("paymentRecords2.xml", 2);
        }
    }

    // Клас для обробки звітів
    public static class ReportProcessor
    {
        // Завдання (а): Кількість поїздок пасажира за маршрутами
        public static XElement TaskA(List<Route> routes, List<Passenger> passengers, List<PaymentRecord> paymentRecords)
        {
            var result = new XElement("PassengerTrips",
                from p in passengers
                orderby p.LastName
                select new XElement("Passenger",
                    new XElement("Id", p.Id),
                    new XElement("LastName", p.LastName),
                    new XElement("Trips",
                        from payment in paymentRecords
                        where payment.PassengerId == p.Id
                        group payment by payment.RouteNumber into g
                        orderby g.Key
                        select new XElement("Route",
                            new XElement("RouteNumber", g.Key),
                            new XElement("TripCount", g.Count())
                        )
                    )
                )
            );

            result.Save("taskA_result.xml");
            return result;
        }

        // Завдання (б): Маршрут з найбільшою сумою оплати за місяць
        public static XElement TaskB(List<PaymentRecord> paymentRecords, List<Category> categories, List<Passenger> passengers)
        {
            var result = new XElement("MonthlyTopRoutes",
                from payment in paymentRecords
                group payment by new { payment.Date.Year, payment.Date.Month } into g
                orderby g.Key.Year, g.Key.Month
                select new XElement("Month",
                    new XElement("Year", g.Key.Year),
                    new XElement("MonthNumber", g.Key.Month),
                    new XElement("TopRoute",
                        (from p in g
                         join passenger in passengers on p.PassengerId equals passenger.Id
                         join category in categories on passenger.CategoryId equals category.Id
                         group new { p.RouteNumber, category.Fare } by p.RouteNumber into routeGroup
                         let totalFare = routeGroup.Sum(x => x.Fare)
                         orderby totalFare descending
                         select new XElement("Route",
                             new XElement("RouteNumber", routeGroup.Key),
                             new XElement("TotalFare", totalFare)
                         )).FirstOrDefault()
                    )
                )
            );

            result.Save("taskB_result.xml");
            return result;
        }
    }

    // Основна програма
    public class Program
    {
        public static void Main()
        {
            DataGenerator.GenerateAllXML();

            var tickets = DataLoader.LoadPaymentRecord("paymentRecords1.xml")
                          .Concat(DataLoader.LoadPaymentRecord("paymentRecords2.xml"))
                          .ToList();
            var routes = DataLoader.LoadRoute("routes.xml");
            var passengers = DataLoader.LoadPassenger("passengers.xml");
            var categories = DataLoader.LoadCategory("categorys.xml");

            var resultA = ReportProcessor.TaskA(routes, passengers, tickets);
            Console.WriteLine("Task A result saved to taskA_result.xml");

            var resultB = ReportProcessor.TaskB(tickets, categories, passengers);
            Console.WriteLine("Task B result saved to taskB_result.xml");

            Console.WriteLine("Reports taskA_result.xml and taskB_result.xml have been saved.");
        }
    }
}