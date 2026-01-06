//Варіант 1: Реалізація системи управління електронними пристроямиОпис:
//Розробити систему для управління електронними пристроями, використовуючи інтерфейси та класи. У програмі має бути загальний інтерфейс для електронних пристроїв та кілька класів, що його реалізують.Основні вимоги:
//Створи інтерфейс IElectronicDevice, який містить:
//Властивості:
//string Brand – бренд пристрою.
//string Model – модель пристрою.
//bool IsOn – стан пристрою (увімкнено/вимкнено).
//Методи:
//void TurnOn() – увімкнення пристрою.
//void TurnOff() – вимкнення пристрою.
//void GetInfo() – вивід інформації про пристрій.
//Створи класи пристроїв, які реалізують IElectronicDevice:
//Smartphone (Смартфон)
//Laptop (Ноутбук)
//SmartTV (Розумний телевізор)
//Додай у класи унікальні особливості:
//Smartphone має операційну систему (string OS).
//Laptop має обсяг оперативної пам’яті (int RAM).
//SmartTV має діагональ екрану (double ScreenSize).
//У методі Main() створи список електронних пристроїв та виклич методи TurnOn(), TurnOff() і GetInfo() для кожного з них.

using System;
using System.Collections.Generic;

interface IElectronicDevice
{
    string Brand { get; set; }
    string Model { get; set; }
    bool IsOn { get; set; }
    void TurnOn();
    void TurnOff();
    void GetInfo();
}
class Smartphone : IElectronicDevice
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public bool IsOn { get; set; }
    public string OS { get; set; }
    public void TurnOn()
    {
        IsOn = true;
    }
    public void TurnOff()
    {
        IsOn = false;
    }
    public void GetInfo()
    {
        Console.WriteLine($"Smartphone: {Brand} {Model}, OS: {OS}, IsOn: {IsOn}");
    }
}

class Laptop : IElectronicDevice
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public bool IsOn { get; set; }
    public int RAM { get; set; }
    public void TurnOn()
    {
        IsOn = true;
    }
    public void TurnOff()
    {
        IsOn = false;
    }
    public void GetInfo()
    {
        Console.WriteLine($"Laptop: {Brand} {Model}, RAM: {RAM} GB, IsOn: {IsOn}");
    }
}
class SmartTV : IElectronicDevice
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public bool IsOn { get; set; }
    public double ScreenSize { get; set; }
    public void TurnOn()
    {
        IsOn = true;
    }
    public void TurnOff()
    {
        IsOn = false;
    }
    public void GetInfo()
    {
        Console.WriteLine($"SmartTV: {Brand} {Model}, ScreenSize: {ScreenSize} inch, IsOn: {IsOn}");
    }
}

class Program
{
    static void Main()
    {
        List<IElectronicDevice> devices = new List<IElectronicDevice>
        {
            new Smartphone { Brand = "Apple", Model = "iPhone 16", OS = "iOS", IsOn = true },
            new Smartphone { Brand = "Samsung", Model = "Galaxy S23", OS = "Android", IsOn = false },
            new Laptop { Brand = "Lenovo", Model = "iPad Slim 5", RAM = 16, IsOn = false },
            new Laptop { Brand = "MacBook", Model = "Air 2", RAM = 32, IsOn = true },
            new SmartTV { Brand = "Asus", Model = "Pro 203", ScreenSize = 68.4, IsOn = true },
            new SmartTV { Brand = "Samsung", Model = "Galaxy A51", ScreenSize = 55, IsOn = false }
        };

        foreach (var device in devices)
        {
            device.TurnOn();
            device.GetInfo();
            device.TurnOff();
            device.GetInfo();
        }
    }
}

