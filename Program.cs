using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace stringFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(@"Type the location of the folder to inspect (ie: ""C:\docs\"" ) ");
            string fileLocation = Console.ReadLine();

            System.Console.WriteLine("Type the regular expression to match");
            //TODO add string input validation
            string regexPattern = Console.ReadLine(); // Example:  @"(Session\[""[a-zA-Z0-9]\w+\""\])"

            System.Console.WriteLine("Chose an optional file name? (default is : MyMatches)");
            string fileName = Console.ReadLine();

            string[] list = Program.fileList($"{fileLocation}");

            Regex pattern = new Regex($"{regexPattern}");
            
            Find(list, pattern , fileName);

        }

        private static void Find(string[] list, Regex pattern, string fileName = "MyMatches")
        {
            MatchCollection matches = null;

            var resultList = new List<string>();

            foreach (var file in list)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string line;

                    System.Console.WriteLine($"Processing file{file} ");

                    while ((line = sr.ReadLine()) != null)
                    {
                        matches = pattern.Matches(line);

                        if (pattern.IsMatch(line) && !resultList.Contains(matches[0].Value))
                        {
                            resultList.Add(matches[0].Value);
                        }

                    }
                }

            }

            if (resultList != null)
            {
                Console.WriteLine("Writing File ");

                var outFileName = $"{fileName}-{DateTime.Now.ToFileTime()}.txt";

                using (var fs = new FileStream(outFileName, FileMode.CreateNew))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        foreach (var r in resultList)
                        {
                            sw.WriteLine(r);
                        }
                    }
                }

                Console.WriteLine("Process Completed!");

            }

            else
            {
                System.Console.WriteLine("No matches found!");
            }
        }

        public static string[] fileList(string folder)
        {
            return Directory.GetFiles($@"{folder}");
        }
    }
}