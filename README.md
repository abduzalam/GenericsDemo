# GenericsDemo

Type List , you can see the List with Angle bracket, so this list use Generics, and it has that T that T refers to the Type of elements in the list, 
![image](https://user-images.githubusercontent.com/32676744/223138482-64821c6c-c5b6-4d3e-a665-09b7652070ed.png)

so I could put int 

![image](https://user-images.githubusercontent.com/32676744/223139030-111e0023-a3b1-4b0f-8760-4d7bb352784a.png)


so above <int> is the T ( type ) here . the T allows us to specify which type this list is about. 

in earlier version of C#, ArrayList was there, which is deprecated , it was part of system.collections. do not use ArrayList 
**Generics is so much better**. Arraylist accepts objects, so we can add int, string etc to an arraylist . it requires casting to access the elements from arraylist. also this leads to run time errors if the casting is not properly done, lot of mess, lot of resource it takes for conversion from object type etc , so very inefficient, slow , not very safe. 

### Generic gives compile eror if we add a string to an int list, which is good. We like compile time error . means strongly typed.
  
  
The below code explain the new way of load and save file using generic.
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.WithGenerics
{
    public static class GenericTextFileProcessor
    {
        /// <summary>
        /// where T : class , new () : This is generally called Limiter
        /// What it means is this method LoadFromTextFile can accept any Type T , where T should be a class and should have an empty constructor to it( I mean in the T class )
        /// List<T> output = new List<T> (); This is the output it can be any class say Person or LogEntry or any other class
        ///  T entry = new T (); This is a new instance of type T ( Person p = new Person() ) , here we have T() : we initialize using an empty constructor, that when the new () in the limiter get visibility
        ///  If we didn't have the new () in the method signature, then T entry = new T () will throw compile time error
        ///  entry.GetType().GetProperties() : this is reflection. This allows us to look in object at runtime and get the properties; So here we get the actual type, like a Person or LogEntry and get all properties
        ///  Say in case of Person class, we have FirstName, LastName and IsAlive and for LogEntry we have ErrorCode, Message and TimeOfEvent property
        ///  Note that, reflection is expensive and it will reduce the program speed/performance, so use it only when necessary
        ///  Reading/Wrting to a text file, saving things out of memory to another disc, loading thing from external memory are some of the great use of reflection because we are not using it all the time 
        ///  like saving, application start up etc usually not carried at very often
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public static List<T> LoadFromTextFile<T>(string filePath) where T : class , new ()
        {

            var lines = File.ReadAllLines(filePath).ToList();
            List<T> output = new List<T> ();
            T entry = new T ();

            var cols = entry.GetType().GetProperties();

            // Checks to be sure we have at least one header row and one datarow
            if (lines.Count < 2)
            {
                throw new IndexOutOfRangeException("The file was either empty or missing.");
            }
            //Splits the header into one of the header per entry
            var headers = lines[0].Split(',');
            
            // Removes the header row from the lines so we don't
            // have to worry about skipping over that first row
            lines.RemoveAt(0);


            foreach(var row in lines)
            {
                entry = new T ();
                // Splits the row into individual columns. Now the index 
                // of this row matches the index of the header so the
                // FirstName column header lines up with the FirstName
                // value in this row
                var vals = row.Split(',');

                // Loop through each header entry so we can compare that
                // against the list of columns from reflection. Once we get 
                // the matching column, we can do the SetValue method to
                // set the column value of our entry variable to the vals
                // item at the same index as this particular header.

                for(var i=0; i < headers.Length; i++)
                {
                    foreach(var col in cols)
                    {
                        if(col.Name == headers[i])
                        {
                            col.SetValue(entry, Convert.ChangeType(vals[i], col.PropertyType));
                        }
                    }
                }
                output.Add(entry);
            }
            
            return output;
        }

        public static void SaveToTextFile<T>(List<T> data, string filePath) where T : class//, new()
        {
            List<string> lines = new List<string>();
            StringBuilder line = new StringBuilder();

            if (data == null || data.Count == 0)
            {
                throw new ArgumentException("data", "You must populate the data with at least one line");
            }

            var cols = data[0].GetType().GetProperties();

            // Loops through each column and gets the name so it can comma
            // separate it into the header row.
            foreach (var col in cols)
            {
                line.Append(col.Name);
                line.Append(",");
            }

            // Adds the column header entries to the first line ( removing
            // the last comma from the end first).
            lines.Add(line.ToString().Substring(0, line.Length - 1));

            foreach (var row in data)
            {
                line = new StringBuilder();
                foreach (var col in cols)
                {
                    line.Append(col.GetValue(row));
                    line.Append(",");
                }

                // Adds the row to the set of lines ( removing
                // the last comma from the end of first line)
                lines.Add(line.ToString().Substring(0, line.Length - 1));   
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}

```
              
            

If we need to pass two Types to the method, we can use in the below format

![image](https://user-images.githubusercontent.com/32676744/223408095-e94f5d29-cc3f-46e6-976f-a9f890d8ebcb.png)


 Old Way
              
 ```
 using ConsoleUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.WithoutGenerics
{
    public class OriginalTextFileProcessor
    {
        private static void DemonstrateTextFileStorage()
        {
            List<Person> people = new List<Person>();
            List<LogEntry> logs = new List<LogEntry>();

            string peopleFile = @"C:\Temp\people.csv";
            string logFile = @"C:\Temp\logs.csv";

            PopulateLists(people, logs);

            OriginalTextFileProcessor.SavePeople(people,peopleFile);

            var newPeople = OriginalTextFileProcessor.LoadPeople(peopleFile);

            foreach (var p in newPeople)
            {
                Console.WriteLine($"{p.FirstName} {p.LastName} (IsAlive = {p.IsAlive})");
            }
        }

        private static void PopulateLists(List<Person> people, List<LogEntry> logs)
        {

        }

        public static List<Person> LoadPeople(string filePath)
        {
            List<Person> output = new List<Person>();
            Person p;
            var lines = File.ReadAllLines(filePath).ToList();

            //Remove the header row

            lines.RemoveAt(0);

            foreach(var line in lines)
            {
                var vals = line.Split(',');
                p = new Person();

                p.FirstName = vals[0];
                p.IsAlive = bool.Parse(vals[1]);
                p.LastName = vals[2];

                output.Add(p);
            }
            return output;
        }

        public static List<LogEntry> LoadLogs(string filePath)
        {
            List<LogEntry> output = new List<LogEntry>();
            LogEntry l;
            var lines = File.ReadAllLines(filePath).ToList();

            //Remove the header row

            lines.RemoveAt(0);

            foreach (var line in lines)
            {
                var vals = line.Split(',');
                l = new LogEntry();

                l.ErrorCode = int.Parse(vals[0]);
                l.Message = vals[1];
                l.TimeOfEvent = DateTime.Parse(vals[2]);

                output.Add(l);
            }
            return output;
        }


        public static void SavePeople(List<Person> people,string filePath)
        {
            List<string> lines = new List<string>();

            //Add header row
            lines.Add("FirstName,IsAlive,LastName");

            foreach(var p in people)
            {
                lines.Add($"{p.FirstName},{p.IsAlive},{p.LastName}");
            }
            File.WriteAllLines(filePath,lines);
        }

        public static void SaveLogs(List<LogEntry> logs, string filePath)
        {
            List<string> lines = new List<string>();

            //Add header row
            lines.Add("ErrorCode,Message,TimeOfEvent");

            foreach (var l in logs)
            {
                lines.Add($"{l.ErrorCode},{l.Message},{l.TimeOfEvent}");
            }
            File.WriteAllLines(filePath, lines);
        }

    }
}

 ```
              
 Main Program
 
 ```
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
                         
 ```
 Person Model
 ```
 public class Person
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsAlive { get; set; } = true;
    }
 ```
 LogEntry Model
 
```
 public  class LogEntry
    {
        public int ErrorCode { get; set; }
        public string? Message { get; set; }
        public DateTime TimeOfEvent { get; set; } = DateTime.UtcNow;
    }
```
 


