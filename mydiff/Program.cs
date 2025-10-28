using System.Net.Mime;
using System.Text;

namespace mydiff
{
    class Program
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

            //create the matrix for the LCS
            int firstFileCount = firstFileE.Count() + 1; // + 1 is for the empty string
            int secondFileCount = secondFileE.Count() + 1;
            int[,] lengthLCS = new Int32[firstFileCount, secondFileCount];
            Line[] linesFirst = new Line[firstFileCount];
            Line[] linesSecond = new Line[secondFileCount];
            int idx = 0;
            linesFirst[idx] = new Line() { NumberLine = idx++, ContentLine = "" };
            foreach (string line in firstFileE)
            {
                Line l = new Line() { NumberLine = idx + 1, ContentLine = line };
                linesFirst[idx++] = l;
            }

            idx = 0;
            linesSecond[idx] = new Line() { NumberLine = idx++, ContentLine = "" };
            foreach (string line in secondFileE)
            {
                Line l = new Line() { NumberLine = idx + 1, ContentLine = line };
                linesSecond[idx++] = l;
            }

            for (int i = 0; i < firstFileCount; i++)
            {
                for (int j = 0; j < secondFileCount; j++)
                {
                    if (j == 0 || i == 0)
                    {
                        //the LCS for the empty string is always 0 
                        lengthLCS[i, j] = 0;
                    }
                    else if (linesFirst[i].ContentLine.Equals(linesSecond[j].ContentLine))
                    {
                        //if the content matches get the upper-left value and increase it by 1
                        lengthLCS[i, j] = lengthLCS[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        //if the content don't match get the max value between the left and upper value
                        if (lengthLCS[i, j - 1] > lengthLCS[i - 1, j])
                        {
                            lengthLCS[i, j] = lengthLCS[i, j - 1];
                        }
                        else
                        {
                            lengthLCS[i, j] = lengthLCS[i - 1, j];
                        }
                    }
                }
            }
        }
    }
}

