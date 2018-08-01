using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace lab2Additional1
{
    class Program
    {
        //static double[,] semi_result;
        static Task<double>[,] semi_result;
        static double[,] matrix;
        static string readfrom = @"C:\matrix.csv";
        static string writeto = @"C:\Users\Alexa\source\repos\lab_2_add_1\result.csv";

        static void Main(string[] args)
        {
            Stopwatch stop = new Stopwatch();
            //filling the matrix
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
            string[] string_matrix = File.ReadAllLines(readfrom);
            int le = string_matrix.Length;
            matrix = new double[le, le];
            //semi_result = new double[le, le];
            semi_result = new Task<double>[le, le];
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

            stop.Start();
            for (int i = 0; i < le; i++)
            {
                for (int j = 0; j < le; j++)
                {
                    Tuple<int, int, int> cur = new Tuple<int, int, int>(i, j, le);
                    semi_result[i, j] = new Task<double>((x) => calc((Tuple<int, int, int>)x), cur);
                    semi_result[i, j].Start();
                }
            }
            for (int i = 0; i < le; i++)
            {
                for (int j = 0; j < le; j++)
                {
                    semi_result[i, j].Wait();
                }
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
            Console.WriteLine("Time is " + stop.ElapsedMilliseconds.ToString() + " millis task");
            Console.ReadKey();
        }

        static double calc(Tuple<int, int, int> cur) //calculating [i,j]-th cell of matrix
        {
            int i = cur.Item1;
            int j = cur.Item2;
            int L = cur.Item3;
            double res = 0;
            for (int k = 0; k < L; k++)
            {
                res += matrix[k, j] * matrix[i, k];
            }
            return res;
        }

        static string numstostr(int row, int L, ref Task<double>[,] m)// i-th row of matrix into string
        {
            string res = m[row, 0].ToString();
            for (int i = 1; i < L; i++)
            {
                res += "," + m[row, i].Result;
            }
            return res;
        }
    }

}