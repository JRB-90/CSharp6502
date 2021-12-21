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
                    return new LDA(0xB5, addressingMode);

                case AddressingMode.Absolute:
                    return new LDA(0xAD, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new LDA(0xBD, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new LDA(0xB9, addressingMode);

                case AddressingMode.XIndirect:
                    return new LDA(0xA1, addressingMode);

                case AddressingMode.IndirectY:
                    return new LDA(0xB1, addressingMode);

                default:
                    throw new ArgumentException($"LDA does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status)
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
            int startingCycle = 2;
            if (AddressingMode == AddressingMode.ZeroPageX)
            {
                startingCycle = 3;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle)
                {
                    CpuMicroCode cpuMicroCode =
                        new CpuMicroCode(
                           MicroCodeInstruction.TransferZPDataToAB,
                           MicroCodeInstruction.LatchDataIntoDIL,
                           MicroCodeInstruction.SetToRead
                       );

                    if (AddressingMode == AddressingMode.ZeroPage)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementPC);
                    }
                    else if (AddressingMode == AddressingMode.ZeroPageX)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementABByX);
                    }
                    else if (AddressingMode == AddressingMode.ZeroPageY)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementABByY);
                    }

                    return cpuMicroCode;
                }
                if (instructionCycle == startingCycle + 1)
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
                            MicroCodeInstruction.LatchDataIntoDIL,
                            MicroCodeInstruction.LatchDILIntoA,
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
                                MicroCodeInstruction.LatchDataIntoDIL,
                                MicroCodeInstruction.LatchDILIntoA,
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
                                MicroCodeInstruction.LatchDataIntoDIL,
                                MicroCodeInstruction.LatchDILIntoA,
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
