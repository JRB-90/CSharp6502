using CS6502.Core;
using System;

namespace CS6502.UIConsole.Shared
{
    internal class CpuManager
    {
        const ushort RAM_START    = 0x0000;
        const ushort RAM_END      = 0x7FFF;
        const ushort ROM_START    = 0x8000;
        const ushort ROM_END      = 0xFFFF;
        const ushort VRAM_START   = 0x6000;
        const ushort VRAM_END     = 0x7FFF;

        public CpuManager()
        {
            cpu = new WD65C02();

            rom =
                new GenericROM(
                    (ushort.MaxValue / 2) + 1,
                    16,
                    8
                );

            ram =
                new GenericRAM(
                    (ushort.MaxValue / 2) + 1,
                    16,
                    8
                );

            decoder =
                new AddressDecoder(
                    new AddressSpace(ROM_START, ROM_END),
                    new AddressSpace(RAM_START, RAM_END)
                );

            clock = new ClockGenerator(ClockMode.FreeRunning);

            res_n = new Pin(TriState.True);
            irq_n = new Pin(TriState.True);
            nmi_n = new Pin(TriState.True);

            ConnectBusesAndWires();
        }

        public void LoadProgram(byte[] program)
        {
            lock (memoryLock)
            {
                rom.LoadData(program);
                programLoaded = true;
            }
        }

        public void Start()
        {
            if (!programLoaded)
            {
                throw new InvalidOperationException("No program loaded");
            }

            clock.Start();
        }

        public void Stop()
        {
            clock.Stop();
        }

        public void Cycle()
        {
            if (!programLoaded)
            {
                throw new InvalidOperationException("No program loaded");
            }

            lock (memoryLock)
            {
                clock.Cycle();
                halfCycleCount++;
            }
        }

        public CycleState GetCurrentCycleState()
        {
            lock (memoryLock)
            {
                return cpu.GetCurrentCycleState(halfCycleCount - 1);
            }
        }

        public byte[] GetVRAMCharData()
        {
            lock (memoryLock)
            {
                return GetGloballyAddressedBytes(VRAM_START, VRAM_END - VRAM_START);
            }
        }

        private void ConnectBusesAndWires()
        {
            // Hookup cpu inputs
            cpu.PHI2 = clock.CLK;
            cpu.RDY_N = clock.RDY_N;
            cpu.RES_N = new Wire(WirePull.PullUp);
            cpu.RES_N.ConnectPin(res_n);
            cpu.IRQ_N = new Wire(WirePull.PullUp);
            cpu.IRQ_N.ConnectPin(irq_n);
            cpu.NMI_N = new Wire(WirePull.PullUp);
            cpu.NMI_N.ConnectPin(nmi_n);

            // Hookup ROM
            rom.AddressBus = new Bus(cpu.AddressBus, 0, 15);
            rom.DataBus = cpu.DataBus;
            rom.CS_N = decoder.RomCS_N;
            rom.OE_N = decoder.RomOE_N;

            // Hookup RAM
            ram.AddressBus = new Bus(cpu.AddressBus, 0, 15);
            ram.DataBus = cpu.DataBus;
            ram.CS_N = decoder.RamCS_N;
            ram.OE_N = decoder.RamOE_N;
            ram.RW_N = cpu.RW_N;

            // Hookup decoder
            decoder.AddressBus = cpu.AddressBus;
            decoder.PHI2 = cpu.PHI2O;

            // Hookup clock
            clock.SYNC_N = cpu.SYNC_N;
        }

        private byte GetGloballyAddressedByte(ushort address)
        {
            lock (memoryLock)
            {
                if (address >= RAM_START &&
                address <= RAM_END)
                {
                    return ram.Data[address - RAM_START];
                }
                else
                {
                    return rom.Data[address - ROM_START];
                }
            }
        }

        private byte[] GetGloballyAddressedBytes(ushort start, ushort length)
        {
            lock (memoryLock)
            {
                byte[] bytes = new byte[length];

                if (start >= RAM_START &&
                    start <= RAM_END)
                {
                    Array.Copy(ram.Data, start - RAM_START, bytes, 0, length);
                }
                else
                {
                    Array.Copy(rom.Data, start - ROM_START, bytes, 0, length);
                }

                return bytes;
            }
        }

        private bool programLoaded;
        private WD65C02 cpu;
        private GenericROM rom;
        private GenericRAM ram;
        private AddressDecoder decoder;
        private ClockGenerator clock;
        private Pin res_n;
        private Pin irq_n;
        private Pin nmi_n;
        private int halfCycleCount;

        private static object memoryLock = new object();
    }
}
