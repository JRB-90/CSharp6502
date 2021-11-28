using System;

namespace CS6502.Core
{
    public class STA : InstructionBase
    {
        public static STA CreateSTA(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new STA(0x85, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new STA(0x95, addressingMode);

                case AddressingMode.Absolute:
                    throw new NotImplementedException(); // TODO
                    //return new STA(0x8D, addressingMode);

                case AddressingMode.AbsoluteX:
                    throw new NotImplementedException(); // TODO
                    //return new STA(0x9D, addressingMode);

                case AddressingMode.AbsoluteY:
                    throw new NotImplementedException(); // TODO
                    //return new STA(0x99, addressingMode);

                case AddressingMode.IndirectX:
                    throw new NotImplementedException(); // TODO
                    //return new STA(0x81, addressingMode);

                case AddressingMode.IndirectY:
                    throw new NotImplementedException(); // TODO
                    //return new STA(0x91, addressingMode);

                default:
                    throw new ArgumentException($"STA does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override void Execute(CpuRegisters registers)
        {
            registers.TransferAToDataBus();
        }

        private STA(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "STA",
                opcode,
                addressingMode,
                OperationType.Write)
        {
        }
    }
}
