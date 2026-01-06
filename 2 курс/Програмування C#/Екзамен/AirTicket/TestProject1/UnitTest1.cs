using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using AIRLINE_TICKETS;

namespace AIRLINE_TICKETS.Tests
{
    public class TaskFixture : IDisposable
    {
        public List<Ticket> Tickets { get; }
        public List<Flight> Flights { get; }
        public List<Destination> Destinations { get; }

        public TaskFixture()
        {
            Flights = new List<Flight>
            {
                new Flight { Id = 1, DestinationId = 1, AircraftBrand = "Boeing" },
                new Flight { Id = 2, DestinationId = 2, AircraftBrand = "Airbus" },
                new Flight { Id = 3, DestinationId = 3, AircraftBrand = "Boeing" }
            };

            Destinations = new List<Destination>
            {
                new Destination { Id = 1, Name = "Paris", FlightCost = 200.0 },
                new Destination { Id = 2, Name = "London", FlightCost = 180.0 },
                new Destination { Id = 3, Name = "Tokyo", FlightCost = 300.0 }
            };

            Tickets = new List<Ticket>
            {
                // From tickets1.xml
                new Ticket { FlightDate = new DateTime(2025, 1, 15), FlightId = 1, PassengerId = 1 }, // Paris, 200.0
                new Ticket { FlightDate = new DateTime(2025, 1, 20), FlightId = 2, PassengerId = 2 }, // London, 180.0
                new Ticket { FlightDate = new DateTime(2025, 2, 10), FlightId = 3, PassengerId = 3 }, // Tokyo, 300.0
                // From tickets2.xml
                new Ticket { FlightDate = new DateTime(2025, 1, 25), FlightId = 2, PassengerId = 1 }, // London, 180.0
                new Ticket { FlightDate = new DateTime(2025, 2, 15), FlightId = 1, PassengerId = 2 }, // Paris, 200.0
                new Ticket { FlightDate = new DateTime(2025, 2, 20), FlightId = 3, PassengerId = 3 }  // Tokyo, 300.0
            };
        }

        public void Dispose() { }
    }

    public class ReportProcessorTests : IClassFixture<TaskFixture>
    {
        private readonly TaskFixture _fix;

        public ReportProcessorTests(TaskFixture fix)
        {
            _fix = fix;
        }

        [Fact]
        public void TaskA_ShouldCalculateTotalRevenuePerFlight()
        {
            // Act
            var result = ReportProcessor.TaskA(_fix.Tickets, _fix.Flights, _fix.Destinations);
            var infos = result.Elements("FlightInfo").ToList();

            // Assert
            Assert.Equal(3, infos.Count); // 3 flights

            // Expected order by destination: London, Paris, Tokyo
            var flightLondon = infos[0]; // FlightId 2, London
            var flightParis = infos[1];  // FlightId 1, Paris
            var flightTokyo = infos[2];  // FlightId 3, Tokyo

            // Flight 2 (London): 180.0 + 180.0 = 360.0
            Assert.Equal("2", flightLondon.Element("FlightId")!.Value);
            Assert.Equal("London", flightLondon.Element("Destination")!.Value);
            Assert.Equal("360", flightLondon.Element("TotalRevenue")!.Value);

            // Flight 1 (Paris): 200.0 + 200.0 = 400.0
            Assert.Equal("1", flightParis.Element("FlightId")!.Value);
            Assert.Equal("Paris", flightParis.Element("Destination")!.Value);
            Assert.Equal("400", flightParis.Element("TotalRevenue")!.Value);

            // Flight 3 (Tokyo): 300.0 + 300.0 = 600.0
            Assert.Equal("3", flightTokyo.Element("FlightId")!.Value);
            Assert.Equal("Tokyo", flightTokyo.Element("Destination")!.Value);
            Assert.Equal("600", flightTokyo.Element("TotalRevenue")!.Value);
        }
    }
}