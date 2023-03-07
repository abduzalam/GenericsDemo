// See https://aka.ms/new-console-template for more information
using ConsoleUI.Models;
using ConsoleUI.WithGenerics;
using ConsoleUI.WithoutGenerics;
using System.Runtime.CompilerServices;

public class Program
{
    private static void Main(string[] args)
    {
        //Console.WriteLine($"DateTime.Now = {DateTime.Now} , DateTime.UtcNow = {DateTime.UtcNow}");//both print same result
        Console.ReadLine();
        DemonstrateTextFileStorage();
        Console.WriteLine();
        Console.WriteLine("Press enter to shut down..");
        Console.ReadLine();
    }

    private static void DemonstrateTextFileStorage()
    {
        List<Person> people = new List<Person>();
        List<LogEntry> logs = new List<LogEntry>();

        string peopleFile = @"C:\Temp\people.csv";
        string logFile = @"C:\Temp\logs.csv";

        PopulateLists(people, logs);

        /* New way of doing things with-generics */

        GenericTextFileProcessor.SaveToTextFile<Person>(people, peopleFile);
        GenericTextFileProcessor.SaveToTextFile<LogEntry>(logs, logFile);

        var newPeople = GenericTextFileProcessor.LoadFromTextFile<Person>(peopleFile);

        foreach (var p in newPeople)
        {
            Console.WriteLine($"{p.FirstName} {p.LastName} (IsAlive = {p.IsAlive})");
        }

        var newLogs = GenericTextFileProcessor.LoadFromTextFile<LogEntry>(logFile);

        foreach (var log in newLogs)
        {
            Console.WriteLine($"{log.ErrorCode}: {log.Message} at {log.TimeOfEvent}");
        }



        /* OLD way of doing things non-generics */
        //OriginalTextFileProcessor.SaveLogs(logs, logFile);

        //var newLogs = OriginalTextFileProcessor.LoadLogs(logFile);

        //foreach (var log in newLogs)
        //{
        //    Console.WriteLine($"{log.ErrorCode}: {log.Message} at {log.TimeOfEvent.ToShortTimeString}");
        //}

        //OriginalTextFileProcessor.SavePeople(people, peopleFile);

        //var newPeople = OriginalTextFileProcessor.LoadPeople(peopleFile);

        //foreach (var p in newPeople)
        //{
        //    Console.WriteLine($"{p.FirstName} {p.LastName} (IsAlive = {p.IsAlive})");
        //}
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