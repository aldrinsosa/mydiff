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
            string path = Directory.GetCurrentDirectory();
            string arg1 = args[0];
            string arg2 = args[1];
            IEnumerable<string> firstFileE;
            IEnumerable<string> secondFileE;
            
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
            Console.WriteLine("First file\n");

            var firstFile = firstFileE as string[] ?? firstFileE.ToArray();
            var secondFile = secondFileE as string[] ?? secondFileE.ToArray();
            for (int i = 0; i < firstFile.Count(); i++)
            {
                if (firstFile[i].Equals(secondFile[i]))
                {
                    Console.WriteLine($"     {firstFile[i]}");
                }
                else
                {
                    Console.WriteLine($" {i} - {firstFile[i]}");
                    Console.WriteLine($" {i} + {secondFile[i]}");
                }
            }
            
        }
    }
}

