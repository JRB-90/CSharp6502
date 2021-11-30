﻿using System;

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
            RW = RWState.Read;
            Sync = EnableState.Disabled;
            state = DecodeState.ReadingOpcode;
            LatchIR(0x00);
        }

        public byte IR => currentInstruction.Opcode;

        public RWState RW { get; private set; }

        public EnableState Sync { get; private set; }

        public void LatchIR(byte value)
        {
            currentInstruction = InstructionDecoder.DecodeOpcode(value);
        }

        public CpuMicroCode Cycle(SignalEdge signalEdge)
        {
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
                RW = RWState.Read;

                return 
                    new CpuMicroCode(
                        MicroCodeInstruction.TransferPCSToPC_NoIncrement,
                        MicroCodeInstruction.TransferPCToAddressBus
                    );
            }
            else
            {
                state = DecodeState.Addressing;

                return 
                    new CpuMicroCode(
                        MicroCodeInstruction.LatchDataBus,
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
                currentInstruction.AddressingMode == AddressingMode.IndirectX ||
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
                        MicroCodeInstruction.LatchDataBus
                    );
            }
        }

        private CpuMicroCode ImmediateCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                return
                    new CpuMicroCode(

                    );
            }
            else
            {
                return
                    new CpuMicroCode(

                    );
            }
        }

        private CpuMicroCode ZeroPageCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                return
                    new CpuMicroCode(

                    );
            }
            else
            {
                return
                    new CpuMicroCode(

                    );
            }
        }

        private CpuMicroCode AbsoluteCycle(SignalEdge signalEdge)
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
                else if (instructionCycleCounter == 2)
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
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDataBus,
                            MicroCodeInstruction.TransferDataToPCLS
                        );
                }
                else if (instructionCycleCounter == 2)
                {
                    state = DecodeState.Executing;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDataBus,
                            MicroCodeInstruction.TransferDataToPCHS
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode IndirectCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                return
                    new CpuMicroCode(

                    );
            }
            else
            {
                return
                    new CpuMicroCode(

                    );
            }
        }

        private CpuMicroCode RelativeCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                return
                    new CpuMicroCode(

                    );
            }
            else
            {
                return
                    new CpuMicroCode(

                    );
            }
        }

        private CpuMicroCode ExecutingCycle(SignalEdge signalEdge)
        {
            state = DecodeState.ReadingOpcode;

            return ReadingOpcodeCycle(signalEdge);
        }

        private IInstruction currentInstruction;
        private int instructionCycleCounter;
        private DecodeState state;
    }
}
