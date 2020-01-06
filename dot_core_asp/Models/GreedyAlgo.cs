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
        public IList<int> TotalTimesToCompleteATask = new List<int>();
        public List<int> Priorities = Enumerable.Range(1, 10).ToList<int>();
        public List<double> Ratios = new List<double>();

        private List<int> SortByRatio(out List<int>  prioritiesSorted)
        {
            var prioritiesSortedInt = new List<int>(TaskTimes.Count);
            var ratioTuple = new List<(double, int)>();
            var counter = 0;
            foreach (var ratio in Ratios)
            {
                ratioTuple.Add((ratio, counter));
                counter += 1;
            }
            ratioTuple.Sort(CompareUsingRatio);
            // Max first
            ratioTuple.Reverse();

            var sortIdxs = ratioTuple.Select(x => x.Item2);

            var sortedList = new List<int>(TaskTimes.Count);
            // var sortedList = new List<int>(TaskTimes.Count);
            foreach (var idx in sortIdxs)
            {
                sortedList.Add(TaskTimes[idx]);
                prioritiesSortedInt.Add(Priorities[idx]);
            }            
            Console.WriteLine(String.Join(";", sortedList));
            Console.WriteLine(String.Join(";", ratioTuple));

            prioritiesSorted = prioritiesSortedInt;
            return sortedList;
        }

        private int CompareUsingRatio((double, int) x, (double, int) y)
        {
            if (x.Item1 == y.Item1)
            {
                if (x.Item2 > y.Item2)
                    return 1;
                else if (x.Item2 < y.Item2)
                    return -1; // x is less and moved left
            }
            return x.Item1.CompareTo(y.Item1);
        }

        private int CompareUsingRatio(int x, int y)
        {
            var idxX = TaskTimes.IndexOf(x);
            var idxY = TaskTimes.IndexOf(y);

            // Compare elements in list            
            //Todo: is be prone to occurences with duplicates
            // Below only works if idxX and idxY are unique
            // They should be if the match is on address pointer instead of on value only.
            return Ratios[idxX].CompareTo(Ratios[idxY]);
        }

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

        /// <summary>
        /// Simple greedy algorithm.
        /// Calculate the maximum number of things you can accomplish in time T.
        /// </summary>
        public void ComputeMaxTasksCompleted(bool isSimpleSort=true)
        {
            // compareMaxExceptOneIntegerDel += CompareMaxExceptOneInteger;
            var timeT = 20;

            // var compareMethodInfo = RuntimeReflectionExtensions.GetMethodInfo(compareMaxExceptOneIntegerDel);
            // var del = compareMethodInfo.CreateDelegate(compareMethodInfo.DeclaringType);
            if (isSimpleSort)
                TaskTimes.Sort(CompareMaxExceptOneInteger);
            
            var completedTask = 0;
            var accumulatedTime = 0;
            foreach (var taskTime in TaskTimes)
            {
                if (accumulatedTime + taskTime <= timeT)
                {
                    accumulatedTime += taskTime;
                    TotalTimesToCompleteATask.Add(accumulatedTime);
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

        public void ComputeMaxPrioritizedTasksCompleted()
        {
            var timeT = 20;
            var sumTime = 0;
            var totalTaskTimes = new List<int>();

            // Prepare input
            Random rnd = new Random();
            Priorities = Priorities.Select(x => x = rnd.Next(1,11)).ToList<int>();
            // TaskTimes.Reverse();

            foreach (var taskTime in TaskTimes)
            {
                sumTime += taskTime;
                totalTaskTimes.Add(sumTime);
            }

            var ratios = RatioMinimizationObjectiveFun(TaskTimes, Priorities).ToList<double>();
            Ratios = ratios;

            // Todo: find bug the simple greedy wins over advanced ratio greed algo. Probably ref mistake with ratios.

            // Console.WriteLine("Compare with simple greedy algo sorted priorities.");
            // ComputeMaxTasksCompleted(isSimpleSort:false);
            // var ratiosCopy = Ratios.ToList<double>();
            // Ratios = Priorities.Select(x => (double)x).ToList<double>();
            // var sortedTaskTimes = SortByRatio(out Priorities);
            // TaskTimes = sortedTaskTimes;
            // Console.WriteLine(String.Format("Objective function value:{0}", PriorityObjectiveFun(TotalTimesToCompleteATask, Priorities)));

            Console.WriteLine("Compare with advanced greedy algo.");
            Ratios = ratios;
            TaskTimes = SortByRatio(out Priorities);
            ComputeMaxTasksCompleted(isSimpleSort:false);
            Console.WriteLine(String.Format("Objective function value:{0}", PriorityObjectiveFun(TotalTimesToCompleteATask, Priorities)));
        }

        public int PriorityObjectiveFun(IList<int> totalTimesToCompleteATask, IList<int> priorities)
        {
            var sum = 0;

            foreach (var ite in Enumerable.Range(0, totalTimesToCompleteATask.Count))
                sum += priorities[ite]*totalTimesToCompleteATask[ite];

            return sum;     
        }

        public IList<double> RatioMinimizationObjectiveFun(IList<int> timesToCompleteATask, IList<int> priorities)
        {
            var ratios = new List<double>();

            foreach (var ite in Enumerable.Range(0, priorities.Count))
                ratios.Add((double)timesToCompleteATask[ite]/priorities[ite]);
            return ratios;
        }

    }
}