using System;

namespace CS6502.Core
{
    internal class LDA : InstructionBase
    {
        public static LDA CreateLDA(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new LDA(0xA9, addressingMode);

                case AddressingMode.ZeroPage:
                    return new LDA(0xA5, addressingMode);

                case AddressingMode.ZeroPageX:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xB5, addressingMode);

                case AddressingMode.Absolute:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xAD, addressingMode);

                case AddressingMode.AbsoluteX:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xBD, addressingMode);

                case AddressingMode.AbsoluteY:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xB9, addressingMode);

                case AddressingMode.IndirectX:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xA1, addressingMode);

                case AddressingMode.IndirectY:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xB1, addressingMode);

                default:
                    throw new ArgumentException($"LDA does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            if (AddressingMode == AddressingMode.Immediate)
            {
                return Immediate(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.ZeroPage ||
                     AddressingMode == AddressingMode.ZeroPageX)
            {
                return ZeroPage(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.Absolute ||
                     AddressingMode == AddressingMode.AbsoluteX ||
                     AddressingMode == AddressingMode.AbsoluteY)
            {
                return Absolute(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.IndirectX ||
                     AddressingMode == AddressingMode.IndirectY)
            {
                return Indirect(signalEdge, instructionCycle);
            }
            else
            {
                throw new ArgumentException($"LDA does not support {AddressingMode.ToString()} addressing mode");
            }
        }

        private CpuMicroCode Immediate(SignalEdge signalEdge, int instructionCycle)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                IsInstructionComplete = true;

                if (instructionCycle == 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDILIntoA,
                            MicroCodeInstruction.IncrementPC,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode ZeroPage(SignalEdge signalEdge, int instructionCycle)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == 2)
                {
                    return
                       new CpuMicroCode(
                           MicroCodeInstruction.TransferZPDataToAB,
                           MicroCodeInstruction.LatchDataIntoDIL,
                           MicroCodeInstruction.IncrementPC
                       );
                }
                if (instructionCycle == 3)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDILIntoA
                        );
                }
            }
            else
            {
                if (instructionCycle == 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode Absolute(SignalEdge signalEdge, int instructionCycle)
        {
            throw new NotImplementedException();
        }

        private CpuMicroCode Indirect(SignalEdge signalEdge, int instructionCycle)
        {
            throw new NotImplementedException();
        }

        private LDA(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "LDA",
                opcode,
                addressingMode)
        {
        }
    }
}
