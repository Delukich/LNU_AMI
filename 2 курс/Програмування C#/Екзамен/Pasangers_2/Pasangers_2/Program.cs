using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

/*застосування xml, linq i xunit
 • Розробити вказанi типи i статичнi методи, якi реалiзують завдання; статичнi методи, якi реалiзують завдання,
 розробити з використанням linq;
 • Перевiрити функцiональнiсть розроблених класiв i статичних методiв за допомогою xunit, використовуючи Test
 i Class fixtures
 • Виконати поставленi завдання на даних, якi заданi кiлькома xml-файлами, результати завдань перевiрити
 xunit-тестами.
 1. Розробити типи для облiку пасажирських перевезень.
 Маршрут характеризується номером i назвою пункту призначення.
 Автобус характеризується номером i типом (string) пального.
 Пальне характеризується типом (string) i цiною 1 лiтра.
 Водiй характеризується числовим iдентифiкатором i прiзвищем.
 Звiт про здiйснений рейс мiстить номер маршруту, номер автобуса, iдентифiкатор водiя, сумарну вартiсть квиткiв
 та витрату пального в лiтрах.
 Прибутковiсть маршруту оцiнюють як рiзницю сумарної вартостi квиткiв та витрат на пальне.
 Звiти подано кiлькома (не менше 2) окремими xml-файлами.
 2. Вивести:
 (а)xml - файл, де для кожного водiя вказано пункти призначення (без повторень), куди вiн виконував рейси, i
 який сумарний прибуток був на кожному маршрутi; перелiки впорядкувати за прiзвищем водiя та назвою
 пункту у лексико-графiчному порядку;
(б)xml - файл, де для кожного маршруту вказати середнє арифметичне вартостi пального; впорядкувати за
 назвою пункту призначення у лексико-графiчному порядку
*/
namespace TransportReports
{
    public class Marshrut
    {
        public int Number { get; set; }
        public string Destination { get; set; } // Non-nullable, initialized via object initializer
    }

    public class Bus
    {
        public int Number { get; set; }
        public string FuelType { get; set; }
    }

    public class Fuel
    {
        public string Type { get; set; }
        public decimal PricePerLiter { get; set; }
    }

    public class Driver
    {
        public int Id { get; set; }
        public string Surname { get; set; }
    }

    public class Report
    {
        public int MarshrutNumber { get; set; }
        public int BusNumber { get; set; }
        public int DriverId { get; set; }
        public decimal TicketRevenue { get; set; }
        public decimal FuelConsumption { get; set; }
    }

    public static class DataLoader
    {
        public static List<Marshrut> LoadMarshrut(string path) =>
            XDocument.Load(path)
                     .Descendants("Marshrut")
                     .Select(m => new Marshrut
                     {
                         Number = (int)m.Element("Number"),
                         Destination = (string)m.Element("Destination")
                     })
                     .ToList();

        public static List<Bus> LoadBuses(string path) =>
            XDocument.Load(path)
                     .Descendants("Bus")
                     .Select(b => new Bus
                     {
                         Number = (int)b.Element("Number"),
                         FuelType = (string)b.Element("FuelType")
                     })
                     .ToList();

        public static List<Fuel> LoadFuels(string path) =>
            XDocument.Load(path)
                     .Descendants("Fuel")
                     .Select(f => new Fuel
                     {
                         Type = (string)f.Element("Type"),
                         PricePerLiter = (decimal)f.Element("PricePerLiter")
                     })
                     .ToList();

        public static List<Driver> LoadDrivers(string path) =>
            XDocument.Load(path)
                     .Descendants("Driver")
                     .Select(d => new Driver
                     {
                         Id = (int)d.Element("Id"),
                         Surname = (string)d.Element("Surname")
                     })
                     .ToList();

        public static List<Report> LoadReports(string path) =>
            XDocument.Load(path)
                     .Descendants("Report")
                     .Select(r => new Report
                     {
                         MarshrutNumber = (int)r.Element("MarshrutNumber"),
                         BusNumber = (int)r.Element("BusNumber"),
                         DriverId = (int)r.Element("DriverId"),
                         TicketRevenue = (decimal)r.Element("TicketRevenue"),
                         FuelConsumption = (decimal)r.Element("FuelConsumption")
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
                    new XElement("Number", 1),
                    new XElement("Destination", "Київ")),
                new XElement("Marshrut",
                    new XElement("Number", 2),
                    new XElement("Destination", "Львів")),
                new XElement("Marshrut",
                    new XElement("Number", 3),
                    new XElement("Destination", "Одеса"))
            };

            var doc = new XDocument(new XElement("Marshruts", marshrut));
            doc.Save("marshrut.xml");
            Console.WriteLine("marshrut.xml has been generated.");
        }

        public static void GenerateBusesXml()
        {
            var buses = new List<XElement>
            {
                new XElement("Bus",
                    new XElement("Number", 101),
                    new XElement("FuelType", "Дизель")),
                new XElement("Bus",
                    new XElement("Number", 102),
                    new XElement("FuelType", "Бензин"))
            };

            var doc = new XDocument(new XElement("Buses", buses));
            doc.Save("buses.xml");
            Console.WriteLine("buses.xml has been generated.");
        }

        public static void GenerateFuelsXml()
        {
            var fuels = new List<XElement>
            {
                new XElement("Fuel",
                    new XElement("Type", "Дизель"),
                    new XElement("PricePerLiter", 40.0m)),
                new XElement("Fuel",
                    new XElement("Type", "Бензин"),
                    new XElement("PricePerLiter", 45.0m))
            };

            var doc = new XDocument(new XElement("Fuels", fuels));
            doc.Save("fuels.xml");
            Console.WriteLine("fuels.xml has been generated.");
        }

