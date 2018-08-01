
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab2
{
    class Program
    {
        static double summ(double[,] m, int row, int length)
        {
            double res = 0;
            for (int k = 0; k < length; k++)
            {
                res += m[row, k];
            }
            return res;
        }

        static double mid(double[,] m, int row, int length)
        {
            double res = 0;
            for (int k = 0; k < length; k++)
            {
                res += m[row, k];
            }
            return res/length;
        }

        static double[] final_matrix_1;
        static double[] final_matrix_2;
        static double[,] matrix;
        static string readfrom = @"C:\matrix.csv";
        static string writeto = @"C:\Users\Alexa\source\repos\Matrix\Matrix\end_matrix.csv";

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
            string[] string_matrix = File.ReadAllLines(readfrom);
            int le = string_matrix.Length;

            string[] forsize = string_matrix[0].Split(',');
            int le2 = forsize.Length;
            matrix = new double[le, le2];

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
            Console.WriteLine("Hobba! You've read matrix!");
            Console.WriteLine("Let's find summ for each row");

            final_matrix_1 = new double[le];
            final_matrix_2 = new double[le];

            Console.WriteLine("Summ:");
             Parallel.For(0, le, (j) =>
                {
                    final_matrix_1[j] = summ(matrix, j, le2);
                        Console.WriteLine("Calculated " + j.ToString() + "-th row");
                });
            Console.WriteLine("Double hobba!");

            Console.WriteLine("Mid:");
            Parallel.For(0, le, (j) =>
            {
                final_matrix_2[j] = mid(matrix, j, le2);
                Console.WriteLine("Calculated " + j.ToString() + "-th row");
            });
            Console.WriteLine("hobbanality!");

            string[] result = new string[le];

            Parallel.For(0, le, (cur_row) =>
            {
                result[cur_row] = final_matrix_1[cur_row].ToString();
                if (cur_row % 100 == 0)
                    Console.WriteLine("Reached " + cur_row.ToString() + "-th row");
            });

            Parallel.For(0, le, (cur_row) =>
            {
                result[cur_row] = final_matrix_2[cur_row].ToString();
                if (cur_row % 100 == 0)
                    Console.WriteLine("Reached " + cur_row.ToString() + "-th row");
            });

            File.WriteAllLines(writeto, result);
            Console.WriteLine("Finish!");
            Console.ReadKey();


        }
    }

}
