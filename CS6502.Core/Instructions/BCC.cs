using System;

namespace CS6502.Core
{
    internal class BCC : InstructionBase
    {
        public BCC()
          :
            base(
                "BCC",
                0x90,
                AddressingMode.Relative)
        {
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == 2)
                {
                    if (status.CarryFlag == false)
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
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.Branch,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }
    }
}
