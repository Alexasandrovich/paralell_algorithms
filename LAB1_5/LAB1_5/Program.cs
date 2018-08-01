using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace additional5
{
    class Program
    {
        static int global_index = -1;
        static String command;

        static void Main(string[] args)
        {
            while (true)
            {
                command = Console.ReadLine();

                if (command == "S")
                {
                    for (int i = 1; i < 4; ++i)
                    {
                        ++global_index;
                        Thread T = new Thread(Count);
                        T.Name = global_index.ToString();

                        T.Start();
                    }
                }

                if (command == "A")
                {
                    if (global_index > -1)
                        global_index -= 1;
                }

                if (command == "F")
                {
                    global_index = -1;
                }
            }
        }

        static void Count()
        {
            int index = int.Parse(Thread.CurrentThread.Name);

            int res = 0;
            Random rnd = new Random();

            for (; global_index >= index; ++res)
                Thread.Sleep(rnd.Next(1, 6));

            if (command == "F")
                Console.WriteLine("Поток " + Thread.CurrentThread.Name + " досчитал до " + res.ToString());
        }
    }

}