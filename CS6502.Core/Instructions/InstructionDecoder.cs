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
                case 0x06:
                    return ASL.CreateASL(AddressingMode.ZeroPage);
                case 0x08:
                    return new PHP();
                case 0x0A:
                    return ASL.CreateASL(AddressingMode.Immediate);
                case 0x0E:
                    return ASL.CreateASL(AddressingMode.Absolute);
                case 0x16:
                    return ASL.CreateASL(AddressingMode.ZeroPageX);
                case 0x18:
                    return new CLC();
                case 0x1E:
                    return ASL.CreateASL(AddressingMode.AbsoluteX);
                case 0x20:
                    return new JSR();
                case 0x26:
                    return ROL.CreateROL(AddressingMode.ZeroPage);
                case 0x28:
                    return new PLP();
                case 0x2A:
                    return ROL.CreateROL(AddressingMode.Immediate);
                case 0x2E:
                    return ROL.CreateROL(AddressingMode.Absolute);
                case 0x36:
                    return ROL.CreateROL(AddressingMode.ZeroPageX);
                case 0x38:
                    return new SEC();
                case 0x3E:
                    return ROL.CreateROL(AddressingMode.AbsoluteX);
                case 0x46:
                    return LSR.CreateLSR(AddressingMode.ZeroPage);
                case 0x48:
                    return new PHA();
                case 0x4A:
                    return LSR.CreateLSR(AddressingMode.Immediate);
                case 0x4C:
                    return JMP.CreateJMP(AddressingMode.Absolute);
                case 0x4E:
                    return LSR.CreateLSR(AddressingMode.Absolute);
                case 0x56:
                    return LSR.CreateLSR(AddressingMode.ZeroPageX);
                case 0x58:
                    return new CLI();
                case 0x5E:
                    return LSR.CreateLSR(AddressingMode.AbsoluteX);
                case 0x60:
                    return new RTS();
                case 0x61:
                    return ADC.CreateADC(AddressingMode.XIndirect);
                case 0x65:
                    return ADC.CreateADC(AddressingMode.ZeroPage);
                case 0x66:
                    return ROR.CreateROR(AddressingMode.ZeroPage);
                case 0x68:
                    return new PLA();
                case 0x69:
                    return ADC.CreateADC(AddressingMode.Immediate);
                case 0x6A:
                    return ROR.CreateROR(AddressingMode.Immediate);
                case 0x6C:
                    return JMP.CreateJMP(AddressingMode.Indirect);
                case 0x6D:
                    return ADC.CreateADC(AddressingMode.Absolute);
                case 0x6E:
                    return ROR.CreateROR(AddressingMode.Absolute);
                case 0x71:
                    return ADC.CreateADC(AddressingMode.IndirectY);
                case 0x75:
                    return ADC.CreateADC(AddressingMode.ZeroPageX);
                case 0x76:
                    return ROR.CreateROR(AddressingMode.ZeroPageX);
                case 0x78:
                    return new SEI();
                case 0x79:
                    return ADC.CreateADC(AddressingMode.AbsoluteY);
                case 0x7D:
                    return ADC.CreateADC(AddressingMode.AbsoluteX);
                case 0x7E:
                    return ROR.CreateROR(AddressingMode.AbsoluteX);
                case 0x81:
                    return STA.CreateSTA(AddressingMode.XIndirect);
                case 0x84:
                    return STY.CreateSTY(AddressingMode.ZeroPage);
                case 0x85:
                    return STA.CreateSTA(AddressingMode.ZeroPage);
                case 0x86:
                    return STX.CreateSTX(AddressingMode.ZeroPage);
                case 0x88:
                    return new DEY();
                case 0x8A:
                    return new TXA();
                case 0x8C:
                    return STY.CreateSTY(AddressingMode.Absolute);
                case 0x8E:
                    return STX.CreateSTX(AddressingMode.Absolute);
                case 0xB9:
                    return LDA.CreateLDA(AddressingMode.AbsoluteY);
                case 0x8D:
                    return STA.CreateSTA(AddressingMode.Absolute);
                case 0x91:
                    return STA.CreateSTA(AddressingMode.IndirectY);
                case 0x94:
                    return STY.CreateSTY(AddressingMode.ZeroPageX);
                case 0x95:
                    return STA.CreateSTA(AddressingMode.ZeroPageX);
                case 0x96:
                    return STX.CreateSTX(AddressingMode.ZeroPageY);
                case 0x98:
                    return new TYA();
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
                case 0xA4:
                    return LDY.CreateLDY(AddressingMode.ZeroPage);
                case 0xA5:
                    return LDA.CreateLDA(AddressingMode.ZeroPage);
                case 0xA6:
                    return LDX.CreateLDX(AddressingMode.ZeroPage);
                case 0xA8:
                    return new TAY();
                case 0xA9:
                    return LDA.CreateLDA(AddressingMode.Immediate);
                case 0xAA:
                    return new TAX();
                case 0xAC:
                    return LDY.CreateLDY(AddressingMode.Absolute);
                case 0xAD:
                    return LDA.CreateLDA(AddressingMode.Absolute);
                case 0xAE:
                    return LDX.CreateLDX(AddressingMode.Absolute);
                case 0xB1:
                    return LDA.CreateLDA(AddressingMode.IndirectY);
                case 0xB4:
                    return LDY.CreateLDY(AddressingMode.ZeroPageX);
                case 0xB5:
                    return LDA.CreateLDA(AddressingMode.ZeroPageX);
                case 0xB6:
                    return LDX.CreateLDX(AddressingMode.ZeroPageY);
                case 0xB8:
                    return new CLV();
                case 0xBA:
                    return new TSX();
                case 0xBC:
                    return LDY.CreateLDY(AddressingMode.AbsoluteX);
                case 0xBD:
                    return LDA.CreateLDA(AddressingMode.AbsoluteX);
                case 0xBE:
                    return LDX.CreateLDX(AddressingMode.AbsoluteY);
                case 0xC6:
                    return DEC.CreateDEC(AddressingMode.ZeroPage);
                case 0xC8:
                    return new INY();
                case 0xCA:
                    return new DEX();
                case 0xCE:
                    return DEC.CreateDEC(AddressingMode.Absolute);
                case 0xD6:
                    return DEC.CreateDEC(AddressingMode.ZeroPageX);
                case 0xD8:
                    return new CLD();
                case 0xDE:
                    return DEC.CreateDEC(AddressingMode.AbsoluteX);
                case 0xE1:
                    return SBC.CreateSBC(AddressingMode.XIndirect);
                case 0xE5:
                    return SBC.CreateSBC(AddressingMode.ZeroPage);
                case 0xE6:
                    return INC.CreateINC(AddressingMode.ZeroPage);
                case 0xE8:
                    return new INX();
                case 0xE9:
                    return SBC.CreateSBC(AddressingMode.Immediate);
                case 0xEA:
                    return new NOP();
                case 0xED:
                    return SBC.CreateSBC(AddressingMode.Absolute);
                case 0xEE:
                    return INC.CreateINC(AddressingMode.Absolute);
                case 0xF1:
                    return SBC.CreateSBC(AddressingMode.IndirectY);
                case 0xF5:
                    return SBC.CreateSBC(AddressingMode.ZeroPageX);
                case 0xF6:
                    return INC.CreateINC(AddressingMode.ZeroPageX);
                case 0xF8:
                    return new SED();
                case 0xF9:
                    return SBC.CreateSBC(AddressingMode.AbsoluteY);
                case 0xFD:
                    return SBC.CreateSBC(AddressingMode.AbsoluteX);
                case 0xFE:
                    return INC.CreateINC(AddressingMode.AbsoluteX);
                default:
                    throw new ArgumentException($"Opcode {opcode.ToHexString()} not supported yet");
            }
        }
    }
}
