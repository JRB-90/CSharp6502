using System;

namespace CS6502.Core
{
    internal class RTS : InstructionBase
    {
        public RTS()
          :
            base(
                "RTS",
                0x60,
                AddressingMode.Implied)
        {
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == 2)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                        );
                }
            }
            else
            {
                if (instructionCycle == 2)
                {
                    return
                        new CpuMicroCode(
                        );
                }
            }

            return new CpuMicroCode();
        }
    }
}
