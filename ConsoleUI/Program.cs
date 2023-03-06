// See https://aka.ms/new-console-template for more information
using ConsoleUI.Models;
using System.Runtime.CompilerServices;

public class Program
{
    private static void Main(string[] args)
    {
        //Console.WriteLine($"DateTime.Now = {DateTime.Now} , DateTime.UtcNow = {DateTime.UtcNow}");//both print same result
        
        Console.WriteLine();
        Console.WriteLine("Press enter to shut down..");
        Console.ReadLine();
    }

    private static void DemonstrateTextFileStorage()
    {

    }

    private static void PopulateLists(List<Person> people, List<LogEntry> logs)
    {
        people.Add(new Person() { FirstName = "AbdulSalam", LastName = "Abd" });
        people.Add(new Person() { FirstName = "Steve", LastName = "Jobs", IsAlive = false });
        people.Add(new Person() { FirstName = "Bill", LastName = "Gates" });

        logs.Add(new LogEntry() { Message="I blew up", ErrorCode= 9999 });
        logs.Add(new LogEntry() { Message = "I'm too awesome", ErrorCode = 1337 });
        logs.Add(new LogEntry() { Message = "I was tired", ErrorCode = 2222 });
    }
  
}