using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dot_core_asp.Models
{
    /// <summary>
    /// A greedy algo is an algorithm that make local choices that optimize a problem. It has only one shot a doing this and the hope is then
    /// that this also holds as a globally optimized solution. But this cannot not always be ensured.
    /// The completion for at task i is T[i] where task coming before are those that conveniently should be completed first eith due to priority or some other param.
    /// C(1) = T[1] = 1
    /// C(2) = T[1] + T[2] = 1 + 2 = 3
    /// C(3) = T[1] + T[2] + T[3] = 1 + 2 + 3 = 6
    /// Objective function needs to be minimized and in order to get the best result a sorting param that combines both priority and completeness times for individual task.
    /// F = P[1] * C(1) + P[2] * C(2) + ...... + P[N] * C(N)
    /// ex.
    /// T = {5, 2} and P = {3, 1}
    ///  ( P[1] / T[1] ) > ( P[2] / T[2] ) as sort param,
    /// F = P[1] * C(1) + P[2] * C(2) = 3 * 5 + 1 * 7 = 22
    /// </summary>
    public class GreedyAlgo
    {
        // Time that a task takes to complete.
        public List<int> TaskTimes = Enumerable.Range(1, 10).ToList<int>();
        public int TaskTimesSum 
        { 
            get { return TaskTimes.Sum(); } 
        }
        public IList<int> TotalTimesToCompleteATask = new List<int>();
        public IList<int> PriorityObjRes = new List<int>();
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
            // Max first, since highest ratio is most important
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
        public void ComputeMaxTasksCompleted()
        {
            // compareMaxExceptOneIntegerDel += CompareMaxExceptOneInteger;
            var timeT = TaskTimesSum / 2;

            // var compareMethodInfo = RuntimeReflectionExtensions.GetMethodInfo(compareMaxExceptOneIntegerDel);
            // var del = compareMethodInfo.CreateDelegate(compareMethodInfo.DeclaringType);
            
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
            var sumTime = 0;
            var totalTaskTimes = new List<int>();

            // Prepare input
            Random rnd = new Random();
            Priorities = Priorities.Select(x => x = rnd.Next(1,11)).ToList<int>();

            var ratios = RatioMinimizationObjectiveFun(TaskTimes, Priorities).ToList<double>();
            // Ratios = ratios;
            var taskTimes = TaskTimes.ToList<int>();

            Console.WriteLine("Compare with simple greedy algo.");
            var ratiosSimple = Priorities.Select(x => (double)x).ToList<double>();
            GreedyModelResult(ratiosSimple, taskTimes);

            Console.WriteLine("Compare with advanced greedy algo.");
            TotalTimesToCompleteATask = new List<int>();
            GreedyModelResult(ratios, taskTimes);

            Console.WriteLine("Is simple greedy {0} worse than adv greedy {1}:{2}", PriorityObjRes[0], PriorityObjRes[1], PriorityObjRes[0] > PriorityObjRes[1]);
        }

        public void GreedyModelResult(List<double> ratios, List<int> times)
        {
            Ratios = ratios;
            TaskTimes = times;
            var prioritiesView = new List<int>();
            TaskTimes = SortByRatio(out prioritiesView);
            ComputeMaxTasksCompleted();
            var priorityObjRes = PriorityObjectiveFun(TotalTimesToCompleteATask, prioritiesView);
            PriorityObjRes.Add(priorityObjRes);
            Console.WriteLine(String.Format("Objective function value:{0}", priorityObjRes));
        }

        /// <summary>
        /// For all the tasks completed a sum is computed with each of the task's priority times the total time for that task to complete,
        /// which is the sum of times of previous completed tasks and with time of the task itself. Which in case all long task times get completed first,
        /// then every task after those will get a very high total time to complete value, which is far from optimal according to this metric
        /// </summary>
        /// <param name="totalTimesToCompleteATask"></param>
        /// <param name="priorities"></param>
        /// <returns></returns>
        public int PriorityObjectiveFun(IList<int> totalTimesToCompleteATask, IList<int> priorities)
        {
            var sum = 0;

            foreach (var ite in Enumerable.Range(0, totalTimesToCompleteATask.Count))
                sum += priorities[ite]*totalTimesToCompleteATask[ite];

            return sum;     
        }

        /// <summary>
        /// In order to minimize the objective function which is a measure for how good a greedy algorithm is.
        /// In below the optimized way of sorting task is according to each task's priority/(time to complete a task)
        /// Priority is incrementing such that a task with high priority like 10 and little time to complete gets a large ratio
        /// compared to a task of low priority 1 and same time to complete
        /// </summary>
        /// <param name="timesToCompleteATask"></param>
        /// <param name="priorities"></param>
        /// <returns></returns>
        public IList<double> RatioMinimizationObjectiveFun(IList<int> timesToCompleteATask, IList<int> priorities)
        {
            var ratios = new List<double>();

            foreach (var ite in Enumerable.Range(0, priorities.Count))
                ratios.Add((double)priorities[ite]/timesToCompleteATask[ite]);
            return ratios;
        }

    }
}