using System;

namespace CS6502.Core
{
    internal class AND : InstructionBase
    {
        public static AND CreateAND(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new AND(0x29, addressingMode);

                case AddressingMode.ZeroPage:
                    return new AND(0x25, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new AND(0x35, addressingMode);

                case AddressingMode.Absolute:
                    return new AND(0x2D, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new AND(0x3D, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new AND(0x39, addressingMode);

                case AddressingMode.XIndirect:
                    return new AND(0x21, addressingMode);

                case AddressingMode.IndirectY:
                    return new AND(0x31, addressingMode);

                default:
                    throw new ArgumentException($"AND does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status,
            bool wasPageBoundaryCrossed)
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
            else if (AddressingMode == AddressingMode.XIndirect ||
                     AddressingMode == AddressingMode.IndirectY)
            {
                return Indirect(signalEdge, instructionCycle);
            }
            else
            {
                throw new ArgumentException($"AND does not support {AddressingMode.ToString()} addressing mode");
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
                            MicroCodeInstruction.AND,
                            MicroCodeInstruction.IncrementPC,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode ZeroPage(SignalEdge signalEdge, int instructionCycle)
        {
            int startingCycle = 2;
            if (AddressingMode == AddressingMode.ZeroPageX)
            {
                startingCycle = 3;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle)
                {
                    if (AddressingMode == AddressingMode.ZeroPage)
                    {
                        return
                            new CpuMicroCode(
                               MicroCodeInstruction.TransferZPDataToAB,
                               MicroCodeInstruction.LatchDataIntoDIL,
                               MicroCodeInstruction.SetToRead,
                               MicroCodeInstruction.IncrementPC
                            );
                    }
                    else if (AddressingMode == AddressingMode.ZeroPageX)
                    {
                        return
                            new CpuMicroCode(
                               MicroCodeInstruction.LatchDataIntoDIL,
                               MicroCodeInstruction.SetToRead,
                               MicroCodeInstruction.IncrementABByX_NoCarry
                            );
                    }
                }
                if (instructionCycle == startingCycle + 1)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.AND,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }
            else
            {
                if (instructionCycle == startingCycle)
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
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.SetToRead,
                            MicroCodeInstruction.TransferDILToPCHS,
                            MicroCodeInstruction.TransferPCSToAddressBus,
                            MicroCodeInstruction.IncrementPC
                        );
                }
                else if (instructionCycle == 4)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.AND,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }
            else
            {
                if (instructionCycle == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDataIntoDIL
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode Indirect(SignalEdge signalEdge, int instructionCycle)
        {
            int startingCycle = 4;
            if (AddressingMode == AddressingMode.XIndirect ||
                AddressingMode == AddressingMode.IndirectY)
            {
                startingCycle = 5;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (AddressingMode == AddressingMode.XIndirect)
                {
                    if (instructionCycle == startingCycle)
                    {
                        if (AddressingMode == AddressingMode.XIndirect)
                        {
                            return
                                new CpuMicroCode(
                                    MicroCodeInstruction.SetToRead,
                                    MicroCodeInstruction.TransferDILToPCHS,
                                    MicroCodeInstruction.TransferPCSToAddressBus
                                );
                        }
                    }
                    else if (instructionCycle == startingCycle + 1)
                    {
                        IsInstructionComplete = true;

                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.AND,
                                MicroCodeInstruction.TransferPCToPCS
                            );
                    }
                }
                else if (AddressingMode == AddressingMode.IndirectY)
                {
                    if (instructionCycle == startingCycle)
                    {
                        IsInstructionComplete = true;

                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.AND,
                                MicroCodeInstruction.TransferPCToPCS
                            );
                    }
                }
            }
            else
            {
                if (instructionCycle == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDataIntoDIL
                        );
                }
            }

            return new CpuMicroCode();
        }

        private AND(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "AND",
                opcode,
                addressingMode)
        {
        }
    }
}
