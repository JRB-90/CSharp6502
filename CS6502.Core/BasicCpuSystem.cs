using System;

namespace CS6502.Core
{
    /// <summary>
    /// Class to handle setup, wiring and interaction with a basic 6502 system.
    /// System is memory mapped as below:
    /// 0x0000 -> 0x7FFF 32K RAM
    /// 0x8000 -> 0xFFFF 32K ROM
    /// All loaded programs will be stored in the 32K ROM and so need to created
    /// with the first byte of the program being at address 0x8000.
    /// </summary>
    public class BasicCpuSystem
    {
        const uint RAM_START    = 0x0000;
        const uint RAM_END      = 0x7FFF;
        const uint ROM_START    = 0x8000;
        const uint ROM_END      = 0xFFFF;

        public BasicCpuSystem(string path)
        {
            cpu = new WD65C02();

            rom = 
                new GenericROM(
                    path,
                    16,
                    8
                );

            ram =
                new GenericRAM(
                    ushort.MaxValue / 2,
                    16,
                    8
                );

            decoder = 
                new AddressDecoder(
                    new AddressSpace(ROM_START, ROM_END),
                    new AddressSpace(RAM_START, RAM_END)
                );

            clock = new ClockGenerator(ClockMode.StepHalfCycle);

            res_n = new Pin(TriState.True);
            irq_n = new Pin(TriState.True);
            nmi_n = new Pin(TriState.True);

            ConnectBusesAndWires();
        }

        public void Cycle(bool printState = false)
        {
            clock.Cycle();

            if (printState)
            {
                string stateStr = cpu.GetCurrentStateString('\t');
                System.Console.WriteLine($"{halfCycleCount}\t{stateStr}");
            }

            halfCycleCount++;
        }

        public CycleState GetCurrentCycleState()
        {
            return cpu.GetCurrentCycleState(halfCycleCount - 1);
        }

        public byte[] GetCurrentMemoryState(AddressSpace addressSpace)
        {
            int length = (int)addressSpace.EndAddress - (int)addressSpace.StartAddress + 1;
            byte[] memory = new byte[length];

            for (int i = 0; i < memory.Length; i++)
            {
                memory[i] = GetGloballyAddressedByte((ushort)(addressSpace.StartAddress + i));
            }

            return memory;
        }

        private byte GetGloballyAddressedByte(ushort address)
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

        private WD65C02 cpu;
        private GenericROM rom;
        private GenericRAM ram;
        private AddressDecoder decoder;
        private ClockGenerator clock;
        private Pin res_n;
        private Pin irq_n;
        private Pin nmi_n;
        private int halfCycleCount;
    }
}
