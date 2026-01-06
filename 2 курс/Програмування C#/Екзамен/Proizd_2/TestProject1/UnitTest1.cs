using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Xml.Linq;
using TransportSystem;
using Xunit;

namespace TransportSystem.Tests
{
    public class TaskFixture : IDisposable
    {
        public List<Marshrut> Marshrut { get; }
        public List<Account> Accounts { get; }

        public TaskFixture()
        {
            Marshrut = new List<Marshrut>
            {
                new Marshrut { Number = 101, StartStop = "Центр", EndStop = "Вокзал" },
                new Marshrut { Number = 102, StartStop = "Аеропорт", EndStop = "Парк" },
                new Marshrut { Number = 103, StartStop = "Ботсад", EndStop = "Музей" }
            };

            Accounts = new List<Account>
            {
                // From accounts1.xml
                new Account { Date = new DateTime(2025, 1, 10), PassengerId = 1, MarshrutNumber = 101 },
                new Account { Date = new DateTime(2025, 1, 15), PassengerId = 2, MarshrutNumber = 102 },
                new Account { Date = new DateTime(2025, 1, 20), PassengerId = 3, MarshrutNumber = 103 },
                // From accounts2.xml
                new Account { Date = new DateTime(2025, 2, 5), PassengerId = 1, MarshrutNumber = 102 },
                new Account { Date = new DateTime(2025, 2, 10), PassengerId = 2, MarshrutNumber = 101 },
                new Account { Date = new DateTime(2025, 2, 15), PassengerId = 3, MarshrutNumber = 103 }
            };
        }

        public void Dispose() { }
    }

    public class TasksTests : IClassFixture<TaskFixture>
    {
        private readonly TaskFixture _fix;

        public TasksTests(TaskFixture fix)
        {
            _fix = fix;
        }

        [Fact]
        public void TaskA_ShouldCalculatePassengerCountPerMarshrut()
        {
            // Act
            var result = Tasks.TaskA(_fix.Marshrut, _fix.Accounts);
            var infos = result.Elements("Info").ToList();

            // Assert
            Assert.Equal(3, infos.Count); // 3 маршрути

            // Expected order by StartStop: Аеропорт, Ботсад, Центр
            var marshrut102 = infos[0]; // Аеропорт (102)
            var marshrut103 = infos[1]; // Ботсад (103)
            var marshrut101 = infos[2]; // Центр (101)

            // Marshrut 102: 2 пасажири (1 in Jan, 1 in Feb)
            Assert.Equal("102", marshrut102.Element("MarshrutNumber")!.Value);
            Assert.Equal("Аеропорт", marshrut102.Element("StartStop")!.Value);
            Assert.Equal("2", marshrut102.Element("PassengerCount")!.Value);

            // Marshrut 103: 2 пасажири (1 in Jan, 1 in Feb)
            Assert.Equal("103", marshrut103.Element("MarshrutNumber")!.Value);
            Assert.Equal("Ботсад", marshrut103.Element("StartStop")!.Value);
            Assert.Equal("2", marshrut103.Element("PassengerCount")!.Value);

            // Marshrut 101: 2 пасажири (1 in Jan, 1 in Feb)
            Assert.Equal("101", marshrut101.Element("MarshrutNumber")!.Value);
            Assert.Equal("Центр", marshrut101.Element("StartStop")!.Value);
            Assert.Equal("2", marshrut101.Element("PassengerCount")!.Value);
        }
    }
}