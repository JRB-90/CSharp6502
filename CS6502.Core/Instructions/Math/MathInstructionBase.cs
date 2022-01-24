using System;

namespace CS6502.Core
{
    internal abstract class MathInstructionBase : InstructionBase
    {
        public MathInstructionBase(
            string name, 
            byte opcode, 
            AddressingMode addressingMode,
            MicroCodeInstruction mathInstruction)
          :
            base(name, opcode, addressingMode)
        {
            this.mathInstruction = mathInstruction;
        }

        protected override CpuMicroCode Immediate(
            SignalEdge signalEdge, 
            int instructionCycle)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                IsInstructionComplete = true;

                if (instructionCycle == 2)
                {
                    return
                        new CpuMicroCode(
                            mathInstruction,
                            MicroCodeInstruction.IncrementPC,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }

        protected override CpuMicroCode ZeroPage(
            SignalEdge signalEdge, 
            int instructionCycle)
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
                            mathInstruction,
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

        protected override CpuMicroCode Absolute(
            SignalEdge signalEdge,
            int instructionCycle,
            bool wasPageBoundaryCrossed)
        {
            int startingCycle = 3;
            if (AddressingMode == AddressingMode.AbsoluteX ||
                AddressingMode == AddressingMode.AbsoluteY)
            {
                startingCycle = 4;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle)
                {
                    CpuMicroCode cpuMicroCode =
                        new CpuMicroCode(
                            MicroCodeInstruction.SetToRead
                        );

                    if (AddressingMode == AddressingMode.Absolute)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.TransferDILToPCHS);
                        cpuMicroCode.Add(MicroCodeInstruction.TransferPCSToAddressBus);
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementPC);
                    }

                    if (AddressingMode == AddressingMode.AbsoluteX ||
                        AddressingMode == AddressingMode.AbsoluteY)
                    {
                        if (wasPageBoundaryCrossed)
                        {
                            cpuMicroCode.Add(MicroCodeInstruction.IncrementABH);
                            cpuMicroCode.Add(MicroCodeInstruction.ClearPageBoundaryCrossed);
                        }
                        else
                        {
                            IsInstructionComplete = true;
                            cpuMicroCode.Add(mathInstruction);
                            cpuMicroCode.Add(MicroCodeInstruction.TransferPCToPCS);
                        }
                    }

                    return cpuMicroCode;
                }
                else if (instructionCycle == startingCycle + 1)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            mathInstruction,
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
            }

            return new CpuMicroCode();
        }

        protected override CpuMicroCode Indirect(
            SignalEdge signalEdge,
            int instructionCycle,
            bool wasPageBoundaryCrossed)
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
                                mathInstruction,
                                MicroCodeInstruction.TransferPCToPCS
                            );
                    }
                }
                else if (AddressingMode == AddressingMode.IndirectY)
                {
                    if (instructionCycle == startingCycle)
                    {
                        if (wasPageBoundaryCrossed)
                        {
                            return
                                new CpuMicroCode(
                                    MicroCodeInstruction.IncrementABH,
                                    MicroCodeInstruction.ClearPageBoundaryCrossed
                                );
                        }
                        else
                        {
                            IsInstructionComplete = true;

                            return
                                new CpuMicroCode(
                                    mathInstruction,
                                    MicroCodeInstruction.TransferPCToPCS
                                );
                        }
                    }
                    else if (instructionCycle == startingCycle + 1)
                    {
                        IsInstructionComplete = true;

                        return
                            new CpuMicroCode(
                                mathInstruction,
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

        protected MicroCodeInstruction mathInstruction;
    }
}
