using System;

namespace CS6502.Core
{
    /// <summary>
    /// JMP (Jump) instruction, used to jump to the program to a new position memory.
    /// </summary>
    internal class JMP : InstructionBase
    {
        public static JMP CreateJMP(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Absolute:
                    return new JMP(0x4C, addressingMode);

                case AddressingMode.Indirect:
                    return new JMP(0x6C, addressingMode);
                
                default:
                    throw new ArgumentException($"JMP does not support {addressingMode.ToString()} addressing mode");
            }
        }

        protected override CpuMicroCode Absolute(
            SignalEdge signalEdge, 
            int instructionCycle,
            bool wasPageBoundaryCrossed)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.LatchDataIntoDIL,
                    MicroCodeInstruction.TransferDILToPCHS
                );
        }

        protected override CpuMicroCode Indirect(
            SignalEdge signalEdge, 
            int instructionCycle,
            bool wasPageBoundaryCrossed)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.LatchDataIntoDIL,
                    MicroCodeInstruction.TransferDILToPCHS
                );
        }

        private JMP(
            byte opcode,
            AddressingMode addressingMode) 
          : 
            base(
                "JMP", 
                opcode, 
                addressingMode)
        {
        }
    }
}
