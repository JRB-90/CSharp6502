using System;

namespace CS6502.Core
{
    internal class STA : InstructionBase
    {
        public static STA CreateSTA(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new STA(0x85, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new STA(0x95, addressingMode);

                case AddressingMode.Absolute:
                    throw new NotImplementedException(); // TODO
                    //return new STA(0x8D, addressingMode);

                case AddressingMode.AbsoluteX:
                    throw new NotImplementedException(); // TODO
                    //return new STA(0x9D, addressingMode);

                case AddressingMode.AbsoluteY:
                    throw new NotImplementedException(); // TODO
                    //return new STA(0x99, addressingMode);

                case AddressingMode.IndirectX:
                    throw new NotImplementedException(); // TODO
                    //return new STA(0x81, addressingMode);

                case AddressingMode.IndirectY:
                    throw new NotImplementedException(); // TODO
                    //return new STA(0x91, addressingMode);

                default:
                    throw new ArgumentException($"STA does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            if (AddressingMode == AddressingMode.ZeroPage ||
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
                throw new ArgumentException($"STA does not support {AddressingMode.ToString()} addressing mode");
            }
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
                           MicroCodeInstruction.SetToWrite,
                           MicroCodeInstruction.IncrementPC
                       );
                }
                if (instructionCycle == 3)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }    
            }
            else
            {
                if (instructionCycle == 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchAIntoData,
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

        private STA(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "STA",
                opcode,
                addressingMode,
                OperationType.Write)
        {
        }
    }
}
