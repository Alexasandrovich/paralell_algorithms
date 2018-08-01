using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace additional4 // переделать

{
    class Program
    {
        static bool[] arr = new bool[20];

        static void Main(string[] args)
        {
            for (int i = 0; i < 20; ++i)
            {
                arr[i] = false;
            }

            Thread first = new Thread(Count);
            Thread second = new Thread(Count);

            first.Name = "0";
            second.Name = "10";

            first.Start();
            second.Start();

            int cur_i = 0;

            for (; cur_i < 20;)
            {
                Thread.Sleep(1);
                if (arr[cur_i] == true)
                    Console.WriteLine((++cur_i).ToString());
            }
        }

        static void Count()
        {
            Random rnd = new Random();
            int start_index = int.Parse(Thread.CurrentThread.Name);

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(rnd.Next(100, 501));
                arr[start_index + i] = true;
            }
        }
    }

}