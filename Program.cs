using System.Linq;
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
            try
            {
                System.Console.WriteLine(@"Type the location of the folder to inspect (ie: ""C:\docs\"" ) ");
                string[] list = fileList(@Console.ReadLine());

                System.Console.WriteLine("Type the regular expression to match");
                // Example:  @"(Session\[""[a-zA-Z0-9]\w+\""\])"
                Regex pattern = new Regex(@Console.ReadLine());

                System.Console.WriteLine("Chose an optional file name? (default is : MyMatches)");
                string fileName = Console.ReadLine().ToString();
            
                Find(list, pattern , fileName);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Find(string[] list, Regex pattern, string fileName = "MyMatches")
        {
            List<string> resultList = FindMatches(list, pattern);

            if (resultList == null || !resultList.Any())
            {
                System.Console.WriteLine("No matches found!");
            }

            ProcessMatches(fileName, resultList);
            
            Console.WriteLine("Process Completed!");

        }
        private static bool ProcessMatches(string fileName, List<string> resultList)
        {
            try
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
                
                return true;
            }
            catch (System.Exception)
            {
                throw new Exception();
            }
        }
        private static List<string> FindMatches(string[] list, Regex pattern)
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

            return resultList;
        }
        public static string[] fileList(string folder)
        {
            try
            {
                return Directory.GetFiles($@"{folder}");

            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException();   
            }
        }
    }
}