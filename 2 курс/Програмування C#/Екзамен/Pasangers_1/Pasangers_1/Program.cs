using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
 (а)xml - файл, де для кожного маршруту вказано сумарний прибуток кожного водiя; перелiки впорядкувати за
 назвою пункту призначення та прiзвищем водiя у лексико-графiчному порядку без повторень;
(б)xml - файл, де для кожного водiя вказати маршрути з найбiльшим прибутком; перелiк впорядкувати за
 прiзвищем водiя у лексико-графiчному порядку без повторень*/

namespace PASANGERS
{
    public class Route
    {
        public int Number { get; set; }
        public string Destination { get; set; }
    }


    public class Bus
    {
        public int Number { get; set; }
        public string FuelType { get; set; }
    }


    public class Fuel
    {
        public string Type { get; set; }
        public double PricePerLiter { get; set; }
    }


    public class Driver
    {
        public int Id { get; set; }
        public string Surname { get; set; }
    }


    public class Report
    {
        public int RouteNumber { get; set; }
        public int BusNumber { get; set; }
        public int DriverId { get; set; }
        public double TicketIncome { get; set; }
        public double Fuels { get; set; }
    }


    public static class DataLoader
    {
        public static List<Route> LoadRoutes(string path) =>
             XDocument.Load(path)
                      .Descendants("Route")
                      .Select(t => new Route
                      {
                          Number = (int)t.Element("Number"),
                          Destination = (string)t.Element("Destination")
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
                         PricePerLiter = (double)f.Element("PricePerLiter")
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
                         RouteNumber = (int)r.Element("RouteNumber"),
                         BusNumber = (int)r.Element("BusNumber"),
                         DriverId = (int)r.Element("DriverId"),
                         TicketIncome = (double)r.Element("TicketIncome"),
                         Fuels = (double)r.Element("Fuel")
                     })
                     .ToList();
    }



    public static class ReportProcessor
    {
        public static XElement TaskA(
            List<Route> routes,
            List<Bus> buses,
            List<Fuel> fuels,
            List<Driver> drivers,
            List<Report> reports)
        {
            var result = from report in reports
                         join route in routes on report.RouteNumber equals route.Number
                         join bus in buses on report.BusNumber equals bus.Number
                         join fuel in fuels on bus.FuelType equals fuel.Type
                         join driver in drivers on report.DriverId equals driver.Id
                         group new
                         {
                             Profit = report.TicketIncome - report.Fuels * fuel.PricePerLiter,
                             route.Destination,
                             driver.Surname
                         }
                         by new { route.Number, route.Destination, driver.Id, driver.Surname } into g
                         orderby g.Key.Destination, g.Key.Surname
                         select new XElement("ProfitInfo",
                             new XElement("RouteNumber", g.Key.Number),
                             new XElement("Destination", g.Key.Destination),
                             new XElement("DriverId", g.Key.Id),
                             new XElement("DriverSurname", g.Key.Surname),
                             new XElement("TotalProfit", g.Sum(x => x.Profit))
                         );

            return new XElement("Profits", result);
        }

        public static XElement TaskB(
            List<Route> routes,
            List<Bus> buses,
            List<Fuel> fuels,
            List<Driver> drivers,
            List<Report> reports)
        {
            var fuelPrices = fuels.ToDictionary(f => f.Type, f => f.PricePerLiter);
            var busFuelTypes = buses.ToDictionary(b => b.Number, b => b.FuelType);

            var topRoutes = reports
                .GroupBy(r => r.DriverId)
                .Select(driverGroup =>
                {
                    var driverId = driverGroup.Key;

                    var routeProfits = driverGroup
                        .GroupBy(r => r.RouteNumber)
                        .Select(routeGroup =>
                        {
                            var routeNumber = routeGroup.Key;
                            var totalProfit = routeGroup.Sum(r =>
                            {
                                var fuelType = busFuelTypes[r.BusNumber];
                                var fuelCost = r.Fuels * fuelPrices[fuelType];
                                return r.TicketIncome - fuelCost;
                            });

                            return new
                            {
                                RouteNumber = routeNumber,
                                Profit = totalProfit
                            };
                        })
                        .OrderByDescending(r => r.Profit)
                        .FirstOrDefault();

                    var driver = drivers.First(d => d.Id == driverId);
                    var route = routes.First(r => r.Number == routeProfits.RouteNumber);

                    return new XElement("Driver",
                        new XAttribute("Surname", driver.Surname),
                        new XElement("TopRoute",
                        new XAttribute("RouteNumber", route.Number),
                        new XAttribute("Destination", route.Destination),
                        new XAttribute("Profit", routeProfits.Profit.ToString("F2"))
                        )
                    );
                });

            return new XElement("TopRoutesByDriver", topRoutes);
        }
    }


