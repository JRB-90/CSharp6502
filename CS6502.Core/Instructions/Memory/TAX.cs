using System;

namespace CS6502.Core
{
    internal class TAX : InstructionBase
    {
        public TAX()
          :
            base(
                "TAX",
                0xAA,
                AddressingMode.Implied)
        {
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status,
            bool wasPageBoundaryCrossed)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.TransferAToX
                );
        }
    }
}
