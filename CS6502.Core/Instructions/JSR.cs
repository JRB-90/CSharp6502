using System;

namespace CS6502.Core
{
    internal class JSR : InstructionBase
    {
        public JSR()
          :
            base(
                "JSR",
                0x20,
                AddressingMode.Absolute)
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
