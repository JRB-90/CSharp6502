using System;

namespace CS6502.Core
{
    internal class BNE : InstructionBase
    {
        public BNE()
          :
            base(
                "BNE",
                0xD0,
                AddressingMode.Relative)
        {
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status,
            bool wasPageBoundaryCrossed)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == 2)
                {
                    if (status.ZeroFlag == false)
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
    }
}
