using CS6502.Core;

namespace CS6502.UIConsole.Models
{
    internal class SystemModel
    {
        readonly ClockGenerator clock;
        readonly MemoryModel memory;
        readonly AddressDecoder decoder;

        public SystemModel(
            ClockGenerator clock,
            MemoryModel memory,
            AddressDecoder decoder)
        {
            this.clock = clock;
            this.memory = memory;
            this.decoder = decoder;

            cpu = new WD65C02();

            res_n = new Pin(TriState.True);
            irq_n = new Pin(TriState.True);
            nmi_n = new Pin(TriState.True);

            ConnectBusesAndWires();
        }

        public CycleState GetCurrentCycleState(int halfCycleCount)
        {
            return cpu.GetCurrentCycleState(halfCycleCount);
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
            memory.ROM.AddressBus = new Bus(cpu.AddressBus, 0, 15);
            memory.ROM.DataBus = cpu.DataBus;
            memory.ROM.CS_N = decoder.RomCS_N;
            memory.ROM.OE_N = decoder.RomOE_N;

            // Hookup RAM
            memory.RAM.AddressBus = new Bus(cpu.AddressBus, 0, 15);
            memory.RAM.DataBus = cpu.DataBus;
            memory.RAM.CS_N = decoder.RamCS_N;
            memory.RAM.OE_N = decoder.RamOE_N;
            memory.RAM.RW_N = cpu.RW_N;

            // Hookup decoder
            decoder.AddressBus = cpu.AddressBus;
            decoder.PHI2 = cpu.PHI2O;

            // Hookup clock
            clock.SYNC_N = cpu.SYNC_N;
        }

        private WD65C02 cpu;
        private Pin res_n;
        private Pin irq_n;
        private Pin nmi_n;
    }
}
