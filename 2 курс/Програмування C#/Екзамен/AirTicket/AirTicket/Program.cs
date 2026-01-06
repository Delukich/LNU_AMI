using System;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;


/*застосування xml, linq i xunit 
Розробити вказані типи і статичні методи, які реалізують завдання з використанням Инд і повертають результат у вигляді об'екта XElement: 
Перевірити функціональність статичних методів за допомогою xunit, використовуючи Test Class Extures 
Викопати поставлені завдании на даних, які задані кількома xml-файлами, результати завдань, які поверта ють статичні методи, перевірити xunit-тестами. 
1. Розробити типи для обліку продаж авіаквитків. 
Інформація про квиток містить дату польоту, числовий ідентифікатор рейсу та ідентифікатор пасажира. 
Рейс характеризується числовим ідентифікатором, ідентифікатором пункту призначення і маркою літака. 
Пасажир характеризується числовим ідентифікатором, прізвищем та віком. 
Пункт призначення характеризується числовим ідентифікатором, назвою та вартістю одного польоту. 
Інформацію про продані квитки подано кількома (не менше 2) окремими хті-файлами. 
2. Отримати: 
(а)об'єкт типу XElement де для кожного рейсу вказано сумарну вартість проданих квитків, цей результат також вивести у хті-файл; перелік впорядкувати за назвою пункту призначення у лексико-графічному порядку: 
(6) об'єкт типу XElement де для кожного пасажира вказано рейси, на які витрачено найбільшу щомісячну суму, яку також включити у результат, отриманий результат також вивести у хті-файл. Переліки впорал кувати за прізвищем пасажира і назвою пукту призначення у лексико-графічному порядку (без повторень).
*/
namespace AIRLINE_TICKETS
{
    public class Ticket
    {
        public DateTime FlightDate { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
    }

    public class Flight
    {
        public int Id { get; set; }
        public int DestinationId { get; set; }
        public string AircraftBrand { get; set; }
    }

    public class Passenger
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
    }

