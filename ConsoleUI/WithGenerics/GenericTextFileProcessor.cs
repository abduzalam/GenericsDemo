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
