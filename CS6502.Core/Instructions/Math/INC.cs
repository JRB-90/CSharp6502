using System;

namespace CS6502.Core
{
    internal class INC : ShiftInstructionBase
    {
        public static INC CreateINC(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new INC(0xE6, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new INC(0xF6, addressingMode);

                case AddressingMode.Absolute:
                    return new INC(0xEE, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new INC(0xFE, addressingMode);

                default:
                    throw new ArgumentException($"INC does not support {addressingMode.ToString()} addressing mode");
            }
        }

        protected override CpuMicroCode Immediate(SignalEdge signalEdge, int instructionCycle)
        {
            throw new InvalidOperationException("INC does not support Immediate addressing mode");
        }

        private INC(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "INC",
                opcode,
                addressingMode,
                MicroCodeInstruction.INC,
                MicroCodeInstruction.INC)
        {
        }
    }
}