    public class Destination
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double FlightCost { get; set; }
    }

    public static class DataLoader
    {
        public static List<Ticket> LoadTickets(string path) =>
            XDocument.Load(path)
                     .Descendants("Ticket")
                     .Select(t => new Ticket
                     {
                         FlightDate = (DateTime)t.Element("FlightDate"),
                         FlightId = (int)t.Element("FlightId"),
                         PassengerId = (int)t.Element("PassengerId")
                     })
                     .ToList();

        public static List<Flight> LoadFlights(string path) =>
            XDocument.Load(path)
                     .Descendants("Flight")
                     .Select(f => new Flight
                     {
                         Id = (int)f.Element("Id"),
                         DestinationId = (int)f.Element("DestinationId"),
                         AircraftBrand = (string)f.Element("AircraftBrand")
                     })
                     .ToList();

        public static List<Passenger> LoadPassengers(string path) =>
            XDocument.Load(path)
                     .Descendants("Passenger")
                     .Select(p => new Passenger
                     {
                         Id = (int)p.Element("Id"),
                         Surname = (string)p.Element("Surname"),
                         Age = (int)p.Element("Age")
                     })
                     .ToList();

        public static List<Destination> LoadDestinations(string path) =>
            XDocument.Load(path)
                     .Descendants("Destination")
                     .Select(d => new Destination
                     {
                         Id = (int)d.Element("Id"),
                         Name = (string)d.Element("Name"),
                         FlightCost = (double)d.Element("FlightCost")
                     })
                     .ToList();
    }

    public static class ReportProcessor
    {
        public static XElement TaskA(
            List<Ticket> tickets,
            List<Flight> flights,
            List<Destination> destinations)
        {
            var result = from t in tickets
                         join f in flights on t.FlightId equals f.Id
                         join d in destinations on f.DestinationId equals d.Id
                         group d.FlightCost by new { f.Id, d.Name } into g
                         orderby g.Key.Name
                         select new XElement("FlightInfo",
                             new XElement("FlightId", g.Key.Id),
                             new XElement("Destination", g.Key.Name),
                             new XElement("TotalRevenue", g.Sum())
                         );

            return new XElement("FlightRevenues", result);
        }

        public static XElement TaskB(
            List<Ticket> tickets,
            List<Flight> flights,
            List<Destination> destinations,
            List<Passenger> passengers)
        {
            var fullData = from t in tickets
                           join f in flights on t.FlightId equals f.Id
                           join d in destinations on f.DestinationId equals d.Id
                           join p in passengers on t.PassengerId equals p.Id
                           group new { t.FlightDate, d.FlightCost, f.Id, d.Name }
                           by new { p.Id, p.Surname, Year = t.FlightDate.Year, Month = t.FlightDate.Month } into g
                           select new
                           {
                               PassengerId = g.Key.Id,
                               Surname = g.Key.Surname,
                               Year = g.Key.Year,
                               Month = g.Key.Month,
                               TotalCost = g.Sum(x => x.FlightCost),
                               Flights = g.Select(x => new { x.Id, x.Name, x.FlightCost }).ToList()
                           };

            var grouped = from entry in fullData
                          group entry by new { entry.PassengerId, entry.Surname } into pg
                          let maxMonthlyCost = pg.Max(x => x.TotalCost)
                          let bestMonths = pg
                              .Where(x => x.TotalCost == maxMonthlyCost)
                              .SelectMany(x => x.Flights)
                              .Distinct()
                              .OrderBy(x => x.Name)
                              .Select(x => new XElement("Flight",
                                  new XElement("FlightId", x.Id),
                                  new XElement("Destination", x.Name),
                                  new XElement("Cost", x.FlightCost)
                              ))
                          orderby pg.Key.Surname
                          select new XElement("Passenger",
                              new XElement("PassengerId", pg.Key.PassengerId),
                              new XElement("Surname", pg.Key.Surname),
                              new XElement("MaxMonthlyCost", maxMonthlyCost),
                              new XElement("BestFlights", bestMonths)
                          );

            return new XElement("BestFlightsByPassenger", grouped);
        }
    }

    public class DataGenerator
    {
        public static void GenerateTicketsXML(string fileName, int ticketSet)
        {
            List<XElement> tickets;

            if (ticketSet == 1)
            {
                tickets = new List<XElement>
                {
                    new XElement("Ticket",
                        new XElement("FlightDate", new DateTime(2025, 1, 15)),
                        new XElement("FlightId", 1),
                        new XElement("PassengerId", 1)),
                    new XElement("Ticket",
                        new XElement("FlightDate", new DateTime(2025, 1, 20)),
                        new XElement("FlightId", 2),
                        new XElement("PassengerId", 2)),
                    new XElement("Ticket",
                        new XElement("FlightDate", new DateTime(2025, 2, 10)),
                        new XElement("FlightId", 3),
                        new XElement("PassengerId", 3))
                };
            }
            else // ticketSet == 2
            {
                tickets = new List<XElement>
                {
                    new XElement("Ticket",
                        new XElement("FlightDate", new DateTime(2025, 1, 25)),
                        new XElement("FlightId", 2),
                        new XElement("PassengerId", 1)),
                    new XElement("Ticket",
                        new XElement("FlightDate", new DateTime(2025, 2, 15)),
                        new XElement("FlightId", 1),
                        new XElement("PassengerId", 2)),
                    new XElement("Ticket",
                        new XElement("FlightDate", new DateTime(2025, 2, 20)),
                        new XElement("FlightId", 3),
                        new XElement("PassengerId", 3))
                };
            }

            var doc = new XDocument(new XElement("Tickets", tickets));
            doc.Save(fileName);
            Console.WriteLine($"{fileName} has been generated.");
        }

        public static void GenerateFlightsXML()
        {
            var flights = new List<XElement>
            {
                new XElement("Flight",
                    new XElement("Id", 1),
                    new XElement("DestinationId", 1),
                    new XElement("AircraftBrand", "Boeing")),
                new XElement("Flight",
                    new XElement("Id", 2),
                    new XElement("DestinationId", 2),
                    new XElement("AircraftBrand", "Airbus")),
                new XElement("Flight",
                    new XElement("Id", 3),
                    new XElement("DestinationId", 3),
                    new XElement("AircraftBrand", "Boeing"))
            };

            var doc = new XDocument(new XElement("Flights", flights));
            doc.Save("flights.xml");
            Console.WriteLine("flights.xml has been generated.");
        }

        public static void GeneratePassengersXML()
        {
            var passengers = new List<XElement>
            {
                new XElement("Passenger",
                    new XElement("Id", 1),
                    new XElement("Surname", "Ivanov"),
                    new XElement("Age", 30)),
                new XElement("Passenger",
                    new XElement("Id", 2),
                    new XElement("Surname", "Petrov"),
                    new XElement("Age", 25)),
                new XElement("Passenger",
                    new XElement("Id", 3),
                    new XElement("Surname", "Sidorov"),
                    new XElement("Age", 40))
            };

            var doc = new XDocument(new XElement("Passengers", passengers));
            doc.Save("passengers.xml");
            Console.WriteLine("passengers.xml has been generated.");
        }

        public static void GenerateDestinationsXML()
        {
            var destinations = new List<XElement>
            {
                new XElement("Destination",
                    new XElement("Id", 1),
                    new XElement("Name", "Paris"),
                    new XElement("FlightCost", 200.0)),
                new XElement("Destination",
                    new XElement("Id", 2),
                    new XElement("Name", "London"),
                    new XElement("FlightCost", 180.0)),
                new XElement("Destination",
                    new XElement("Id", 3),
                    new XElement("Name", "Tokyo"),
                    new XElement("FlightCost", 300.0))
            };

            var doc = new XDocument(new XElement("Destinations", destinations));
            doc.Save("destinations.xml");
            Console.WriteLine("destinations.xml has been generated.");
        }

        public static void GenerateAllXMLFiles()
        {
            GenerateFlightsXML();
            GeneratePassengersXML();
            GenerateDestinationsXML();
            GenerateTicketsXML("tickets1.xml", 1);
            GenerateTicketsXML("tickets2.xml", 2);
        }
    }

    class Program
    {
        static void Main()
        {
            DataGenerator.GenerateAllXMLFiles();

            var tickets = DataLoader.LoadTickets("tickets1.xml")
                          .Concat(DataLoader.LoadTickets("tickets2.xml"))
                          .ToList();
            var flights = DataLoader.LoadFlights("flights.xml");
            var passengers = DataLoader.LoadPassengers("passengers.xml");
            var destinations = DataLoader.LoadDestinations("destinations.xml");

            var resultA = ReportProcessor.TaskA(tickets, flights, destinations);
            resultA.Save("taskA_result.xml");

            var resultB = ReportProcessor.TaskB(tickets, flights, destinations, passengers);
            resultB.Save("taskB_result.xml");

            Console.WriteLine("Reports taskA_result.xml and taskB_result.xml have been saved.");
        }
    }
}