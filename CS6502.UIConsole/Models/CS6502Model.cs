using CS6502.Core;
using System;
using System.IO;
using System.Reactive.Subjects;

namespace CS6502.UIConsole.Models
{
    internal class CS6502Model
    {
        const string PROG_PATH = @"C:\Development\Sim6502\asm\consoleasm\consoleTests.bin";
        const ushort RAM_START = 0x0000;
        const ushort RAM_END = 0x7FFF;
        const ushort ROM_START = 0x8000;
        const ushort ROM_END = 0xFFFF;
        const ushort VRAM_START = 0x6000;
        const ushort VRAM_END = 0x7FFF;

        public CS6502Model()
        {
            isRunning = new Subject<bool>();
            cycleState = new Subject<CycleState>();
            consoleChars = new Subject<byte[]>();
            Init();
        }

        public IObservable<bool> IsRunning => isRunning;

        public IObservable<CycleState> CycleState => cycleState;

        public IObservable<byte[]> ConsoleChars => consoleChars;

        public void Start()
        {
            if (!memory.IsProgramLoaded)
            {
                throw new InvalidOperationException("No program loaded");
            }

            clock.Start();
            isRunning.OnNext(clock.IsRunning);
        }

        public void Stop()
        {
            clock.Stop();
            isRunning.OnNext(clock.IsRunning);
        }

        public void Reset()
        {
            Stop();
            Init();
        }

        private void Init()
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
            
            isRunning.OnNext(false);
            cycleState.OnNext(system.GetCurrentCycleState(halfCycleCount));
            consoleChars.OnNext(memory.GetVRAMCharData());
        }

        private void Clock_ClockTicked(object? sender, EventArgs e)
        {
            cycleState.OnNext(system.GetCurrentCycleState(halfCycleCount));
            consoleChars.OnNext(memory.GetVRAMCharData());
            halfCycleCount++;
        }

        private int halfCycleCount;
        private Subject<bool> isRunning;
        private Subject<CycleState> cycleState;
        private Subject<byte[]> consoleChars;
        private SystemModel system;
        private ClockGenerator clock;
        private MemoryModel memory;
        private AddressDecoder decoder;
    }
}
