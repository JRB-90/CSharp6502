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

        public static string ToNumStr(this bool value)
        {
            return value == true ? "1" : "0";
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

        #region Wire

        public static byte ToByte(this Wire[] wires)
        {
            if (wires.Length > 8)
            {
                throw new ArgumentException("Too many wires to resolve to a byte, must be <= 8");
            }

            byte value = 0;
            for (int i = 0; i < wires.Length; i++)
            {
                value |= (byte)(Convert.ToByte(wires[i].State ? 1 : 0) << i);
            }

            return value;
        }

        public static ushort ToUshort(this Wire[] wires)
        {
            if (wires.Length > 16)
            {
                throw new ArgumentException("Too many wires to resolve to a ushort, must be <= 16");
            }

            ushort value = 0;
            for (int i = 0; i < wires.Length; i++)
            {
                value |= (ushort)(Convert.ToByte(wires[i].State ? 1 : 0) << i);
            }

            return value;
        }

        public static uint ToUint(this Wire[] wires)
        {
            if (wires.Length > 32)
            {
                throw new ArgumentException("Too many wires to resolve to a uint, must be <= 32");
            }

            uint value = 0;
            for (int i = 0; i < wires.Length; i++)
            {
                value |= (uint)(Convert.ToByte(wires[i].State ? 1 : 0) << i);
            }

            return value;
        }

        public static byte ToByte(this List<Wire> wires)
        {
            if (wires.Count > 8)
            {
                throw new ArgumentException("Too many wires to resolve to a byte, must be <= 8");
            }

            byte value = 0;
            for (int i = 0; i < wires.Count; i++)
            {
                value |= (byte)(Convert.ToByte(wires[i].State ? 1 : 0) << i);
            }

            return value;
        }

        public static ushort ToUshort(this List<Wire> wires)
        {
            if (wires.Count > 16)
            {
                throw new ArgumentException("Too many wires to resolve to a ushort, must be <= 16");
            }

            ushort value = 0;
            for (int i = 0; i < wires.Count; i++)
            {
                value |= (ushort)(Convert.ToByte(wires[i].State ? 1 : 0) << i);
            }

            return value;
        }

        public static uint ToUint(this List<Wire> wires)
        {
            if (wires.Count > 32)
            {
                throw new ArgumentException("Too many wires to resolve to a uint, must be <= 32");
            }

            uint value = 0;
            for (int i = 0; i < wires.Count; i++)
            {
                value |= (uint)(Convert.ToByte(wires[i].State ? 1 : 0) << i);
            }

            return value;
        }

        public static string ToBinaryString(this Wire[] wires)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0b");

            for (int i = wires.Length - 1; i >= 0; i--)
            {
                sb.Append(wires[i].ToString());
            }

            return sb.ToString();
        }

        public static string ToBinaryString(this IList<Wire> wires)
        {
            return wires.ToArray().ToBinaryString();
        }

        #endregion
    }
}
