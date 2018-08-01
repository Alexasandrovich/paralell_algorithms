using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace additional2
{
    class Program
    {

        static double elem = 5 * 1e-2;

        static void Main(string[] args)
        {
            double a = 1e9, b = -1e9;
            do
            {
                Console.WriteLine("Please input segment borders in right order");
                a = double.Parse(Console.ReadLine());
                b = double.Parse(Console.ReadLine());
            }
            while (a > b);

            int num_elem = Convert.ToInt32(Math.Floor((b - a) / elem));
            ++num_elem;
            elem = (b - a) / num_elem;


            int[] extrems = new int[num_elem];

            double current = a;

            Parallel.For(0, num_elem, (i) =>
            {
                extrems[i] = calc(current + i * elem, current + (1 + i) * elem);
            });


            int maxes = 0;
            int mins = 0;

            for (int i = 0; i < num_elem; ++i)
            {
                if (extrems[i] == 1) ++maxes;
                if (extrems[i] == -1) ++mins;
            }

            Console.WriteLine("number of maximums is " + maxes.ToString());
            Console.WriteLine("number of minimums is " + mins.ToString());
            Console.ReadKey();

        }

        static int calc(double left, double right)
        {// 0 = no extrems, 1 = max, -1 = min

            if (right < left) return 0;

            double eps = (right - left) / 1e5;
            int sign1 = sign(left, left + eps);
            int sign2 = sign(right - eps, right);

            if (sign1 == sign2) return 0;

            if (sign1 == 1)
                return 1;
            else
                return -1;

        }

        static double f(double x)
        {
            return Math.Cos(2 * x) + Math.Sin(3 * x);
        }

        static int sign(double a, double b)
        {
            if (f(b) > f(a)) return 1;
            if (f(b) < f(a)) return -1;
            return 0;
        }
    }
}