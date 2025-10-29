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
            
            var firstFile = firstFileE as string[] ?? firstFileE.ToArray();
            var secondFile = secondFileE as string[] ?? secondFileE.ToArray();
           
            // + 1 is for the empty string
            int firstFileCount = firstFile.Count() + 1; 
            int secondFileCount = secondFile.Count() + 1;
            
            Line[] linesFirst = new Line[firstFileCount];
            Line[] linesSecond = new Line[secondFileCount];
            
            int idx = 0;
            //create the empty string for the LCS
            linesFirst[idx] = new Line() { NumberLine = idx++, ContentLine = "", IsLcs = false};
            foreach (string line in firstFile)
            {
                Line l = new Line() { NumberLine = idx + 1, ContentLine = line, IsLcs = false};
                linesFirst[idx++] = l;
            }

            idx = 0;
            //create the empty string for the LCS
            linesSecond[idx] = new Line() { NumberLine = idx++, ContentLine = "", IsLcs = false};
            foreach (string line in secondFile)
            {
                Line l = new Line() { NumberLine = idx + 1, ContentLine = line,  IsLcs = false};
                linesSecond[idx++] = l;
            }

            //get the matrix with the length of the LCS
            int[,] lengthLcs = GetLengthLcs(linesFirst, linesSecond);

            int [] traceback = TracebackLcs(lengthLcs,  firstFileCount, secondFileCount);
            
            SetLcs(traceback, linesFirst, linesSecond);
            
            PrintDiff(linesFirst, linesSecond);
            //TODO: Print the diff
        }

        private static int[,] GetLengthLcs(Line[] linesFirst, Line[] linesSecond)
        {
            int linesFirstCount = linesFirst.Count();
            int linesSecondCount = linesSecond.Count();
            int[,] lengthLcs = new Int32[linesFirstCount, linesSecondCount];
            for (int i = 0; i < linesFirstCount; i++)
            {
                for (int j = 0; j < linesSecondCount; j++)
                {
                    if (j == 0 || i == 0)
                    {
                        //the LCS for the empty string is always 0 
                        lengthLcs[i, j] = 0;
                    }
                    else if (linesFirst[i].ContentLine.Equals(linesSecond[j].ContentLine))
                    {
                        //if the content matches get the upper-left value and increase it by 1
                        lengthLcs[i, j] = lengthLcs[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        //if the content don't match get the max value between the left and upper value
                        if (lengthLcs[i, j - 1] > lengthLcs[i - 1, j])
                        {
                            lengthLcs[i, j] = lengthLcs[i, j - 1];
                        }
                        else
                        {
                            lengthLcs[i, j] = lengthLcs[i - 1, j];
                        }
                    }
                }
            }
            return lengthLcs;
        }

        private static int[] TracebackLcs(int[,] lengthLcs, int firstFileCount, int secondFileCount)
        {
            int[] traceback = new Int32[lengthLcs[firstFileCount - 1, secondFileCount -1]];
            int actualLcs = 0;
            int indexLcs = 0;
            for (int i = 1; i < firstFileCount; i++)
            {
                for (int j = 1; j < secondFileCount; j++)
                {
                    if (lengthLcs[i, j] > actualLcs)
                    {
                        traceback[indexLcs++] = i;
                        actualLcs = lengthLcs[i, j];
                    }
                }
            }

            return traceback;
        }

        private static void SetLcs(int[] traceback, Line[] linesFirst, Line[] linesSecond)
        {
            foreach (var t in traceback)
            {
                linesFirst[t].IsLcs = true;
                linesSecond[t].IsLcs = true;
            }
        }

        private static void PrintDiff(Line[] linesFirst, Line[] linesSecond)
        {
            Console.WriteLine("First file");
            foreach (var line in linesFirst)
            {
                if (line.IsLcs)
                {
                    Console.WriteLine($"{line.NumberLine}   {line.ContentLine}");
                }
                else
                {
                    Console.WriteLine($"{line.NumberLine} - {line.ContentLine}");
                }
            }
            Console.WriteLine("Second file");
            foreach (var line in linesSecond)
            {
                if (line.IsLcs)
                {
                    Console.WriteLine($"{line.NumberLine}   {line.ContentLine}");
                }
                else
                {
                    Console.WriteLine($"{line.NumberLine} + {line.ContentLine}");
                }
            }
        }
    }
}

