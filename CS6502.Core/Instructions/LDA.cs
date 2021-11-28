using System;

namespace CS6502.Core
{
    public class LDA : InstructionBase
    {
        public static LDA CreateLDA(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new LDA(0xA9, addressingMode);

                case AddressingMode.ZeroPage:
                    return new LDA(0xA5, addressingMode);

                case AddressingMode.ZeroPageX:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xB5, addressingMode);

                case AddressingMode.Absolute:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xAD, addressingMode);

                case AddressingMode.AbsoluteX:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xBD, addressingMode);

                case AddressingMode.AbsoluteY:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xB9, addressingMode);

                case AddressingMode.IndirectX:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xA1, addressingMode);

                case AddressingMode.IndirectY:
                    throw new NotImplementedException(); // TODO
                    //return new LDA(0xB1, addressingMode);

                default:
                    throw new ArgumentException($"LDA does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.LoadA();
        }

        private LDA(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "LDA",
                opcode,
                addressingMode,
                OperationType.Read)
        {
        }
    }
}
