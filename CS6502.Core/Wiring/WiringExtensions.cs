using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS6502.Core
{
    /// <summary>
    /// Static class to hold all extension methods for wiring classes.
    /// </summary>
    public static class WiringExtensions
    {
        #region String Conversions

        public static string ToBinaryString(this byte value)
        {
            return ToBinary(value, 8);
        }

        public static string ToBinaryString(this ushort value)
        {
            return ToBinary(value, 16);
        }

        public static string ToBinaryString(this uint value)
        {
            return ToBinary(value, 32);
        }

        private static string ToBinary(uint value, int charCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0b");
            string binStr = Convert.ToString(value, 2);
            int whiteSpaceToFill = charCount - binStr.Length;
            for (int i = 0; i < whiteSpaceToFill; i++)
            {
                sb.Append("0");
            }
            sb.Append(binStr);

            return sb.ToString();
        }

        public static string ToHexString(this byte value)
        {
            return ToHex(value, 2);
        }

        public static string ToHexString(this ushort value)
        {
            return ToHex(value, 4);
        }

        public static string ToHexString(this uint value)
        {
            return ToHex(value, 8);
        }

        private static string ToHex(uint value, int charCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0x");
            string hexStr = Convert.ToString(value, 16).ToUpper();
            int whiteSpaceToFill = charCount - hexStr.Length;
            for (int i = 0; i < whiteSpaceToFill; i++)
            {
                sb.Append("0");
            }
            sb.Append(hexStr);

            return sb.ToString();
        }

        #endregion

        #region Pin

        public static void SetTo(this Pin[] pins, uint value)
        {
            for (int i = 0; i < pins.Length; i++)
            {
                pins[i].State =
                    Convert.ToBoolean(value & (1 << i)) ? TriState.True : TriState.False;
            }
        }

        public static void SetTo(this IList<Pin> pins, uint value)
        {
            for (int i = 0; i < pins.Count; i++)
            {
                pins[i].State = 
                    Convert.ToBoolean(value & (1 << i)) ? TriState.True : TriState.False;
            }
        }

        public static void SetAllTo(this IEnumerable<Pin> pins, TriState state)
        {
            foreach (Pin p in pins)
            {
                p.State = state;
            }
        }

        public static string ToBinaryString(this Pin[] pins)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0b");

            for (int i = pins.Length - 1; i >= 0; i--)
            {
                sb.Append(pins[i].ToString());
            }

            return sb.ToString();
        }

        public static string ToBinaryString(this IList<Pin> pins)
        {
            return pins.ToArray().ToBinaryString();
        }

        #endregion
    }
}
