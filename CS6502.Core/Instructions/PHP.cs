using System;

namespace CS6502.Core
{
    internal class PHP : InstructionBase
    {
        public PHP()
          :
            base(
                "PHP",
                0x08,
                AddressingMode.Implied)
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
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferSPToAB,
                            MicroCodeInstruction.LatchDILIntoDOR,
                            MicroCodeInstruction.SetToWrite
                        );
                }
                else if (instructionCycle == 3)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.DecrementSP
                        );
                }
            }
            else
            {
                if (instructionCycle == 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchPIntoDOR,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }
    }
}
