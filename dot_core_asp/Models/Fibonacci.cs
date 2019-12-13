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

        public void TestFibonacci()
        {
            int input = 3;
            var result = FibonacciRecursive(input);
            var inputFibo = Enumerable.Range(0, 10);
            foreach (var ite in inputFibo)
                Console.WriteLine(FibonacciRecursive(ite));
            // Console.WriteLine(String.Format("Fibonacci result: {0} from input s={1}", result, input));
        }
    }
}