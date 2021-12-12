using System;

namespace CS6502.Core
{
    /// <summary>
    /// Class to represent the decoding logic ROM inside a 6502 CPU.
    /// It is responsible for decoding the instruction and then
    /// sequencing the rest of the register transfers inside the
    /// chip to execute the instruction correctly.
    /// </summary>
    internal class DecodeLogic
    {
        public DecodeLogic()
        {
            Sync = EnableState.Disabled;
            state = DecodeState.ReadingOpcode;
            LatchIR(0x00);
        }

        public byte IR => currentInstruction.Opcode;

        public EnableState Sync { get; private set; }

        public void LatchIR(byte value)
        {
            currentInstruction = InstructionDecoder.DecodeOpcode(value);
        }

        public CpuMicroCode Cycle(SignalEdge signalEdge)
        {
            Sync = EnableState.Disabled;

            CpuMicroCode microCode = new CpuMicroCode();

            switch (state)
            {
                case DecodeState.ReadingOpcode:
                    microCode = ReadingOpcodeCycle(signalEdge);
                    break;
                case DecodeState.Addressing:
                    microCode = AddressingCycle(signalEdge);
                    break;
                case DecodeState.Executing:
                    microCode = ExecutingCycle(signalEdge);
                    break;
                default:
                    throw new InvalidOperationException($"Decoding state [{state}] not supported");
            }

            if (signalEdge == SignalEdge.RisingEdge)
            {
                instructionCycleCounter++;
            }

            return microCode;
        }

        private CpuMicroCode ReadingOpcodeCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                instructionCycleCounter = 0;

