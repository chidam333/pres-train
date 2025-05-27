using System;

public class Greeter
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Please enter your name:");
        string name = Console.ReadLine();
        Console.WriteLine($"Hello, {name}! Welcome.");
    }
}