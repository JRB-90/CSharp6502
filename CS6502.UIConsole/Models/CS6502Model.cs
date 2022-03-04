using CS6502.Core;
using System;
using System.IO;
using System.Reactive.Subjects;

namespace CS6502.UIConsole.Models
{
    internal class CS6502Model
    {
        const ushort RAM_START = 0x0000;
        const ushort RAM_END = 0x7FFF;
        const ushort ROM_START = 0x8000;
        const ushort ROM_END = 0xFFFF;
        const ushort VRAM_START = 0x6000;
        const ushort VRAM_END = 0x7FFF;

        public CS6502Model()
        {
            targetFrequency = -1;
            runFrequency = new Subject<int>();
            cycleState = new Subject<CycleState>();
            consoleChars = new Subject<byte[]>();
            Init();
        }

        public bool IsRunning
        {
            get => isRunning;
            set
            {
                isRunning = value;
                RunStateChanged?.Invoke(this, isRunning);

                if (isRunning)
                {
                    previousTime = DateTime.Now.Ticks;
                }
                else
                {
                    runFrequency.OnNext(-1);
                }
            }
        }

        public bool IsProgramLoaded
        {
            get => isProgramLoaded;
            set
            {
                isProgramLoaded = value;
                ProgramLoadedChanged?.Invoke(this, isProgramLoaded);
            }
        }

        public IObservable<int> RunFrequency => runFrequency;

        public IObservable<CycleState> CycleState => cycleState;

        public IObservable<byte[]> ConsoleChars => consoleChars;

        public event EventHandler<bool> RunStateChanged;

        public event EventHandler<bool> ProgramLoadedChanged;

        public void LoadProgram(string path)
        {
            if (!isRunning)
            {
                memory.LoadProgram(File.ReadAllBytes(path));
                IsProgramLoaded = memory.IsProgramLoaded;
            }
        }

        public void Start()
        {
            if (!memory.IsProgramLoaded)
            {
                throw new InvalidOperationException("No program loaded");
            }

            clock.Start();
            IsRunning = clock.IsRunning;
        }

        public void Stop()
        {
            clock.Stop();
            IsRunning = clock.IsRunning;
        }

        public void Reset()
        {
            Stop();
            Init();
        }

        public void SetTargetFrequency(int targetFrequency)
        {
            clock.TargetFrequency = targetFrequency;
            this.targetFrequency = targetFrequency;
        }

        public byte[] GetMemoryBlock(AddressSpace addressSpace)
        {
            return memory.GetMemoryBlock(addressSpace);
        }

        private void Init()
        {
            halfCycleCount = 0;
            clock = new ClockGenerator(ClockMode.FreeRunning);
            clock.TargetFrequency = targetFrequency;
            clock.ClockTicked += Clock_ClockTicked;

            memory =
                new MemoryModel(
                    RAM_START, RAM_END,
                    ROM_START, ROM_END,
                    VRAM_START, VRAM_END
                );

            decoder =
                new AddressDecoder(
                    new AddressSpace(ROM_START, ROM_END),
                    new AddressSpace(RAM_START, RAM_END)
                );

            system =
                new SystemModel(
                    clock,
                    memory,
                    decoder
                );

            IsRunning = clock.IsRunning;
            IsProgramLoaded = memory.IsProgramLoaded;
            runFrequency.OnNext(-1);
            cycleState.OnNext(system.GetCurrentCycleState(halfCycleCount));
            consoleChars.OnNext(memory.GetVRAMCharData());
        }

        private int CalculateFrequency()
        {
            var currentTime = DateTime.Now.Ticks;
            var ticksPassed = currentTime - previousTime;
            var secondsPassed = ((double)ticksPassed / (double)TimeSpan.TicksPerSecond);
            previousTime = currentTime;

            return (int)(1.0 / secondsPassed);
        }

        private void Clock_ClockTicked(object? sender, EventArgs e)
        {
            cycleState.OnNext(system.GetCurrentCycleState(halfCycleCount));
            consoleChars.OnNext(memory.GetVRAMCharData());
            runFrequency.OnNext(CalculateFrequency());
            halfCycleCount++;
        }

        private int halfCycleCount;
        private int targetFrequency;
        private long previousTime;
        private bool isRunning;
        private bool isProgramLoaded;
        private Subject<int> runFrequency;
        private Subject<CycleState> cycleState;
        private Subject<byte[]> consoleChars;
        private SystemModel system;
        private ClockGenerator clock;
        private MemoryModel memory;
        private AddressDecoder decoder;
    }
}
