using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using TransportationAccounting;

namespace TransportationAccounting.Tests
{
    public class TransportFixture : IDisposable
    {
        public List<Route> Routes { get; }
        public List<Driver> Drivers { get; }
        public List<Bus> Buses { get; }
        public List<Fuel> Fuels { get; }
        public List<TripReport> Reports { get; }

        public TransportFixture()
        {
            Routes = new List<Route>
            {
                new Route { Number = 1, Destination = "Kyiv" },
                new Route { Number = 2, Destination = "Lviv" }
            };

            Drivers = new List<Driver>
            {
                new Driver { Id = 1, LastName = "Shevchenko" },
                new Driver { Id = 2, LastName = "Bandera" }
            };

            Fuels = new List<Fuel>
            {
                new Fuel { Type = "Diesel", PricePerLiter = 30 },
                new Fuel { Type = "Gasoline", PricePerLiter = 35 }
            };

            Buses = new List<Bus>
            {
                new Bus { Number = "A1", FuelType = "Diesel" },
                new Bus { Number = "B2", FuelType = "Gasoline" }
            };

            Reports = new List<TripReport>
            {
                new TripReport { RouteNumber = 1, BusNumber = "A1", DriverId = 1, TicketRevenue = 1000, FuelUsedLiters = 20 },
                new TripReport { RouteNumber = 1, BusNumber = "B2", DriverId = 1, TicketRevenue = 800, FuelUsedLiters = 10 },
                new TripReport { RouteNumber = 2, BusNumber = "A1", DriverId = 2, TicketRevenue = 900, FuelUsedLiters = 25 },
                new TripReport { RouteNumber = 2, BusNumber = "B2", DriverId = 2, TicketRevenue = 700, FuelUsedLiters = 15 }
            };
        }

        public void Dispose() { }
    }

    public class TransportTests : IClassFixture<TransportFixture>
    {
        private readonly TransportFixture _f;

        public TransportTests(TransportFixture fixture)
        {
            _f = fixture;
        }

        [Fact]
        public void TaskA_ShouldReturnProfitPerRoutePerDriver()
        {
            var xml = ReportProcessor.TaskA(
                _f.Routes, _f.Buses, _f.Fuels, _f.Drivers, _f.Reports);

            var routeElements = xml.Elements("Route").ToList();
            Assert.Equal(2, routeElements.Count);

            var kyiv = routeElements.FirstOrDefault(e => e.Attribute("Destination")?.Value == "Kyiv");
            Assert.NotNull(kyiv);

            var shevchenko = kyiv.Elements("Driver")
                .FirstOrDefault(d => d.Attribute("LastName")?.Value == "Shevchenko");
            Assert.NotNull(shevchenko);
            Assert.Equal("1650", shevchenko.Attribute("Profit")?.Value); // 1000 - 600 + 800 - 350 = 1650
        }

        [Fact]
        public void TaskB_ShouldReturnTopProfitRouteForEachDriver()
        {
            var xml = ReportProcessor.TaskB(
                _f.Routes, _f.Buses, _f.Fuels, _f.Drivers, _f.Reports);

            var driverElements = xml.Elements("Driver").ToList();
            Assert.Equal(2, driverElements.Count);

            var shevchenko = driverElements.FirstOrDefault(e => e.Attribute("LastName")?.Value == "Shevchenko");
            Assert.NotNull(shevchenko);
            Assert.Equal("Kyiv", shevchenko.Element("TopRoute")?.Attribute("Destination")?.Value);

            var bandera = driverElements.FirstOrDefault(e => e.Attribute("LastName")?.Value == "Bandera");
            Assert.NotNull(bandera);
            Assert.Equal("Lviv", bandera.Element("TopRoute")?.Attribute("Destination")?.Value);
        }
    }
}