    public class DataGenerator
    {
        public static void GenerateRoutesXML()
        {
            var routes = new List<XElement>
        {
            new XElement("Route",
                new XElement("Number", 1),
                new XElement("Destination", "Kyiv")),
            new XElement("Route",
                new XElement("Number", 2),
                new XElement("Destination", "Lviv")),
            new XElement("Route",
                new XElement("Number", 3),
                new XElement("Destination", "Odesa"))
        };

            var doc = new XDocument(new XElement("Routes", routes));
            doc.Save("routes.xml");
            Console.WriteLine("routes.xml has been generated.");
        }

        public static void GenerateBusesXML()
        {
            var buses = new List<XElement>
        {
            new XElement("Bus",
                new XElement("Number", 1),
                new XElement("FuelType", "Diesel")),
            new XElement("Bus",
                new XElement("Number", 2),
                new XElement("FuelType", "Gas")),
            new XElement("Bus",
                new XElement("Number", 3),
                new XElement("FuelType", "Electric"))
        };

            var doc = new XDocument(new XElement("Buses", buses));
            doc.Save("buses.xml");
            Console.WriteLine("buses.xml has been generated.");
        }

        public static void GenerateFuelsXML()
        {
            var fuels = new List<XElement>
        {
            new XElement("Fuel",
                new XElement("Type", "Diesel"),
                new XElement("PricePerLiter", 1.5)),
            new XElement("Fuel",
                new XElement("Type", "Gas"),
                new XElement("PricePerLiter", 1.0)),
            new XElement("Fuel",
                new XElement("Type", "Electric"),
                new XElement("PricePerLiter", 0.5))
        };

            var doc = new XDocument(new XElement("Fuels", fuels));
            doc.Save("fuels.xml");
            Console.WriteLine("fuels.xml has been generated.");
        }

        public static void GenerateDriversXML()
        {
            var drivers = new List<XElement>
        {
            new XElement("Driver",
                new XElement("Id", 1),
                new XElement("Surname", "Shevchenko")),
            new XElement("Driver",
                new XElement("Id", 2),
                new XElement("Surname", "Koval")),
            new XElement("Driver",
                new XElement("Id", 3),
                new XElement("Surname", "Bondarenko"))
        };

            var doc = new XDocument(new XElement("Drivers", drivers));
            doc.Save("drivers.xml");
            Console.WriteLine("drivers.xml has been generated.");
        }

        public static void GenerateReportsXML(string fileName, int reportSet)
        {
            List<XElement> reports;

            if (reportSet == 1)
            {
                reports = new List<XElement>
            {
                new XElement("Report",
                    new XElement("RouteNumber", 1),
                    new XElement("BusNumber", 1),
                    new XElement("DriverId", 1),
                    new XElement("TicketIncome", 1000),
                    new XElement("Fuel", 200)),
                new XElement("Report",
                    new XElement("RouteNumber", 2),
                    new XElement("BusNumber", 2),
                    new XElement("DriverId", 2),
                    new XElement("TicketIncome", 1200),
                    new XElement("Fuel", 150))
            };
            }
            else
            {
                reports = new List<XElement>
            {
                new XElement("Report",
                    new XElement("RouteNumber", 3),
                    new XElement("BusNumber", 3),
                    new XElement("DriverId", 3),
                    new XElement("TicketIncome", 800),
                    new XElement("Fuel", 100)),
                new XElement("Report",
                    new XElement("RouteNumber", 1),
                    new XElement("BusNumber", 1),
                    new XElement("DriverId", 1),
                    new XElement("TicketIncome", 900),
                    new XElement("Fuel", 180))
            };
            }

            var doc = new XDocument(new XElement("Reports", reports));
            doc.Save(fileName);
            Console.WriteLine($"{fileName} has been generated.");
        }

        public static void GenerateAllXMLFiles()
        {
            GenerateRoutesXML();
            GenerateBusesXML();
            GenerateFuelsXML();
            GenerateDriversXML();
            GenerateReportsXML("reports1.xml", 1);
            GenerateReportsXML("reports2.xml", 2);
        }
    }



    class Program
    {
        static void Main()
        {
            DataGenerator.GenerateAllXMLFiles();

            var routes = DataLoader.LoadRoutes("routes.xml");
            var buses = DataLoader.LoadBuses("buses.xml");
            var fuels = DataLoader.LoadFuels("fuels.xml");
            var drivers = DataLoader.LoadDrivers("drivers.xml");
            var reports = DataLoader.LoadReports("reports1.xml")
                          .Concat(DataLoader.LoadReports("reports2.xml"))
                          .ToList();

            var resultXmlA = ReportProcessor.TaskA(routes, buses, fuels, drivers, reports);
            resultXmlA.Save("taskA_result.xml");

            var resultXmlB = ReportProcessor.TaskB(routes, buses, fuels, drivers, reports);
            resultXmlB.Save("taskB_result.xml");
            
            Console.WriteLine(resultXmlA);
            Console.WriteLine(resultXmlB);



        }
    }
}