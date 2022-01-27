using System;

namespace CS6502.Core
{
    internal class DEC : ShiftInstructionBase
    {
        public static DEC CreateDEC(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new DEC(0xC6, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new DEC(0xD6, addressingMode);

                case AddressingMode.Absolute:
                    return new DEC(0xCE, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new DEC(0xDE, addressingMode);

                default:
                    throw new ArgumentException($"DEC does not support {addressingMode.ToString()} addressing mode");
            }
        }

        protected override CpuMicroCode Immediate(SignalEdge signalEdge, int instructionCycle)
        {
            throw new InvalidOperationException("DEC does not support Immediate addressing mode");
        }

        private DEC(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "DEC",
                opcode,
                addressingMode,
                MicroCodeInstruction.DEC,
                MicroCodeInstruction.DEC)
        {
        }
    }
}
