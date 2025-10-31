﻿using System.Net.Mime;
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
            linesFirst[idx] = new Line() { NumberLine = idx++, ContentLine = "", };
            foreach (string line in firstFile)
            {
                Line l = new Line() { NumberLine = idx + 1, ContentLine = line, IsLcs = false, IsFirst = true};
                linesFirst[idx++] = l;
            }

            idx = 0;
            //create the empty string for the LCS
            linesSecond[idx] = new Line() { NumberLine = idx++, ContentLine = ""};
            foreach (string line in secondFile)
            {
                Line l = new Line() { NumberLine = idx + 1, ContentLine = line,  IsLcs = false, IsFirst = false};
                linesSecond[idx++] = l;
            }

            //get the matrix with the length of the LCS
            int[,] lengthLcs = GetLengthLcs(linesFirst, linesSecond);

            int [][] traceback = TracebackLcs(lengthLcs,  firstFileCount, secondFileCount);
            
            SetLcs(traceback, linesFirst, linesSecond);
            
            PrintDiff(linesFirst, linesSecond);
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

        private static int[][] TracebackLcs(int[,] lengthLcs, int firstFileCount, int secondFileCount)
        {
            int[][] traceback = new Int32[2][];
            int[] firstTraceback = new Int32[lengthLcs[firstFileCount-1,secondFileCount-1]];
            int[] secondTraceback = new Int32[lengthLcs[firstFileCount-1,secondFileCount-1]];
            int actualLcs = 0;
            int indexLcs = 0;
            for (int i = 1; i < firstFileCount; i++)
            {
                for (int j = 1; j < secondFileCount; j++)
                {
                    if (lengthLcs[i, j] > actualLcs)
                    {
                        firstTraceback[indexLcs] = i;
                        secondTraceback[indexLcs++] = j;
                        actualLcs = lengthLcs[i, j];
                    }
                }
            }

            traceback[0] = firstTraceback;
            traceback[1] = secondTraceback;
            return traceback;
        }

        private static void SetLcs(int[][] traceback, Line[] linesFirst, Line[] linesSecond)
        {
            for (int i = 0; i < traceback.Length; i++)
            {
                for (int j = 0; j < traceback[i].Length; j++)
                {
                    if (i == 0)
                    {
                        linesFirst[traceback[i][j]].IsLcs = true;
                    }
                    else
                    {
                        linesSecond[traceback[i][j]].IsLcs = true;
                    }
                }
            }
        }

        private static void PrintDiff(Line[] linesFirst, Line[] linesSecond)
        {
            //obtain which is the longest array
            Line[] maxLines = linesFirst.Length > linesSecond.Length ? linesFirst : linesSecond;
            Line[] minLines = maxLines == linesFirst ? linesSecond : linesFirst;
            
            for (int i = 1; i < maxLines.Length; i++)
            {
                if (maxLines[i].IsLcs)
                {
                    PrintLine(maxLines[i], "none");
                }
                else if (minLines.Length <= i)
                {
                    if (maxLines[i].IsFirst)
                    {
                        PrintLine(maxLines[i], "minus");
                    }
                    else
                    {
                        PrintLine(maxLines[i], "plus");
                    }
                }
                else
                {
                    if (maxLines[i].IsFirst)
                    {
                        PrintLine(minLines[i], "plus");
                        PrintLine(maxLines[i], "minus");
                    }
                    else
                    {
                        PrintLine(maxLines[i], "plus");
                        PrintLine(minLines[i], "minus");
                    }
                }
            }
        }

        private static void PrintLine(Line line, string sign)
        {
            Console.Write($"{line.NumberLine} ");
            if (sign == "minus")
            {
                Console.Write($"-");
            }
            else if (sign == "plus")
            {
                Console.Write($"+");
            }
            else
            {
                Console.Write($" ");
            }
            Console.WriteLine($" {line.ContentLine}");
        }
    }
}

