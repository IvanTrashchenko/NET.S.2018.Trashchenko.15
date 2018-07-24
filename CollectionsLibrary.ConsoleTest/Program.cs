using System;

namespace CollectionsLibrary.ConsoleTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Queue<int> a = new Queue<int>();
            Queue<string> b = new Queue<string>(new string[] { "f0", "f1", "f2", "f3", "f4", "f5" });
            Queue<TimeSpan> c = new Queue<TimeSpan>(8);

            for (int i = 1; i <= 7; i++)
            {
                a.Enqueue(i);
            }

            a.Dequeue();
            a.Dequeue();

            Console.WriteLine(a.Contains(3)); // true

            Console.WriteLine(a.Contains(0)); // true

            foreach (int item in a)
            {
                Console.WriteLine(item);
            }

            /*0
              0
              3 head 0ind
              4      1ind
              5      2ind
              6
              7 tail
              0*/

            Console.WriteLine(a.GetElement(2)); // 5

            a.TrimExcess();

            foreach (int item in a)
            {
                Console.WriteLine(item);
            }

            /*3 head
              4
              5
              6
              7 tail*/

            Console.WriteLine(b.Peek()); // f0

            Console.WriteLine(b.Contains("f3")); // true

            Console.WriteLine(b.Contains(null)); // false

            b.Clear();

            Console.WriteLine(b.Contains("f3")); // false

            Console.WriteLine(b.Contains(null)); // true

            foreach (string item in b)
            {
                Console.WriteLine(item); // 8 nulls because of capacity increasing
            }

            for (int i = 0; i < 1000; i++)
            {
                c.Enqueue(new TimeSpan(i));
            }

            for (int i = 0; i < 500; i++)
            {
                c.Dequeue();
            }

            var arr = c.ToArray();

            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }


            Console.ReadKey();
        }
    }
}
