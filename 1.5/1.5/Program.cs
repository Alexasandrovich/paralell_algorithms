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
        static int thr, points;

        static int[] final_mas;
        
        static void find_num()
        {
            int res = 0;
            int index = int.Parse(Thread.CurrentThread.Name);

            for (int i = 0; i < points; i++)
            {
                Random rnd = new Random(i);

                double x = rnd.NextDouble();
                double y = rnd.NextDouble();

                if (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) < 1)
                    res++;
                            }
            final_mas[index] = res;
        }

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("Put down number of treads");
            thr = int.Parse(Console.ReadLine());
            Console.WriteLine("Put down number of points");
            points = int.Parse(Console.ReadLine());

            final_mas = new int[thr]; // сюда будем впихивать результаты каждого потока

            int result = 0;

            Thread[] t = new Thread[thr];
            int[] res = new int[thr];

            for (int i = 0; i < thr; i++)
            {
                res[i] = 0;
                t[i] = new Thread(find_num);
                t[i].Name = i.ToString(); // Имя нужно, чтобы обращаться к каждому потоку отдельно
                t[i].Start();
            }

            for (int i = 0; i < thr; i++)
            {
                t[i].Join(); //Блокиреут вызывающий поток до завершения потока, представленного экземпялра 
                result += final_mas[i];
            }

            Console.WriteLine("PI~{0}",(double)(4 * result) / (points * thr));
            sw.Stop();
            Console.WriteLine("time is " + (sw.ElapsedMilliseconds / 1000.0).ToString());
            Console.ReadKey();
        }

    }
}