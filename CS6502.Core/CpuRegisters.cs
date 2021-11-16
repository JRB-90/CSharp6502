namespace CS6502.Core
{
    /// <summary>
    /// Represents the state of the internal CPU registers.
    /// Responsible for conducting register transfers on clocks.
    /// </summary>
    public class CpuRegisters
    {
        public CpuRegisters()
        {
            A = 0x00;
            X = 0xC0;
            Y = 0x00;
            SP = 0xC0;
            PC = 0x00FF;
            P = new StatusRegister(0x02);
            InputDataLatch = 0x00;
            DataBusBuffer = 0x00;
            IR = InstructionDecoder.DecodeOpcode(0x00);
        }

        public byte A { get; private set; }

        public byte X { get; private set; }

        public byte Y { get; private set; }

        public byte SP { get; private set; }

        public ushort PC { get; private set; }

        public StatusRegister P { get; private set; }

        public byte InputDataLatch { get; private set; }

        public byte DataBusBuffer { get; private set; }

        public IInstruction IR { get; private set; }

        public void SetProgramCounter(ushort value)
        {
            PC = value;
        }

        public void IncrementProgramCounter()
        {
            PC++;
        }

        public void LatchDataBus(byte dataBusValue)
        {
            DataBusBuffer = dataBusValue;
        }

        public void DecodeOpcode()
        {
            IR = InstructionDecoder.DecodeOpcode(DataBusBuffer);
        }
    }
}
