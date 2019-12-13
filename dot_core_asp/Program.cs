using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using dot_core_asp.Models;

namespace dot_core_asp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // var dfs = new DFS();
            // dfs.TestDFS();
            // var bfs = new BFS();
            // bfs.TestBFS();

            var fibo = new Fibonacci();
            fibo.TestFibonacci();

            // DicesSumModel diceSumModel = new DicesSumModel();
            // diceSumModel.TestProbabilityDiceSum();

            // DateTimeModel dateTimeModel = new DateTimeModel();
            // dateTimeModel.TestMultipleDateTimeDiff();

            // Build webhost
            // BuildWebHost(args).Run();
        }

        // public static IWebHost BuildWebHost(string[] args) =>
        //     WebHost.CreateDefaultBuilder(args)
        //         .UseStartup<Startup>()
        //         .Build();
    }
}
