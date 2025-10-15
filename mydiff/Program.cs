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
            IEnumerable<string> firstFile;
            IEnumerable<string> secondFile;
            
            try
            {
                firstFile = File.ReadLines(string.Format("{0}/{1}", path, arg1));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            try
            {
                secondFile = File.ReadLines(string.Format("{0}/{1}", path, arg2));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            Console.WriteLine("First file\n");
            foreach (string line in firstFile)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("\nSecond file\n");
            foreach (string line in secondFile)
            {
                Console.WriteLine(line);
            }
        }
    }
}

