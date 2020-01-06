using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dot_core_asp.Models
{
    /// <summary>
    /// A greedy algo is an algorithm that make local choices that optimize a problem. It has only one shot a doing this and the hope is then
    /// that this also holds as a globally optimized solution. But this cannot not always be ensured.
    /// </summary>
    public class GreedyAlgo
    {
        // Time that a task takes to complete.
        public List<int> TaskTimes = Enumerable.Range(1, 10).ToList<int>();

        // 
        private static int CompareMaxExceptOneInteger(int x, int y, int exceptedInt=5)
        {
            // Always move 5 to left in sorting and make it starting element.
            if (x == exceptedInt)
                return -1; // y is greater
            if (y == exceptedInt)
                return 1; // x is greater

            return x.CompareTo(y);
        }

        private static int CompareMaxExceptOneInteger(int x, int y)
        {
            int exceptedInt=5;
            return CompareMaxExceptOneInteger(x, y, exceptedInt);
            
            // Casts StackOverflow exception
            // return CompareMaxExceptOneInteger(x, y);
        }

        // public delegate int CompareMaxExceptOneIntegerDel<in T>(T x, T y); 
        // CompareMaxExceptOneIntegerDel<int> compareMaxExceptOneIntegerDel;

        // Calculate the maximum number of things you can accomplish in time T.
        public void ComputeMaxTasksCompleted()
        {
            // compareMaxExceptOneIntegerDel += CompareMaxExceptOneInteger;
            var timeT = 20;

            // var compareMethodInfo = RuntimeReflectionExtensions.GetMethodInfo(compareMaxExceptOneIntegerDel);
            // var del = compareMethodInfo.CreateDelegate(compareMethodInfo.DeclaringType);
            TaskTimes.Sort(CompareMaxExceptOneInteger);
            
            var completedTask = 0;
            var accumulatedTime = 0;
            foreach (var taskTime in TaskTimes)
            {
                if (accumulatedTime + taskTime <= timeT)
                {
                    accumulatedTime += taskTime;
                    Console.WriteLine(accumulatedTime);
                    completedTask += 1;
                }
                else
                {
                    Console.WriteLine(String.Format("Total number of completed task:{0}", completedTask));
                    break;
                }
            }

        }
    }
}