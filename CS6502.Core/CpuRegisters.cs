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
            P = new StatusRegister(0x02);
            A = 0x00;
            X = 0xC0;
            Y = 0x00;
            SP = 0xC0;
            PC = 0x00FF;
            InputDataLatch = 0x00;
            DataBusBuffer = 0x00;
            IR = InstructionDecoder.DecodeOpcode(0x00);
        }

        public byte A { get; private set; }

        public byte X { get; private set; }

        public byte Y { get; private set; }

        public byte SP { get; private set; }

        public IInstruction IR { get; private set; }

        public ushort PC { get; private set; }

        public StatusRegister P { get; private set; }

        public byte InputDataLatch { get; private set; }

        public byte DataBusBuffer { get; private set; }

        public void LoadA(byte value)
        {
            A = value;
        }

        public void LoadX(byte value)
        {
            X = value;
        }

        public void LoadY(byte value)
        {
            Y = value;
        }

        public void TransferAtoX()
        {
            X = A;
        }

        public void TransferAtoY()
        {
            Y = A;
        }

        public void TransferXtoA()
        {
            A = X; ;
        }

        public void TransferYtoA()
        {
            A = Y;
        }

        public void DecodeOpcode()
        {
            IR = InstructionDecoder.DecodeOpcode(DataBusBuffer);
        }

        public void IncrementStackPointer()
        {
            SP++;
        }

        public void DecrementStackPointer()
        {
            SP--;
        }

        public void SetStackPointer(byte value)
        {
            SP = value;
        }

        public void SetProgramCounter(ushort value)
        {
            PC = value;
        }

        public void IncrementProgramCounter()
        {
            PC++;
        }

        public void ClearCarry()
        {
            P.CarryFlag = false;
        }

        public void SetCarry()
        {
            P.CarryFlag = true;
        }

        public void ClearIRQ()
        {
            P.IrqFlag = false;
        }

        public void SetIRQ()
        {
            P.IrqFlag = true;
        }

        public void ClearDecimal()
        {
            P.DecimalFlag = false;
        }

        public void SetDecimal()
        {
            P.DecimalFlag = true;
        }

        public void ClearBRK()
        {
            P.BrkFlag = false;
        }

        public void SetBRK()
        {
            P.BrkFlag = true;
        }

        public void ClearOverflow()
        {
            P.OverflowFlag = false;
        }

        public void SetOverflow()
        {
            P.OverflowFlag = true;
        }

        public void LatchInputData(byte inputDataValue)
        {
            InputDataLatch = inputDataValue;
            UpdateP();
        }

        public void LatchDataBus(byte dataBusValue)
        {
            DataBusBuffer = dataBusValue;
        }

        private void UpdateP()
        {
            P.ZeroFlag = InputDataLatch == 0x00;
            P.NegativeFlag = (InputDataLatch & 0b10000000) > 0;
        }

        private byte a;
    }
}
