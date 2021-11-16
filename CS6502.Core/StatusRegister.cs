using System;

namespace CS6502.Core
{
    /// <summary>
    /// Represents a 6502 status register and provides an easier
    /// approach to interacting with it.
    /// </summary>
    public class StatusRegister
    {
        const byte C    = 0b00000001;
        const byte C_N  = 0b11111110;
        const byte Z    = 0b00000010;
        const byte Z_N  = 0b11111101;
        const byte I    = 0b00000100;
        const byte I_N  = 0b11111011;
        const byte D    = 0b00001000;
        const byte D_N  = 0b11110111;
        const byte B    = 0b00010000;
        const byte B_N  = 0b11101111;
        const byte NA   = 0b00100000;
        const byte V    = 0b01000000;
        const byte V_N  = 0b10111111;
        const byte N    = 0b10000000;
        const byte N_N  = 0b01111111;

        public StatusRegister()
        {
            value = 0x00;
        }

        public StatusRegister(byte initialValue)
        {
            value = initialValue;
        }

        public byte Value
        {
            get => value;
            set => this.value = value;
        }

        public bool CarryFlag
        {
            get => Convert.ToBoolean(value & C);
            set
            {
                if (value == true)
                {
                    this.value |= C;
                }
                else
                {
                    this.value &= C_N;
                }
            }
        }

        public bool ZeroFlag
        {
            get => Convert.ToBoolean(value & Z);
            set
            {
                if (value == true)
                {
                    this.value |= Z;
                }
                else
                {
                    this.value &= Z_N;
                }
            }
        }

        public bool IrqFlag
        {
            get => Convert.ToBoolean(value & I);
            set
            {
                if (value == true)
                {
                    this.value |= I;
                }
                else
                {
                    this.value &= I_N;
                }
            }
        }

        public bool DecimalFlag
        {
            get => Convert.ToBoolean(value & D);
            set
            {
                if (value == true)
                {
                    this.value |= D;
                }
                else
                {
                    this.value &= D_N;
                }
            }
        }

        public bool BrkFlag
        {
            get => Convert.ToBoolean(value & B);
            set
            {
                if (value == true)
                {
                    this.value |= B;
                }
                else
                {
                    this.value &= B_N;
                }
            }
        }

        public bool OverflowFlag
        {
            get => Convert.ToBoolean(value & V);
            set
            {
                if (value == true)
                {
                    this.value |= V;
                }
                else
                {
                    this.value &= V_N;
                }
            }
        }

        public bool NegativeFlag
        {
            get => Convert.ToBoolean(value & N);
            set
            {
                if (value == true)
                {
                    this.value |= N;
                }
                else
                {
                    this.value &= N_N;
                }
            }
        }

        public override string ToString()
        {
            return 
                $"{(NegativeFlag ? "N" : "0")}" +
                $"{(OverflowFlag ? "V" : "0")}" +
                $"1" +
                $"{(BrkFlag ? "B" : "0")}" +
                $"{(DecimalFlag ? "D" : "0")}" +
                $"{(IrqFlag ? "I" : "0")}" +
                $"{(ZeroFlag ? "Z" : "0")}" +
                $"{(CarryFlag ? "C" : "0")}" +
                $" - {value.ToHexString()}";
        }

        private byte value;
    }
}
