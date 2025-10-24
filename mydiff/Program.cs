using System.Net.Mime;
using System.Text;

namespace mydiff
{
    class  Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: mydiff <first_file> <second_file>");
                return;
            }
            //set address of the files
            string path = Directory.GetCurrentDirectory();
            string arg1 = args[0];
            string arg2 = args[1];
            IEnumerable<string> firstFileE;
            IEnumerable<string> secondFileE;
            
            //read the files
            try
            {
                firstFileE = File.ReadLines($"{path}/{arg1}");
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            try
            {
                secondFileE = File.ReadLines($"{path}/{arg2}");
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Dictionary<int, Line> linesFirst = new Dictionary<int, Line>();
            Dictionary<int, Line> linesSecond = new Dictionary<int, Line>();
            int idx = 1;
            foreach (string line in firstFileE)
            {
                Line l = new Line() {NumberLine = idx++, ContentLine = line};
                linesFirst.Add(idx, l);
            }
            idx = 1;
            foreach (string line in secondFileE)
            {
                Line l = new Line() {NumberLine = idx++, ContentLine = line};
                linesSecond.Add(idx, l);
            }
            
            foreach (var l in linesFirst)
            {
                if (linesSecond.ContainsKey(l.Key))
                {
                    try
                    {
                        Console.WriteLine(linesSecond[l.Key].ContentLine);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    System.Environment.Exit(0);
                }
            }
        }
    }
}

