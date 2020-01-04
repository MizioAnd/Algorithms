using System;
using System.Collections.Generic;
using System.Linq;

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
        private static int CompareMax(int x, int y)
        {
            return x.CompareTo(y);
        }

        // Calculate the maximum number of things you can accomplish in time T.
        public void ComputeMaxTasksCompleted()
        {
            var timeT = 20;
            TaskTimes.Sort(CompareMax);
            
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