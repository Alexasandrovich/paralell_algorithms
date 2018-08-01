using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace lab2
{
    class Program
    {

        static double[,] semi_result;
        static double[,] matrix;
        static string readfrom = @"C:\matrix.csv";
        static string writeto = @"C:\Users\Alexa\source\repos\Matrix\Matrix\result.csv";

        static void Main(string[] args)
        {
            Stopwatch stop = new Stopwatch();
            stop.Start();
            //filling the matrix
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
            string[] string_matrix = File.ReadAllLines(readfrom);
            int le = string_matrix.Length;
            matrix = new double[le, le];
            semi_result = new double[le, le];
            int i_m = 0;
            foreach (string line in string_matrix)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                {
                    matrix[i_m, j] = Double.Parse(str, CultureInfo.InvariantCulture);
                    ++j;
                }
                ++i_m;
            }
            Console.WriteLine("filling done successfully");
            //-------------

            //calculating the stuff
            for (int i = 0; i < le; i++)
            {
                Parallel.For(0, le, (j) =>
                {
                    semi_result[i, j] = calc(i, j, le);
                });
                if (i % 50 == 0)
                    Console.WriteLine("Calculated " + i.ToString() + "-th row");
            }
            Console.WriteLine("calculated successfully");
            //--------------------

            //writing result
            string[] result = new string[le];
            Parallel.For(0, le, (cur_row) =>
            {
                result[cur_row] = numstostr(cur_row, le, ref semi_result);
                if (cur_row % 100 == 0)
                    Console.WriteLine("Reached " + cur_row.ToString() + "-th row");
            });
            File.WriteAllLines(writeto, result);
            Console.WriteLine("wrote result successfully");
            //-------------------
            stop.Stop();
            Console.WriteLine("Time is " + stop.ElapsedMilliseconds.ToString() + " millis parfor");


        }

        static double calc(int i, int j, int L) //calculating [i,j]-th cell of matrix
        {
            double res = 0;
            for (int k = 0; k < L; k++)
            {
                res += matrix[k, j] * matrix[i, k];
            }
            return res;
        }

        static string numstostr(int row, int L, ref double[,] m)// i-th row of matrix into string
        {
            string res = m[row, 0].ToString();
            for (int i = 1; i < L; i++)
            {
                res += "," + m[row, i];
            }
            return res;
        }
    }

}