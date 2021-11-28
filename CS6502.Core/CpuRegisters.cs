using System.Collections.Generic;

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
            aluResultQueue = new Queue<(InternalRegister, byte)>();
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

        public void PollInternalAluQueue()
        {
            if (aluResultQueue.Count > 0)
            {
                var result = aluResultQueue.Dequeue();
                if (result.Item1 == InternalRegister.X)
                {
                    X = result.Item2;
                }
                else if (result.Item1 == InternalRegister.Y)
                {
                    Y = result.Item2;
                }
            }
        }

        public void LoadA()
        {
            A = DataBusBuffer;
            UpdateP(A);
        }

        public void LoadX()
        {
            X = DataBusBuffer;
            UpdateP(X);
        }

        public void LoadY()
        {
            Y = DataBusBuffer;
            UpdateP(Y);
        }

        public void TransferAtoX()
        {
            X = A;
            // TODO - Latch here?
        }

        public void TransferAtoY()
        {
            Y = A;
            // TODO - Latch here?
        }

        public void TransferXtoA()
        {
            A = X;
            // TODO - Latch here?
        }

        public void TransferYtoA()
        {
            A = Y;
            // TODO - Latch here?
        }

        public void IncrementX()
        {
            aluResultQueue.Enqueue((InternalRegister.X, (byte)(X + 1)));
        }

        public void DecrementX()
        {
            aluResultQueue.Enqueue((InternalRegister.X, (byte)(X - 1)));
        }

        public void IncrementY()
        {
            aluResultQueue.Enqueue((InternalRegister.Y, (byte)(Y + 1)));
        }

        public void DecrementY()
        {
            aluResultQueue.Enqueue((InternalRegister.Y, (byte)(Y - 1)));
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

        public void TransferAToDataBus()
        {
            DataBusBuffer = A;
        }

        public void TransferXToDataBus()
        {
            DataBusBuffer = X;
        }

        public void TransferYToDataBus()
        {
            DataBusBuffer = Y;
        }

        public void LatchDataBus(byte dataBusValue)
        {
            DataBusBuffer = dataBusValue;
        }

        public void LatchInputDataBusBuffer()
        {
            InputDataLatch = DataBusBuffer;
        }

        private void UpdateP(byte value)
        {
            P.ZeroFlag = value == 0x00;
            P.NegativeFlag = (value & 0b10000000) > 0;
        }

        private Queue<(InternalRegister, byte)> aluResultQueue; 
    }
}
