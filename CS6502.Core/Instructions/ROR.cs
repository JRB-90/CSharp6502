using System;

namespace CS6502.Core
{
    internal class ROR : InstructionBase
    {
        public static ROR CreateROR(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new ROR(0x6A, addressingMode);

                case AddressingMode.ZeroPage:
                    return new ROR(0x66, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new ROR(0x76, addressingMode);

                case AddressingMode.Absolute:
                    return new ROR(0x6E, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new ROR(0x7E, addressingMode);

                default:
                    throw new ArgumentException($"ROR does not support {addressingMode.ToString()} addressing mode");
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
            if (AddressingMode == AddressingMode.ZeroPage ||
                AddressingMode == AddressingMode.ZeroPageX)
            {
                return ZeroPage(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.Absolute ||
                     AddressingMode == AddressingMode.AbsoluteX)
            {
                return Absolute(signalEdge, instructionCycle);
            }
            else
            {
                throw new ArgumentException($"ROR does not support {AddressingMode.ToString()} addressing mode");
            }
        }

        private CpuMicroCode Immediate(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.ROR_A,
                    MicroCodeInstruction.ShiftLowABitIntoCarry,
                    MicroCodeInstruction.TransferPCToPCS
                );
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
                else if (instructionCycle == startingCycle + 1)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDILIntoDOR,
                            MicroCodeInstruction.SetToWrite,
                            MicroCodeInstruction.ROR,
                            MicroCodeInstruction.ShiftLowDILBitIntoCarry
                        );
                }
                else if (instructionCycle == startingCycle + 3)
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
                if (instructionCycle == startingCycle + 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferHoldToDOR
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode Absolute(SignalEdge signalEdge, int instructionCycle)
        {
            int startingCycle = 3;
            if (AddressingMode == AddressingMode.AbsoluteX)
            {
                startingCycle = 4;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle)
                {
                    if (AddressingMode == AddressingMode.Absolute)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.SetToRead,
                                MicroCodeInstruction.TransferDILToPCHS,
                                MicroCodeInstruction.TransferPCSToAddressBus,
                                MicroCodeInstruction.IncrementPC
                            );
                    }
                    else if (AddressingMode == AddressingMode.AbsoluteX)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.SetToRead,
                                MicroCodeInstruction.LatchDILIntoDOR
                            );
                    }
                }
                else if (instructionCycle == startingCycle + 1)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.SetToWrite,
                            MicroCodeInstruction.ROR,
                            MicroCodeInstruction.ShiftLowDILBitIntoCarry
                        );
                }
                else if (instructionCycle == startingCycle + 3)
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
                if (instructionCycle == startingCycle)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDataIntoDIL
                        );
                }
                else if (instructionCycle == startingCycle + 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferHoldToDOR
                        );
                }
            }

            return new CpuMicroCode();
        }

        private ROR(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "ROR",
                opcode,
                addressingMode)
        {
        }
    }
}
