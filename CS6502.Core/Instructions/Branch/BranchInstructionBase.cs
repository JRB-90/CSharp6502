using System;

namespace CS6502.Core
{
    internal abstract class BranchInstructionBase : InstructionBase
    {
        public BranchInstructionBase(
            string name, 
            byte opcode, 
            AddressingMode addressingMode,
            Func<StatusRegister, bool> branchTest)
          : 
            base(name, opcode, addressingMode)
        {
            this.branchTest = branchTest;
        }

        protected override CpuMicroCode Relative(
            SignalEdge signalEdge, 
            int instructionCycle, 
            bool wasPageBoundaryCrossed,
            StatusRegister status)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == 2)
                {
                    if (branchTest.Invoke(status))
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchBranchShift,
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferPCToAddressBus
                            );
                    }
                    else
                    {
                        IsInstructionComplete = true;

                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.IncrementPC,
                                MicroCodeInstruction.TransferPCToPCS
                            );
                    }
                }
                else if (instructionCycle == 3)
                {
                    if (!wasPageBoundaryCrossed)
                    {
                        IsInstructionComplete = true;
                    }

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.Branch,
                            MicroCodeInstruction.TransferPCToPCS,
                            MicroCodeInstruction.ClearPageBoundaryCrossed
                        );
                }
                else if (instructionCycle == 4)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.IncrementABH,
                            MicroCodeInstruction.IncrementPCHS
                        );
                }
            }

            return new CpuMicroCode();
        }

        private Func<StatusRegister, bool> branchTest;
    }
}
