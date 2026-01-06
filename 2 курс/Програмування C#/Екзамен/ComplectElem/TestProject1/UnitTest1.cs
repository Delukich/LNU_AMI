using ProductionSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace ProductionSystem.Tests
{
    public class TaskFixture : IDisposable
    {
        public List<Component> Components { get; }
        public List<Category> Categories { get; }
        public List<Product> Products { get; }
        public List<Operation> Operations { get; }

        public TaskFixture()
        {
            Components = new List<Component>
            {
                new Component { Id = 1, Name = "Процесор", Price = 150.50m },
                new Component { Id = 2, Name = "Пам'ять", Price = 75.25m },
                new Component { Id = 3, Name = "Материнська плата", Price = 200.00m }
            };

            Categories = new List<Category>
            {
                new Category { Id = 1, Name = "Ноутбуки" },
                new Category { Id = 2, Name = "Стаціонарні ПК" }
            };

            Products = new List<Product>
            {
                new Product { FactoryNumber = 1001, CategoryId = 1 },
                new Product { FactoryNumber = 1002, CategoryId = 1 },
                new Product { FactoryNumber = 1003, CategoryId = 2 }
            };

            Operations = new List<Operation>
            {
                new Operation { ProductFactoryNumber = 1001, Date = new DateTime(2025, 1, 10), ComponentId = 1, IsCompleted = "так" },
                new Operation { ProductFactoryNumber = 1001, Date = new DateTime(2025, 1, 12), ComponentId = 2, IsCompleted = "так" },
                new Operation { ProductFactoryNumber = 1002, Date = new DateTime(2025, 1, 15), ComponentId = 3, IsCompleted = "ні" },
                new Operation { ProductFactoryNumber = 1002, Date = new DateTime(2025, 1, 16), ComponentId = 1, IsCompleted = "так" },
                new Operation { ProductFactoryNumber = 1003, Date = new DateTime(2025, 1, 20), ComponentId = 2, IsCompleted = "так" }
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
        public void TaskA_ShouldCalculateTotalComponentPricePerProduct()
        {
            // Act
            var result = Tasks.TaskA(_fix.Products, _fix.Categories, _fix.Components, _fix.Operations);
            var infos = result.Elements("Info").ToList();

            // Assert
            Assert.Equal(3, infos.Count); // 3 продукти

            var product1001 = infos.First(x => x.Element("FactoryNumber")!.Value == "1001");
            var product1002 = infos.First(x => x.Element("FactoryNumber")!.Value == "1002");
            var product1003 = infos.First(x => x.Element("FactoryNumber")!.Value == "1003");

            // Product 1001: Процесор (150.50) + Пам'ять (75.25) = 225.75
            Assert.Equal("Ноутбуки", product1001.Element("CategoryName")!.Value);
            Assert.Equal("225.75", product1001.Element("TotalPrice")!.Value);

            // Product 1002: Процесор (150.50, "так") = 150.50 (Материнська плата не враховується, бо "ні")
            Assert.Equal("Ноутбуки", product1002.Element("CategoryName")!.Value);
            Assert.Equal("150.50", product1002.Element("TotalPrice")!.Value);

            // Product 1003: Пам'ять (75.25) = 75.25
            Assert.Equal("Стаціонарні ПК", product1003.Element("CategoryName")!.Value);
            Assert.Equal("75.25", product1003.Element("TotalPrice")!.Value);
        }
    }
}