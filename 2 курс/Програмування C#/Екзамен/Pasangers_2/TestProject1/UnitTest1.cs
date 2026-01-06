using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using TransportReports;

namespace TransportReports.Tests
{
    public class TaskFixture : IDisposable
    {
        public List<Driver> Drivers { get; }
        public List<Report> Reports { get; }
        public List<Marshrut> Marshruts { get; }
        public List<Bus> Buses { get; }
        public List<Fuel> Fuels { get; }

        public TaskFixture()
        {
            Drivers = new List<Driver>
            {
                new Driver { Id = 1, Surname = "Коваль" },
                new Driver { Id = 2, Surname = "Шевченко" },
                new Driver { Id = 3, Surname = "Петренко" }
            };

            Marshruts = new List<Marshrut>
            {
                new Marshrut { Number = 1, Destination = "Київ" },
                new Marshrut { Number = 2, Destination = "Львів" },
                new Marshrut { Number = 3, Destination = "Одеса" }
            };

            Buses = new List<Bus>
            {
                new Bus { Number = 101, FuelType = "Дизель" },
                new Bus { Number = 102, FuelType = "Бензин" }
            };

            Fuels = new List<Fuel>
            {
                new Fuel { Type = "Дизель", PricePerLiter = 40.0m },
                new Fuel { Type = "Бензин", PricePerLiter = 45.0m }
            };

            Reports = new List<Report>
            {
                new Report { MarshrutNumber = 1, BusNumber = 101, DriverId = 1, TicketRevenue = 5000.0m, FuelConsumption = 50.0m },
                new Report { MarshrutNumber = 2, BusNumber = 102, DriverId = 2, TicketRevenue = 4000.0m, FuelConsumption = 40.0m },
                new Report { MarshrutNumber = 3, BusNumber = 101, DriverId = 3, TicketRevenue = 6000.0m, FuelConsumption = 60.0m },
                new Report { MarshrutNumber = 1, BusNumber = 102, DriverId = 2, TicketRevenue = 4500.0m, FuelConsumption = 45.0m },
                new Report { MarshrutNumber = 2, BusNumber = 101, DriverId = 1, TicketRevenue = 4200.0m, FuelConsumption = 42.0m },
                new Report { MarshrutNumber = 3, BusNumber = 102, DriverId = 3, TicketRevenue = 5500.0m, FuelConsumption = 55.0m }
            };
        }

        public void Dispose() { }
    }

    public class TasksTests : IClassFixture<TaskFixture>
    {
        private readonly TaskFixture _fixture;

        public TasksTests(TaskFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TaskA_ShouldGroupByDriverAndDestination()
        {
            // Act
            var result = Tasks.TaskA(_fixture.Drivers, _fixture.Reports, _fixture.Marshruts, _fixture.Buses, _fixture.Fuels);
            var infos = result.Elements("Info").ToList();

            // Assert
            Assert.Equal(5, infos.Count);
            Assert.Single(infos.Where(x => x.Element("Surname")?.Value == "Коваль" && x.Element("Destination")?.Value == "Київ"));
            Assert.Single(infos.Where(x => x.Element("Surname")?.Value == "Коваль" && x.Element("Destination")?.Value == "Львів"));
            Assert.Single(infos.Where(x => x.Element("Surname")?.Value == "Петренко" && x.Element("Destination")?.Value == "Одеса"));
        }

        [Fact]
        public void TaskA_ShouldOrderBySurnameAndDestination()
        {
            // Act
            var result = Tasks.TaskA(_fixture.Drivers, _fixture.Reports, _fixture.Marshruts, _fixture.Buses, _fixture.Fuels);
            var infos = result.Elements("Info").ToList();

            // Assert
            Assert.Equal("Коваль", infos[0].Element("Surname")?.Value);
            Assert.Equal("Київ", infos[0].Element("Destination")?.Value);

            Assert.Equal("Коваль", infos[1].Element("Surname")?.Value);
            Assert.Equal("Львів", infos[1].Element("Destination")?.Value);

            Assert.Equal("Петренко", infos[2].Element("Surname")?.Value);
            Assert.Equal("Одеса", infos[2].Element("Destination")?.Value);
        }

        
    }
}