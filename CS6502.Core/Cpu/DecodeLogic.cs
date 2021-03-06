using System;
using System.Linq;

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

        public CpuMicroCode Cycle(
            SignalEdge signalEdge,
            StatusRegister status,
            InteruptControl interuptControl,
            bool wasPageBoundaryCrossed)
        {
            Sync = EnableState.Disabled;

            CpuMicroCode microCode = new CpuMicroCode();

            switch (state)
            {
                case DecodeState.ReadingOpcode:
                    microCode = ReadingOpcodeCycle(signalEdge, interuptControl);
                    break;
                case DecodeState.Addressing:
                    microCode = AddressingCycle(signalEdge);
                    break;
                case DecodeState.Executing:
                    microCode = ExecutingCycle(signalEdge, status, interuptControl, wasPageBoundaryCrossed);
                    break;
                case DecodeState.Interupt:
                    microCode = InteruptCycle(signalEdge, interuptControl);
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

        private CpuMicroCode ReadingOpcodeCycle(
            SignalEdge signalEdge,
            InteruptControl interuptControl)
        {
            if (interuptControl.IrqActive ||
                interuptControl.NmiActive)
            {
                state = DecodeState.Interupt;

                return InteruptCycle(signalEdge, interuptControl);
            }

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
                            cpuMicroCode.Add(MicroCodeInstruction.IncrementABByX_WithPBCheck);
                        }
                        else if (currentInstruction.AddressingMode == AddressingMode.AbsoluteY)
                        {
                            cpuMicroCode.Add(MicroCodeInstruction.IncrementABByY_WithPBCheck);
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
                                    MicroCodeInstruction.IncrementABByX_NoCarry
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
                                    MicroCodeInstruction.IncrementABByY_WithPBCheck
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

        private CpuMicroCode ExecutingCycle(
            SignalEdge signalEdge,
            StatusRegister status,
            InteruptControl interuptControl,
            bool wasPageBoundaryCrossed)
        {
            CpuMicroCode instructionCode =
                currentInstruction.Execute(
                    signalEdge,
                    instructionCycleCounter,
                    status,
                    wasPageBoundaryCrossed
                );

            // Special case for software triggered interupts
            if (instructionCode.MicroCode.Contains(MicroCodeInstruction.BRK))
            {
                interuptControl.SignalBRK();
            }

            if (currentInstruction.IsInstructionComplete)
            {
                state = DecodeState.ReadingOpcode;
                instructionCode =
                    instructionCode +
                    ReadingOpcodeCycle(signalEdge, interuptControl);
            }

            return instructionCode;
        }

        private CpuMicroCode InteruptCycle(
            SignalEdge signalEdge,
            InteruptControl interuptControl)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycleCounter == 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.IncrementPC,
                            MicroCodeInstruction.TransferSPToAB,
                            MicroCodeInstruction.LatchDILIntoDOR,
                            MicroCodeInstruction.SetToWrite
                        );
                }
                else if (instructionCycleCounter == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.DecrementAB_NoCarry,
                            MicroCodeInstruction.LatchDILIntoDOR
                        );
                }
                else if (instructionCycleCounter == 4)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.DecrementAB_NoCarry,
                            MicroCodeInstruction.LatchDILIntoDOR
                        );
                }
                else if (instructionCycleCounter == 5)
                {
                    CpuMicroCode cpuMicroCode = new CpuMicroCode();
                    cpuMicroCode.Add(MicroCodeInstruction.SetToRead);
                    cpuMicroCode.Add(MicroCodeInstruction.DecrementSP);
                    cpuMicroCode.Add(MicroCodeInstruction.DecrementSP);
                    cpuMicroCode.Add(MicroCodeInstruction.DecrementSP);

                    if (interuptControl.NmiActive)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.TransferNmiVecToAB);
                        interuptControl.ClearNmi();
                    }
                    else
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.TransferIrqVecToAB);
                        interuptControl.ClearBrk();
                    }

                    return cpuMicroCode;
                }
                else if (instructionCycleCounter == 6)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferDILToPCLS,
                            MicroCodeInstruction.IncrementAB_NoCarry
                        );
                }
                else if (instructionCycleCounter == 7)
                {
                    state = DecodeState.ReadingOpcode;

                    CpuMicroCode cpuMicroCode = new CpuMicroCode();
                    cpuMicroCode.Add(MicroCodeInstruction.TransferDILToPCHS);

                    return
                        cpuMicroCode +
                        ReadingOpcodeCycle(signalEdge, interuptControl);
                }
            }
            else
            {
                if (instructionCycleCounter == 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchPCHIntoDOR
                        );
                }
                else if (instructionCycleCounter == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchPCLIntoDOR
                        );
                }
                else if (instructionCycleCounter == 4)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchPIntoDOR
                        );
                }
            }

            return new CpuMicroCode();
        }

        private IInstruction currentInstruction;
        private int instructionCycleCounter;
        private DecodeState state;
    }
}
