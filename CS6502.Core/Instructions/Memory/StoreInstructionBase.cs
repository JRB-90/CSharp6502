using System;

namespace CS6502.Core
{
    internal class StoreInstructionBase : InstructionBase
    {
        public StoreInstructionBase(
            string name, 
            byte opcode, 
            AddressingMode addressingMode,
            MicroCodeInstruction storeInstruction) 
          : 
            base(name, opcode, addressingMode)
        {
            this.storeInstruction = storeInstruction;
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status,
            bool wasPageBoundaryCrossed)
        {
            if (AddressingMode == AddressingMode.ZeroPage ||
                AddressingMode == AddressingMode.ZeroPageX ||
                AddressingMode == AddressingMode.ZeroPageY)
            {
                return ZeroPage(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.Absolute ||
                     AddressingMode == AddressingMode.AbsoluteX ||
                     AddressingMode == AddressingMode.AbsoluteY)
            {
                return Absolute(signalEdge, instructionCycle, wasPageBoundaryCrossed);
            }
            else if (AddressingMode == AddressingMode.XIndirect ||
                     AddressingMode == AddressingMode.IndirectY)
            {
                return Indirect(signalEdge, instructionCycle, wasPageBoundaryCrossed);
            }
            else
            {
                throw new ArgumentException($"{Name} does not support {AddressingMode.ToString()} addressing mode");
            }
        }

        protected virtual CpuMicroCode ZeroPage(SignalEdge signalEdge, int instructionCycle)
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
                    CpuMicroCode cpuMicroCode =
                        new CpuMicroCode(
                           MicroCodeInstruction.LatchDILIntoDOR,
                           MicroCodeInstruction.SetToWrite
                       );

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
                            storeInstruction,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }

        protected virtual CpuMicroCode Absolute(
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
                            MicroCodeInstruction.SetToWrite,
                            MicroCodeInstruction.LatchDILIntoDOR
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
                    }

                    return cpuMicroCode;
                }
                else if (instructionCycle == startingCycle + 1)
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
                            storeInstruction
                        );
                }
            }

            return new CpuMicroCode();
        }

        protected virtual CpuMicroCode Indirect(
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
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.TransferDILToPCHS,
                                MicroCodeInstruction.TransferPCSToAddressBus,
                                MicroCodeInstruction.LatchDILIntoDOR,
                                MicroCodeInstruction.SetToWrite
                            );
                    }
                    else if (AddressingMode == AddressingMode.IndirectY)
                    {
                        CpuMicroCode cpuMicroCode =
                            new CpuMicroCode(
                                MicroCodeInstruction.SetToWrite,
                                MicroCodeInstruction.LatchDILIntoDOR
                            );

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
                            storeInstruction
                        );
                }
            }

            return new CpuMicroCode();
        }

        private MicroCodeInstruction storeInstruction;
    }
}
