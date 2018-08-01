
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
        static double summ(double[,] arr, int row, int length)
        {
            double result = 0;
            for (int k = 0; k < length; k++)
            {
                result += arr[row, k];
            }
            return result;
        }

        static double mid(double[,] arr, int row, int length)
        {
            double result = 0;
            for (int k = 0; k < length; k++)
            {
                result += arr[row, k];
            }
            return result / length;
        }

        static void Main(string[] args)
        {
            double[] final_matrix_1; // put summ here
            double[] final_matrix_2; // put mid here
            double[,] matrix; // start matrix
            string readfrom = @"C:\matrix.csv";
            string writeto = @"C:\Users\Alexa\source\repos\lab_1_add_3\final.csv";
            string writeto_2 = @"C:\Users\Alexa\source\repos\lab_1_add_3\final_2.csv";

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
            string[] string_matrix = File.ReadAllLines(readfrom); // read all rows
            int le1 = string_matrix.Length; // find number rows

            string[] columns = string_matrix[0].Split(','); 
            int le2 = columns.Length; // find number of columns start matrix
            matrix = new double[le1, le2]; 

            int iterator_1 = 0;
            foreach (string line in string_matrix)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                {
                    matrix[iterator_1, j] = Double.Parse(str, CultureInfo.InvariantCulture);
                    ++j;
                }
                ++iterator_1;
            }
            Console.WriteLine("Hobba! You've read matrix!");
            Console.WriteLine("Let's find summ for each row");

            final_matrix_1 = new double[le1];
            final_matrix_2 = new double[le1];

            Console.WriteLine("Summ:");
            Parallel.For(0, le1, (j) =>
            {
                final_matrix_1[j] = summ(matrix, j, le2);
            });
            Console.WriteLine("Double hobba!");

            Console.WriteLine("Mid:");
            Parallel.For(0, le1, (j) =>
            {
                final_matrix_2[j] = mid(matrix, j, le2);
            });
            Console.WriteLine("hobbanality!");

            string[] result = new string[le1];
            string[] result_2 = new string[le1]; // for save


            Parallel.For(0, le1, (cur_row) =>
            {
                result[cur_row] = final_matrix_1[cur_row].ToString();
            });

            Parallel.For(0, le1, (cur_row) =>
            {
                result_2[cur_row] = final_matrix_2[cur_row].ToString();
            });

            File.WriteAllLines(writeto, result);
            File.WriteAllLines(writeto_2, result_2);
            Console.WriteLine("Finish!");
            Console.ReadKey();


        }
    }

}
