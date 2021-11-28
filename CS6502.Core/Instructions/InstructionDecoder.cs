using System;

namespace CS6502.Core
{
    /// <summary>
    /// Factory class for creating instructions from their opcode.
    /// </summary>
    public static class InstructionDecoder
    {
        public static IInstruction DecodeOpcode(byte opcode)
        {
            switch (opcode)
            {
                case 0X00:
                    return new BRK();
                case 0x18:
                    return new CLC();
                case 0x38:
                    return new SEC();
                case 0x4C:
                    return JMP.CreateJMP(AddressingMode.Absolute);
                case 0x58:
                    return new CLI();
                case 0x6C:
                    return JMP.CreateJMP(AddressingMode.Indirect);
                case 0x78:
                    return new SEI();
                case 0x85:
                    return STA.CreateSTA(AddressingMode.ZeroPage);
                case 0x88:
                    return new DEY();
                case 0x95:
                    return STA.CreateSTA(AddressingMode.ZeroPageX);
                case 0xA0:
                    return LDY.CreateLDY(AddressingMode.Immediate);
                case 0xA2:
                    return LDX.CreateLDX(AddressingMode.Immediate);
                case 0xA5:
                    return LDA.CreateLDA(AddressingMode.ZeroPage);
                case 0xA9:
                    return LDA.CreateLDA(AddressingMode.Immediate);
                case 0xB8:
                    return new CLV();
                case 0xC8:
                    return new INY();
                case 0xCA:
                    return new DEX();
                case 0xD8:
                    return new CLD();
                case 0xEA:
                    return new NOP();
                case 0xE8:
                    return new INX();
                case 0xF8:
                    return new SED();
                default:
                    throw new ArgumentException($"Opcode {opcode.ToHexString()} not supported yet");
            }
        }
    }
}
