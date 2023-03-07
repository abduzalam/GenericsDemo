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
