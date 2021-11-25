using System;

namespace CS6502.Core
{
    public class LDX : InstructionBase
    {
        public static LDX CreateLDX(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new LDX(0xA2, addressingMode);

                case AddressingMode.ZeroPage:
                    throw new NotImplementedException(); // TODO
                    //return new LDX(0xA6, addressingMode);

                case AddressingMode.ZeroPageY:
                    throw new NotImplementedException(); // TODO
                    //return new LDX(0xB6, addressingMode);

                case AddressingMode.Absolute:
                    throw new NotImplementedException(); // TODO
                    //return new LDX(0xAE, addressingMode);

                case AddressingMode.AbsoluteY:
                    throw new NotImplementedException(); // TODO
                    //return new LDX(0xBE, addressingMode);

                default:
                    throw new ArgumentException($"LDX does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override void Execute(CpuRegisters registers)
        {
            switch (AddressingMode)
            {
                case AddressingMode.Immediate:
                    registers.LoadX();
                    break;
                default:
                    break;
            }
        }

        private LDX(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "LDX",
                opcode,
                addressingMode)
        {
        }
    }
}
