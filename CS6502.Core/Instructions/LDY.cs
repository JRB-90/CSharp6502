﻿using System;

namespace CS6502.Core
{
    public class LDY : InstructionBase
    {
        public static LDY CreateLDY(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new LDY(0xA0, addressingMode);

                case AddressingMode.ZeroPage:
                    throw new NotImplementedException(); // TODO
                    //return new LDY(0xA4, addressingMode);

                case AddressingMode.ZeroPageX:
                    throw new NotImplementedException(); // TODO
                    //return new LDY(0xB4, addressingMode);

                case AddressingMode.Absolute:
                    throw new NotImplementedException(); // TODO
                    //return new LDY(0xAC, addressingMode);

                case AddressingMode.AbsoluteX:
                    throw new NotImplementedException(); // TODO
                    //return new LDY(0xBC, addressingMode);

                default:
                    throw new ArgumentException($"LDY does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override void Execute(CpuRegisters registers)
        {
            switch (AddressingMode)
            {
                case AddressingMode.Immediate:
                    registers.LoadY();
                    break;
                default:
                    break;
            }
        }

        private LDY(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "LDY",
                opcode,
                addressingMode)
        {
        }
    }
}