        public static void GenerateDriversXml()
        {
            var drivers = new List<XElement>
            {
                new XElement("Driver",
                    new XElement("Id", 1),
                    new XElement("Surname", "Коваль")),
                new XElement("Driver",
                    new XElement("Id", 2),
                    new XElement("Surname", "Шевченко")),
                new XElement("Driver",
                    new XElement("Id", 3),
                    new XElement("Surname", "Петренко"))
            };

            var doc = new XDocument(new XElement("Drivers", drivers));
            doc.Save("drivers.xml");
            Console.WriteLine("drivers.xml has been generated.");
        }

        public static void GenerateReportsXml(string fileName, int set)
        {
            List<XElement> reports;
            if (set == 1)
            {
                reports = new List<XElement>
                {
                    new XElement("Report",
                        new XElement("MarshrutNumber", 1),
                        new XElement("BusNumber", 101),
                        new XElement("DriverId", 1),
                        new XElement("TicketRevenue", 5000.0m),
                        new XElement("FuelConsumption", 50.0m)),
                    new XElement("Report",
                        new XElement("MarshrutNumber", 2),
                        new XElement("BusNumber", 102),
                        new XElement("DriverId", 2),
                        new XElement("TicketRevenue", 4000.0m),
                        new XElement("FuelConsumption", 40.0m)),
                    new XElement("Report",
                        new XElement("MarshrutNumber", 3),
                        new XElement("BusNumber", 101),
                        new XElement("DriverId", 3),
                        new XElement("TicketRevenue", 6000.0m),
                        new XElement("FuelConsumption", 60.0m))
                };
            }
            else // set == 2
            {
                reports = new List<XElement>
                {
                    new XElement("Report",
                        new XElement("MarshrutNumber", 1),
                        new XElement("BusNumber", 102),
                        new XElement("DriverId", 2),
                        new XElement("TicketRevenue", 4500.0m),
                        new XElement("FuelConsumption", 45.0m)),
                    new XElement("Report",
                        new XElement("MarshrutNumber", 2),
                        new XElement("BusNumber", 101),
                        new XElement("DriverId", 1),
                        new XElement("TicketRevenue", 4200.0m),
                        new XElement("FuelConsumption", 42.0m)),
                    new XElement("Report",
                        new XElement("MarshrutNumber", 3),
                        new XElement("BusNumber", 102),
                        new XElement("DriverId", 3),
                        new XElement("TicketRevenue", 5500.0m),
                        new XElement("FuelConsumption", 55.0m))
                };
            }

            var doc = new XDocument(new XElement("Reports", reports));
            doc.Save(fileName);
            Console.WriteLine($"{fileName} has been generated.");
        }

        public static void GenerateAllXmlFiles()
        {
            GenerateMarshrutXml();
            GenerateBusesXml();
            GenerateFuelsXml();
            GenerateDriversXml();
            GenerateReportsXml("reports1.xml", 1);
            GenerateReportsXml("reports2.xml", 2);
        }
    }

    public static class Tasks
    {
        public static XElement TaskA(List<Driver> drivers, List<Report> reports, List<Marshrut> marshrut, List<Bus> buses, List<Fuel> fuels)
        {
            var result = from d in drivers
                         join r in reports on d.Id equals r.DriverId
                         join m in marshrut on r.MarshrutNumber equals m.Number
                         join b in buses on r.BusNumber equals b.Number
                         join f in fuels on b.FuelType equals f.Type
                         let profit = r.TicketRevenue - (r.FuelConsumption * f.PricePerLiter)
                         group profit by new { d.Id, d.Surname, m.Destination } into routeGroup
                         orderby routeGroup.Key.Surname, routeGroup.Key.Destination
                         select new XElement("Info",
                             new XElement("DriverId", routeGroup.Key.Id),
                             new XElement("Surname", routeGroup.Key.Surname),
                             new XElement("Destination", routeGroup.Key.Destination),
                             new XElement("TotalProfit", routeGroup.Sum())
                         );

            return new XElement("TaskA", result);
        }

        public static XElement TaskB(List<Marshrut> marshrut, List<Report> reports, List<Bus> buses, List<Fuel> fuels)
        {
            var result = from m in marshrut
                         join r in reports on m.Number equals r.MarshrutNumber
                         join b in buses on r.BusNumber equals b.Number
                         join f in fuels on b.FuelType equals f.Type
                         group f.PricePerLiter by new { m.Number, m.Destination } into g
                         orderby g.Key.Destination
                         select new XElement("Info",
                             new XElement("MarshrutNumber", g.Key.Number),
                             new XElement("Destination", g.Key.Destination),
                             new XElement("AverageFuelPrice", g.Average())
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
            var buses = DataLoader.LoadBuses("buses.xml");
            var fuels = DataLoader.LoadFuels("fuels.xml");
            var drivers = DataLoader.LoadDrivers("drivers.xml");
            var reports = DataLoader.LoadReports("reports1.xml")
                         .Concat(DataLoader.LoadReports("reports2.xml"))
                         .ToList();

            var resultA = Tasks.TaskA(drivers, reports, marshrut, buses, fuels);
            resultA.Save("taskA_result.xml");

            var resultB = Tasks.TaskB(marshrut, reports, buses, fuels);
            resultB.Save("taskB_result.xml");

            Console.WriteLine("Звіт taskA_result.xml і taskB_result.xml збережено.");
        }
    }
}