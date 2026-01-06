using System;

public delegate void LowPerformanceEventHandler(object sender, EventArgs e);

class Computer
{
    private string processorType;
    private double performance; 
    private int ramSize; 
    
    public event LowPerformanceEventHandler LowPerformanceDetected;
    
    public Computer(string processorType, double performance, int ramSize)
    {
        this.processorType = processorType;
        this.performance = performance;
        this.ramSize = ramSize;
        
    }
    
    public string ProcessorType
    {
        get { return processorType; }
        set { processorType = value; }
    }

    public double Performance
    {
        get { return performance; }
        set 
        { 
            performance = value;
            CheckPerformance(); 
        }
    }

    public int RamSize
    {
        get { return ramSize; }
        set { ramSize = value; }
    }
    
    public void CheckPerformance() 
    {
        if (performance < 2.0)
        {
            OnLowPerformanceDetected();
        }
    }
    
    protected virtual void OnLowPerformanceDetected()
    {
        LowPerformanceDetected?.Invoke(this, EventArgs.Empty);
    }
    
    public override string ToString()
    {
        return $"Комп'ютер: Тип процесора - {processorType}, " +
               $"Швидкодія - {performance} ГГц, " +
               $"Оперативна пам'ять - {ramSize} ГБ";
    }
}

class Program
{
    static void Main()
    {
        Computer myComputer = new Computer("Intel Core i5", 1.8, 8);
        Computer myComputer2 = new Computer("AMD Ryzen 5", 3.0, 16);
        Computer myComputer3 = new Computer("AMD Ryzen 9", 0.8, 24);
        
        myComputer.LowPerformanceDetected += Computer_LowPerformanceDetected;
        myComputer2.LowPerformanceDetected += Computer_LowPerformanceDetected;
        myComputer3.LowPerformanceDetected += Computer_LowPerformanceDetected;
        
        Console.WriteLine(myComputer);
        myComputer.CheckPerformance();

        Console.WriteLine(myComputer2);
        myComputer2.CheckPerformance();

        Console.WriteLine(myComputer3);
        myComputer3.CheckPerformance();
    }
    
    private static void Computer_LowPerformanceDetected(object sender, EventArgs e)
    {
        Console.WriteLine("Увага: Виявлено незадовільну швидкодію комп'ютера!");
    }
}