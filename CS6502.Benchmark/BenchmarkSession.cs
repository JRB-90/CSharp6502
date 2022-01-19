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

        public void LoadFileFromPath(string path)
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

        public void LoadFileFromString(string file)
        {
            string[] lines = file.Split("\r\n");

            foreach (string line in lines)
            {
                if (line == "" ||
                    line.StartsWith("Half Cycle"))
                {
                    continue;
                }
                cycleStates.Add(new CycleState(line));
            }
        }

        public void LoadFromP6502(string path, int cyclesToRun)
        {
            int res = P6502.SetupChipAndMemory(path);
            if (res < 0)
            {
                throw new InvalidOperationException("Failed to initialise CPU and Memory");
            }

            for (int i = 0; i < cyclesToRun; i++)
            {
                res = P6502.StepCpu();
                if (res < 0)
                {
                    throw new InvalidOperationException("Failed to step CPU");
                }

                cycleStates.Add(P6502.GetCurrentState(i));
            }

            res = P6502.DestroyChipAndMemory();
            if (res < 0)
            {
                throw new InvalidOperationException("Failed to destroy CPU and Memory");
            }
        }

        public void Run(string path, int startingOffset = 0)
        {
            Run(MemoryTools.LoadDataFromFile(path));
        }

        public void Run(byte[] data, int startingOffset = 0)
        {
            BasicCpuSystem system = new BasicCpuSystem(data);

            for (int i = startingOffset; i < cycleStates.Count; i++)
            {
                system.Cycle();

                ComparisonResult result =
                    cycleStates[i].Compare(
                        system.GetCurrentCycleState(),
                        startingOffset
                    );

                if (!result.IsCompleteMatch)
                {
                    System.Console.WriteLine(result.ToString());
                }
            }
        }

        List<CycleState> cycleStates;
    }
}
