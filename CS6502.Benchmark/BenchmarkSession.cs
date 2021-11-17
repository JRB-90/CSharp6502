using CS6502.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace CS6502.Benchmark
{
    /// <summary>
    /// Class to load a benchmark file and orchestrate a session.
    /// Does a comparison for the CPU output and the benchmark and
    /// flags any discrepencies.
    /// </summary>
    internal class BenchmarkSession
    {
        public BenchmarkSession()
        {
            cycleStates = new List<CycleState>();
        }

        public void LoadFIle(string path)
        {
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                if (line.StartsWith("Half Cycle"))
                {
                    continue;
                }
                cycleStates.Add(new CycleState(line));
            }
        }

        public void Run(string path)
        {
            BasicCpuSystem system = new BasicCpuSystem(path);

            for (int i = 0; i < cycleStates.Count; i++)
            {
                system.Cycle();

                ComparisonResult result =
                    cycleStates[i].Compare(system.GetCurrentCycleState());

                if (!result.IsCompleteMatch)
                {
                    System.Console.WriteLine(result.ToString());
                }
            }
        }

        List<CycleState> cycleStates;
    }
}
