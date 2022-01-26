using System;

namespace CS6502.Core
{
    internal abstract class ShiftInstructionBase : InstructionBase
    {
        public ShiftInstructionBase(
            string name,
            byte opcode,
            AddressingMode addressingMode,
            MicroCodeInstruction shiftInstruction,
            MicroCodeInstruction shiftInstruction_A)
          :
            base(name, opcode, addressingMode)
        {
            this.shiftInstruction = shiftInstruction;
            this.shiftInstruction_A = shiftInstruction_A;
        }

        protected override CpuMicroCode Immediate(
            SignalEdge signalEdge, 
            int instructionCycle)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    shiftInstruction_A,
                    MicroCodeInstruction.TransferPCToPCS
                );
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
                                MicroCodeInstruction.SetToRead,
                                MicroCodeInstruction.IncrementPC
                            );
                    }
                    else if (AddressingMode == AddressingMode.ZeroPageX)
                    {
                        return
                            new CpuMicroCode(
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
                            shiftInstruction
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

        protected override CpuMicroCode Absolute(
            SignalEdge signalEdge,
            int instructionCycle,
            bool wasPageBoundaryCrossed)
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
                        CpuMicroCode cpuMicroCode = new CpuMicroCode();
                        cpuMicroCode.Add(MicroCodeInstruction.SetToRead);
                        cpuMicroCode.Add(MicroCodeInstruction.LatchDILIntoDOR);

                        if (wasPageBoundaryCrossed)
                        {
                            cpuMicroCode.Add(MicroCodeInstruction.IncrementABH);
                            cpuMicroCode.Add(MicroCodeInstruction.ClearPageBoundaryCrossed);
                        }

                        return cpuMicroCode;
                    }
                }
                else if (instructionCycle == startingCycle + 1)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.SetToWrite,
                            shiftInstruction,
                            MicroCodeInstruction.LatchDILIntoDOR
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

        protected MicroCodeInstruction shiftInstruction;
        protected MicroCodeInstruction shiftInstruction_A;
    }
}
