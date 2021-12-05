using System;

namespace CS6502.Core
{
    /// <summary>
    /// Factory class for creating instructions from their opcode.
    /// </summary>
    internal static class InstructionDecoder
    {
        public static IInstruction DecodeOpcode(byte opcode)
        {
            switch (opcode)
            {
                case 0x00:
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
                case 0x81:
                    return STA.CreateSTA(AddressingMode.XIndirect);
                case 0x85:
                    return STA.CreateSTA(AddressingMode.ZeroPage);
                case 0x88:
                    return new DEY();
                case 0xB9:
                    return LDA.CreateLDA(AddressingMode.AbsoluteY);
                case 0x8D:
                    return STA.CreateSTA(AddressingMode.Absolute);
                case 0x91:
                    return STA.CreateSTA(AddressingMode.IndirectY);
                case 0x95:
                    return STA.CreateSTA(AddressingMode.ZeroPageX);
                case 0x99:
                    return STA.CreateSTA(AddressingMode.AbsoluteY);
                case 0x9A:
                    return new TXS();
                case 0x9D:
                    return STA.CreateSTA(AddressingMode.AbsoluteX);
                case 0xA0:
                    return LDY.CreateLDY(AddressingMode.Immediate);
                case 0xA1:
                    return LDA.CreateLDA(AddressingMode.XIndirect);
                case 0xA2:
                    return LDX.CreateLDX(AddressingMode.Immediate);
                case 0xA5:
                    return LDA.CreateLDA(AddressingMode.ZeroPage);
                case 0xA9:
                    return LDA.CreateLDA(AddressingMode.Immediate);
                case 0xAD:
                    return LDA.CreateLDA(AddressingMode.Absolute);
                case 0xB1:
                    return LDA.CreateLDA(AddressingMode.IndirectY);
                case 0xB5:
                    return LDA.CreateLDA(AddressingMode.ZeroPageX);
                case 0xB8:
                    return new CLV();
                case 0xBD:
                    return LDA.CreateLDA(AddressingMode.AbsoluteX);
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
