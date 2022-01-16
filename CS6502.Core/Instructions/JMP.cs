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

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status,
            bool wasPageBoundaryCrossed)
        {
            if (AddressingMode == AddressingMode.Absolute)
            {
                return ExecuteAbsolute(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.Indirect)
            {
                return ExecuteIndirect(signalEdge, instructionCycle);
            }
            else
            {
                throw new ArgumentException($"JMP does not support {AddressingMode.ToString()} addressing mode");
            }
        }

        private CpuMicroCode ExecuteAbsolute(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.LatchDataIntoDIL,
                    MicroCodeInstruction.TransferDILToPCHS
                );
        }

        private CpuMicroCode ExecuteIndirect(SignalEdge signalEdge, int instructionCycle)
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
