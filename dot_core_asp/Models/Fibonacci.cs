using System;
using System.Collections.Generic;
using System.Linq;

namespace dot_core_asp.Models
{
    public class Fibonacci
    {
        public int FibonacciRecursive(int s)
        {
            int result;
            if (s == 0)
            {
                result = 0;
            }
            else if (s == 1)
            {
                result = 1;
            }
            else
            {
                result = FibonacciRecursive(s - 1) + FibonacciRecursive(s - 2);
            }
            // Console.WriteLine(s);
            return result;
        }        

        public void FibonacciIterative(int s)
        {
            var range = Enumerable.Range(2, s-1);
            var sums = new int[s+1];
            sums[0] = 0;
            sums[1] = 1;
            foreach (var ite in range)
            {
                // if (ite == 0 | ite == 1)
                    // sums[ite] = ite;
                // else
                sums[ite] = sums[ite-1] + sums[ite-2];
            }
            Console.WriteLine(String.Join("; ", sums));
        }

        public void TestFibonacci()
        {
            int input = 3;
            var result = FibonacciRecursive(input);
            var inputFibo = Enumerable.Range(0, 10);
            foreach (var ite in inputFibo)
                Console.WriteLine(FibonacciRecursive(ite));
            // Console.WriteLine(String.Format("Fibonacci result: {0} from input s={1}", result, input));
            FibonacciIterative(9);
        }
    }
}