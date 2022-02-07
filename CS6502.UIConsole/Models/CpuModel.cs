using CS6502.Core;
using CS6502.UIConsole.Shared;
using System;
using System.IO;
using System.Reactive.Subjects;

namespace CS6502.UIConsole.Models
{
    internal class CpuModel
    {
        const string PROG_PATH = @"C:\Development\Sim6502\asm\consoleasm\consoleTests.bin";
        const ushort RAM_START = 0x0000;
        const ushort RAM_END = 0x7FFF;
        const ushort ROM_START = 0x8000;
        const ushort ROM_END = 0xFFFF;
        const ushort VRAM_START = 0x6000;
        const ushort VRAM_END = 0x7FFF;

        public CpuModel()
        {
            halfCycleCount = 0;
            clock = new ClockGenerator(ClockMode.FreeRunning);
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
            
            // TODO - Temp
            memory.LoadProgram(File.ReadAllBytes(PROG_PATH));
            cycleState = new Subject<CycleState>();
            cycleState.OnNext(system.GetCurrentCycleState(halfCycleCount));
            consoleChars = new Subject<byte[]>();
        }

        public bool IsRunning
        {
            get => isRunning;
            set
            {
                isRunning = value;
                RunStateChanged?.Invoke(this, new EventArgs());
            }
        }

        public IObservable<CycleState> CycleState => cycleState;

        public IObservable<byte[]> ConsoleChars => consoleChars;

        public event EventHandler RunStateChanged;

        public void Start()
        {
            if (!memory.IsProgramLoaded)
            {
                throw new InvalidOperationException("No program loaded");
            }

            clock.Start();
        }

        public void Stop()
        {
            clock.Stop();
        }

        public void Reset()
        {
            Stop();

            system =
                new SystemModel(
                    clock,
                    memory,
                    decoder
                );

            halfCycleCount = 0;
            memory.LoadProgram(File.ReadAllBytes(PROG_PATH));
            cycleState.OnNext(system.GetCurrentCycleState(halfCycleCount));
            consoleChars.OnNext(memory.GetVRAMCharData());
        }

        private void Clock_ClockTicked(object? sender, EventArgs e)
        {
            cycleState.OnNext(system.GetCurrentCycleState(halfCycleCount));
            consoleChars.OnNext(memory.GetVRAMCharData());
            halfCycleCount++;
        }

        private bool isRunning;
        private int halfCycleCount;
        private Subject<CycleState> cycleState;
        private Subject<byte[]> consoleChars;
        private SystemModel system;
        private ClockGenerator clock;
        private MemoryModel memory;
        private AddressDecoder decoder;
    }
}
