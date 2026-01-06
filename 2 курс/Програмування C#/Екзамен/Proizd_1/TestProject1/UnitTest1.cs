using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using ELECTRIFICATION_WORKS;

namespace ELECTRIFICATION_WORKS.Tests
{
    // Фікстура для створення тестових даних
    public class TicketProcessFixture : IDisposable
    {
        public List<Route> Routes { get; }
        public List<Passenger> Passengers { get; }
        public List<Category> Categories { get; }
        public List<PaymentRecord> Payments { get; }

        public TicketProcessFixture()
        {
            Routes = new List<Route>
            {
                new Route { Number = 1, StartStop = "First", EndStop = "Last" },
                new Route { Number = 2, StartStop = "Unik", EndStop = "Suxiv" }
            };

            Passengers = new List<Passenger>
            {
                new Passenger { Id = 1, LastName = "Ivanov", CategoryId = 1 },
                new Passenger { Id = 2, LastName = "Petrov", CategoryId = 2 }
            };

            Categories = new List<Category>
            {
                new Category { Id = 1, Name = "Train", Fare = 45 },
                new Category { Id = 2, Name = "Car", Fare = 12 }
            };

            Payments = new List<PaymentRecord>
            {
                new PaymentRecord { Date = new DateTime(2025, 1, 5), PassengerId = 1, RouteNumber = 1 },
                new PaymentRecord { Date = new DateTime(2025, 1, 15), PassengerId = 1, RouteNumber = 1 },
                new PaymentRecord { Date = new DateTime(2025, 2, 1), PassengerId = 2, RouteNumber = 2 }
            };
        }

        public void Dispose() { }
    }

    // Тести з використанням фікстури
    public class TicketProcessTests : IClassFixture<TicketProcessFixture>
    {
        private readonly TicketProcessFixture _fix;

        public TicketProcessTests(TicketProcessFixture fix)
        {
            _fix = fix;
        }

        [Fact]
        public void TaskA_ShouldCountTripsByPassengerAndRoute()
        {
            // Act
            var xml = ReportProcessor.TaskA(_fix.Routes, _fix.Passengers, _fix.Payments);
            var passengerElements = xml.Elements("Passenger").ToList();

            // Assert: Перевіряємо, що є два пасажири
            Assert.Equal(2, passengerElements.Count);

            // Перевіряємо пасажира Ivanov (сортування за прізвищем, тому він перший)
            var ivanov = passengerElements.First(p => p.Element("LastName")?.Value == "Ivanov");
            var ivanovTrips = ivanov.Element("Trips")?.Elements("Route").ToList();
            Assert.NotNull(ivanovTrips);
            Assert.Single(ivanovTrips); // Ivanov має поїздки тільки на маршруті 1
            Assert.Equal("1", ivanovTrips[0].Element("RouteNumber")?.Value);
            Assert.Equal("2", ivanovTrips[0].Element("TripCount")?.Value); // 2 поїздки

            // Перевіряємо пасажира Petrov
            var petrov = passengerElements.First(p => p.Element("LastName")?.Value == "Petrov");
            var petrovTrips = petrov.Element("Trips")?.Elements("Route").ToList();
            Assert.NotNull(petrovTrips);
            Assert.Single(petrovTrips); // Petrov має поїздки тільки на маршруті 2
            Assert.Equal("2", petrovTrips[0].Element("RouteNumber")?.Value);
            Assert.Equal("1", petrovTrips[0].Element("TripCount")?.Value); // 1 поїздка
        }

    }
}