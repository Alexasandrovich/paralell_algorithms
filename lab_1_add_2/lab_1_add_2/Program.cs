using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1_add_2
{
    class Program
    {
        static void Main(string[] args)
        {
            double a, b;

            const double epsilon = 1e-10;

            do
            {
                a = double.Parse(Console.ReadLine());
                b = double.Parse(Console.ReadLine());
            } while (a > b);


        }
        static double Nuton(double start, double eps)
        {// x_n+1 = x_n - (f'(x_n)/f''(x_n))
            double x1, dx;
            double x0 = start;
            do
            {
                x1 = x0 - df_f(x0) / dfdf_f(x0);
                dx = Math.Abs(x1 - x0);
                x0 = x1;
            }
            while (dx < eps);
            if (dfdf_f(x1) < 0)
            {
                return x1;
            }
            else
            {
                throw new Exception("Экстремум на этом интервале не является максимумом!");
            }
        }
        static double df_f(double x)
        {
            return -2* Math.Sin(2 * x) + 3*Math.Cos(3 * x);
        }

        static double dfdf_f(double x)
        {
            return -4 * Math.Cos(2 * x) - 9 * Math.Sin(3 * x);
        }
    }
}
