using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApp25
{
    class Program
    {
        static int Calc(int n)
        {
            int res = 0;

            for (int i = 0; i < n; i++)
            {
                Random random = new Random();

                double x = random.NextDouble();
                double y = random.NextDouble();

                if (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) < 1)
                    res++;
            }
            return res;
        }

        static void Main(string[] args)
        {
            int k, n;
            k = int.Parse(Console.ReadLine()); // number of threads
            n = int.Parse(Console.ReadLine()); // number of points
            int N = 0; // number of points, that hit circle

            Task<int>[] t = new Task<int>[k];
            int[] res = new int[k]; // massive for all threads for next summing

            for (int i = 0; i < k; i++)
            {
                res[i] = 0;
                t[i] = new Task<int>(x => Calc((int)x), n); // n - number of of points, that hit circle for one thread
                t[i].Start();
            }

            for (int i = 0; i < k; i++)
            {
                N += t[i].Result;
            }

            Console.WriteLine("pi ~ {0}", (double)(4 * N) / (n * k)); // n*k - number of all points; don't round off
            Console.ReadLine();
        }

    }
}
