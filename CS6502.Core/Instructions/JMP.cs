using System;

namespace CS6502.Core
{
    /// <summary>
    /// JMP (Jump) instruction, used to jump to the program to a new position memory.
    /// </summary>
    public class JMP : InstructionBase
    {
        public static JMP CreateJMP(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Indirect:
                    return new JMP(0x6C, addressingMode);
                case AddressingMode.Absolute:
                    return new JMP(0x4C, addressingMode);
                default:
                    throw new ArgumentException($"JMP does not support {addressingMode.ToString()} addressing mode");
            }
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
