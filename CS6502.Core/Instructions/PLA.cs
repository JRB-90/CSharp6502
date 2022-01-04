using System;

namespace CS6502.Core
{
    internal class PLA : InstructionBase
    {
        public PLA()
          :
            base(
                "PLA",
                0x68,
                AddressingMode.Implied)
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
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferSPToAB
                        );
                }
                else if (instructionCycle == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.IncrementSP,
                            MicroCodeInstruction.TransferSPToAB
                        );
                }
                else if (instructionCycle == 4)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDILIntoA
                        );
                }
            }
            else
            {
                if (instructionCycle == 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDataIntoDIL,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
                else if (instructionCycle == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDataIntoDIL
                        );
                }
            }

            return new CpuMicroCode();
        }
    }
}
