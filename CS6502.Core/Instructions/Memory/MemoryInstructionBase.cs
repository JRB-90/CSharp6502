namespace CS6502.Core
{
    internal abstract class MemoryInstructionBase : InstructionBase
    {
        protected MemoryInstructionBase(
            string name, 
            byte opcode, 
            AddressingMode addressingMode,
            RWState rw,
            MicroCodeInstruction memoryInstruction) 
          : 
            base(name, opcode, addressingMode)
        {
            this.rw = rw;
            this.memoryInstruction = memoryInstruction;
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
                            memoryInstruction,
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
            if (AddressingMode == AddressingMode.ZeroPageX ||
                AddressingMode == AddressingMode.ZeroPageY)
            {
                startingCycle = 3;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle)
                {
                    CpuMicroCode cpuMicroCode = new CpuMicroCode();

                    if (rw == RWState.Read)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.LatchDataIntoDIL);
                        cpuMicroCode.Add(MicroCodeInstruction.SetToRead);
                    }
                    else
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.LatchDILIntoDOR);
                        cpuMicroCode.Add(MicroCodeInstruction.SetToWrite);
                    }

                    if (AddressingMode == AddressingMode.ZeroPage)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.TransferZPDataToAB);
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementPC);
                    }
                    else if (AddressingMode == AddressingMode.ZeroPageX)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementABByX_NoCarry);
                    }
                    else if (AddressingMode == AddressingMode.ZeroPageY)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementABByY_NoCarry);
                    }

                    return cpuMicroCode;
                }
                if (instructionCycle == startingCycle + 1)
                {
                    IsInstructionComplete = true;

                    if (rw == RWState.Read)
                    {
                        return
                            new CpuMicroCode(
                                memoryInstruction
                            );
                    }
                    else
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.TransferPCToPCS
                            );
                    }
                }
            }
            else
            {
                if (instructionCycle == startingCycle)
                {
                    if (rw == RWState.Read)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.TransferPCToPCS
                            );
                    }
                    else
                    {
                        return
                            new CpuMicroCode(
                                memoryInstruction,
                                MicroCodeInstruction.TransferPCToPCS
                            );
                    }
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
                    CpuMicroCode cpuMicroCode = new CpuMicroCode();

                    if (rw == RWState.Read)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.SetToRead);
                    }
                    else
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.SetToWrite);
                        cpuMicroCode.Add(MicroCodeInstruction.LatchDILIntoDOR);
                    }

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
                        
                        if (!wasPageBoundaryCrossed &&
                            rw == RWState.Read)
                        {
                            IsInstructionComplete = true;

                            cpuMicroCode.Add(MicroCodeInstruction.LatchDataIntoDIL);
                            cpuMicroCode.Add(memoryInstruction);
                            cpuMicroCode.Add(MicroCodeInstruction.TransferPCToPCS);
                        }
                    }

                    return cpuMicroCode;
                }
                else if (instructionCycle == startingCycle + 1)
                {
                    IsInstructionComplete = true;

                    if (rw == RWState.Read)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL,
                                memoryInstruction,
                                MicroCodeInstruction.TransferPCToPCS
                            );
                    }
                    else
                    {
                        return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferPCToPCS
                        );
                    }
                }
            }
            else
            {
                if (instructionCycle == startingCycle)
                {
                    if (rw == RWState.Write)
                    {
                        return
                            new CpuMicroCode(
                                memoryInstruction
                            );
                    }
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
                if (instructionCycle == startingCycle)
                {
                    if (AddressingMode == AddressingMode.XIndirect)
                    {
                        CpuMicroCode cpuMicroCode =
                            new CpuMicroCode(
                                MicroCodeInstruction.TransferDILToPCHS,
                                MicroCodeInstruction.TransferPCSToAddressBus
                            );

                        if (rw == RWState.Read)
                        {
                            cpuMicroCode.Add(MicroCodeInstruction.SetToRead);
                        }
                        else
                        {
                            cpuMicroCode.Add(MicroCodeInstruction.LatchDILIntoDOR);
                            cpuMicroCode.Add(MicroCodeInstruction.SetToWrite);
                        }

                        return cpuMicroCode;
                    }
                    else if (AddressingMode == AddressingMode.IndirectY)
                    {
                        CpuMicroCode cpuMicroCode = new CpuMicroCode();

                        if (rw == RWState.Write)
                        {
                            cpuMicroCode.Add(MicroCodeInstruction.SetToWrite);
                            cpuMicroCode.Add(MicroCodeInstruction.LatchDILIntoDOR);
                        }

                        if (wasPageBoundaryCrossed)
                        {
                            cpuMicroCode.Add(MicroCodeInstruction.IncrementABH);
                            cpuMicroCode.Add(MicroCodeInstruction.ClearPageBoundaryCrossed);
                        }
                        else
                        {
                            if (rw == RWState.Read)
                            {
                                IsInstructionComplete = true;

                                cpuMicroCode.Add(MicroCodeInstruction.LatchDataIntoDIL);
                                cpuMicroCode.Add(memoryInstruction);
                                cpuMicroCode.Add(MicroCodeInstruction.TransferPCToPCS);
                            }
                        }

                        return cpuMicroCode;
                    }
                }
                else if (instructionCycle == startingCycle + 1)
                {
                    IsInstructionComplete = true;

                    if (rw == RWState.Read)
                    {
                        return
                            new CpuMicroCode(
                                memoryInstruction,
                                MicroCodeInstruction.TransferPCToPCS
                            );
                    }
                    else
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.TransferPCToPCS
                            );
                    }
                }
            }
            else
            {
                if (instructionCycle == startingCycle)
                {
                    if (rw == RWState.Write)
                    {
                        return
                            new CpuMicroCode(
                                memoryInstruction
                            );
                    }
                }
            }

            return new CpuMicroCode();
        }

        private RWState rw;
        private MicroCodeInstruction memoryInstruction;
    }
}