                return
                    new CpuMicroCode(
                        MicroCodeInstruction.SetToRead,
                        MicroCodeInstruction.TransferPCSToPC_NoIncrement,
                        MicroCodeInstruction.TransferPCToAddressBus
                    );
            }
            else
            {
                Sync = EnableState.Enabled;
                state = DecodeState.Addressing;

                return 
                    new CpuMicroCode(
                        MicroCodeInstruction.LatchDataIntoDIL,
                        MicroCodeInstruction.LatchIRToData
                    );
            }
        }

        private CpuMicroCode AddressingCycle(SignalEdge signalEdge)
        {
            if (currentInstruction.AddressingMode == AddressingMode.Implied)
            {
                return ImpliedCycle(signalEdge);
            }
            else if (currentInstruction.AddressingMode == AddressingMode.Immediate)
            {
                return ImmediateCycle(signalEdge);
            }
            else if (
                currentInstruction.AddressingMode == AddressingMode.ZeroPage ||
                currentInstruction.AddressingMode == AddressingMode.ZeroPageX ||
                currentInstruction.AddressingMode == AddressingMode.ZeroPageY)
            {
                return ZeroPageCycle(signalEdge);
            }
            else if (
                currentInstruction.AddressingMode == AddressingMode.Absolute ||
                currentInstruction.AddressingMode == AddressingMode.AbsoluteX ||
                currentInstruction.AddressingMode == AddressingMode.AbsoluteY)
            {
                return AbsoluteCycle(signalEdge);
            }
            else if (
                currentInstruction.AddressingMode == AddressingMode.Indirect ||
                currentInstruction.AddressingMode == AddressingMode.XIndirect ||
                currentInstruction.AddressingMode == AddressingMode.IndirectY)
            {
                return IndirectCycle(signalEdge);
            }
            else if (currentInstruction.AddressingMode == AddressingMode.Relative)
            {
                return RelativeCycle(signalEdge);
            }
            else
            {
                throw new InvalidOperationException($"Addressing Mode [{currentInstruction.AddressingMode.ToString()}] not supported");
            }
        }

        private CpuMicroCode ImpliedCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                return
                    new CpuMicroCode(
                        MicroCodeInstruction.TransferPCSToPC_WithCarryIncrement,
                        MicroCodeInstruction.TransferPCToAddressBus
                    );
            }
            else
            {
                state = DecodeState.Executing;

                return
                    new CpuMicroCode(
                        MicroCodeInstruction.TransferPCLToPCLS,
                        MicroCodeInstruction.TransferPCHToPCHS,
                        MicroCodeInstruction.LatchDataIntoDIL
                    );
            }
        }

        private CpuMicroCode ImmediateCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycleCounter == 1)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.IncrementPC,
                            MicroCodeInstruction.TransferPCToAddressBus
                        );
                }
            }
            else
            {
                if (instructionCycleCounter == 1)
                {
                    state = DecodeState.Executing;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDataIntoDIL
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode ZeroPageCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (currentInstruction.AddressingMode == AddressingMode.ZeroPage)
                {
                    if (instructionCycleCounter == 1)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferPCToAddressBus
                            );
                    }
                }
                else if (currentInstruction.AddressingMode == AddressingMode.ZeroPageX ||
                         currentInstruction.AddressingMode == AddressingMode.ZeroPageY)
                {
                    if (instructionCycleCounter == 1)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferPCToAddressBus
                            );
                    }
                    else if (instructionCycleCounter == 2)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferZPDataToAB
                            );
                    }
                }
            }
            else
            {
                if (currentInstruction.AddressingMode == AddressingMode.ZeroPage)
                {
                    if (instructionCycleCounter == 1)
                    {
                        state = DecodeState.Executing;

                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL
                            );
                    }
                }
                else if (currentInstruction.AddressingMode == AddressingMode.ZeroPageX ||
                         currentInstruction.AddressingMode == AddressingMode.ZeroPageY)
                {
                    if (instructionCycleCounter == 1)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL
                            );
                    }
                    else if (instructionCycleCounter == 2)
                    {
                        state = DecodeState.Executing;

                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL
                            );
                    }
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode AbsoluteCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (currentInstruction.AddressingMode == AddressingMode.Absolute)
                {
                    if (instructionCycleCounter == 1)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferPCToAddressBus
                            );
                    }
                    else if (instructionCycleCounter == 2)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL,
                                MicroCodeInstruction.TransferDILToPCLS,
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferPCToAddressBus
                            );
                    }
                }
                else if (currentInstruction.AddressingMode == AddressingMode.AbsoluteX ||
                         (currentInstruction.AddressingMode == AddressingMode.AbsoluteY))
                {
                    if (instructionCycleCounter == 1)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferPCToAddressBus
                            );
                    }
                    else if (instructionCycleCounter == 2)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL,
                                MicroCodeInstruction.TransferDILToPCLS,
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferPCToAddressBus
                            );
                    }
                    else if (instructionCycleCounter == 3)
                    {
                        CpuMicroCode cpuMicroCode =
                            new CpuMicroCode(
                                MicroCodeInstruction.TransferDILToPCHS,
                                MicroCodeInstruction.TransferPCSToAddressBus,
                                MicroCodeInstruction.IncrementPC
                            );

                        if (currentInstruction.AddressingMode == AddressingMode.AbsoluteX)
                        {
                            cpuMicroCode.Add(MicroCodeInstruction.IncrementABByX);
                        }
                        else if (currentInstruction.AddressingMode == AddressingMode.AbsoluteY)
                        {
                            cpuMicroCode.Add(MicroCodeInstruction.IncrementABByY);
                        }

                        return cpuMicroCode;
                    }
                }
            }
            else
            {
                if (currentInstruction.AddressingMode == AddressingMode.Absolute)
                {
                    if (instructionCycleCounter == 2)
                    {
                        state = DecodeState.Executing;
                    }
                }
                else if (currentInstruction.AddressingMode == AddressingMode.AbsoluteX ||
                         currentInstruction.AddressingMode == AddressingMode.AbsoluteY)
                {
                    if (instructionCycleCounter == 3)
                    {
                        state = DecodeState.Executing;
                    }
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode IndirectCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (currentInstruction.AddressingMode == AddressingMode.XIndirect ||
                    currentInstruction.AddressingMode == AddressingMode.IndirectY)
                {
                    if (instructionCycleCounter == 1)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferPCToAddressBus
                            );
                    }
                    else if (instructionCycleCounter == 2)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL,
                                MicroCodeInstruction.TransferZPDataToAB,
                                MicroCodeInstruction.IncrementPC
                            );
                    }

                    if (currentInstruction.AddressingMode == AddressingMode.XIndirect)
                    {
                        if (instructionCycleCounter == 3)
                        {
                            return
                                new CpuMicroCode(
                                    MicroCodeInstruction.LatchDataIntoDIL,
                                    MicroCodeInstruction.IncrementABByX
                                );
                        }
                        else if (instructionCycleCounter == 4)
                        {
                            return
                                new CpuMicroCode(
                                    MicroCodeInstruction.LatchDataIntoDIL,
                                    MicroCodeInstruction.TransferDILToPCLS,
                                    MicroCodeInstruction.IncrementAB_NoCarry
                                );
                        }
                    }
                    else if (currentInstruction.AddressingMode == AddressingMode.IndirectY)
                    {
                        if (instructionCycleCounter == 3)
                        {
                            return
                                new CpuMicroCode(
                                    MicroCodeInstruction.LatchDataIntoDIL,
                                    MicroCodeInstruction.TransferDILToPCLS,
                                    MicroCodeInstruction.IncrementAB_NoCarry
                                );
                        }
                        else if (instructionCycleCounter == 4)
                        {
                            return 
                                new CpuMicroCode(
                                    MicroCodeInstruction.TransferDILToPCHS,
                                    MicroCodeInstruction.TransferPCSToAddressBus,
                                    MicroCodeInstruction.IncrementABByY_WithCarry
                                );
                        }
                    }
                }
                else if (currentInstruction.AddressingMode == AddressingMode.Indirect)
                {
                    if (instructionCycleCounter == 1)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferPCToAddressBus
                            );
                    }
                    else if (instructionCycleCounter == 2)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL,
                                MicroCodeInstruction.TransferDILToPCLS,
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferPCToAddressBus
                            );
                    }
                    else if (instructionCycleCounter == 3)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL,
                                MicroCodeInstruction.TransferDILToPCHS,
                                MicroCodeInstruction.TransferPCSToAddressBus,
                                MicroCodeInstruction.IncrementPC
                            );
                    }
                    else if (instructionCycleCounter == 4)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL,
                                MicroCodeInstruction.TransferDILToPCLS,

                                // TODO - If this crosses the page boundary, it will take another cycle
                                // Need to implement this in the future...
                                MicroCodeInstruction.IncrementAB_NoCarry
                            );
                    }
                }
            }
            else
            {
                if (currentInstruction.AddressingMode == AddressingMode.XIndirect ||
                    currentInstruction.AddressingMode == AddressingMode.IndirectY)
                {
                    if (instructionCycleCounter == 3)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL
                            );
                    }
                    else if (instructionCycleCounter == 4)
                    {
                        state = DecodeState.Executing;

                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL
                            );
                    }
                }
                else if (currentInstruction.AddressingMode == AddressingMode.Indirect)
                {
                    if (instructionCycleCounter == 3)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL
                            );
                    }
                    else if (instructionCycleCounter == 4)
                    {
                        state = DecodeState.Executing;

                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL
                            );
                    }
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode RelativeCycle(SignalEdge signalEdge)
        {
            throw new NotImplementedException();
        }

        private CpuMicroCode ExecutingCycle(SignalEdge signalEdge)
        {
            CpuMicroCode instructionCode =
                currentInstruction.Execute(
                    signalEdge,
                    instructionCycleCounter
                );

            if (currentInstruction.IsInstructionComplete)
            {
                state = DecodeState.ReadingOpcode;
                instructionCode =
                    instructionCode +
                    ReadingOpcodeCycle(signalEdge);
            }

            return instructionCode;
        }

        private IInstruction currentInstruction;
        private int instructionCycleCounter;
        private DecodeState state;
    }
}
