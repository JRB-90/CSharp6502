using System;

namespace CS6502.Core
{
    internal class ROL : InstructionBase
    {
        public static ROL CreateROL(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new ROL(0x2A, addressingMode);

                case AddressingMode.ZeroPage:
                    return new ROL(0x26, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new ROL(0x36, addressingMode);

                case AddressingMode.Absolute:
                    return new ROL(0x2E, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new ROL(0x3E, addressingMode);

                default:
                    throw new ArgumentException($"ROL does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
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
                throw new ArgumentException($"ROL does not support {AddressingMode.ToString()} addressing mode");
            }
        }

        private CpuMicroCode Immediate(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.ROL_A,
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
                                MicroCodeInstruction.IncrementABByX
                            );
                    }
                }
                else if (instructionCycle == startingCycle + 1)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDILIntoDOR,
                            MicroCodeInstruction.SetToWrite,
                            MicroCodeInstruction.ROL
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
                            MicroCodeInstruction.ROL
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

        private ROL(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "ROL",
                opcode,
                addressingMode)
        {
        }
    }
}
