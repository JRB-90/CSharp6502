using CS6502.Core;

namespace CS6502.UIConsole.Models
{
    internal class MemoryAddressModel
    {
        public MemoryAddressModel(
            ushort address,
            byte value)
        {
            Address = address;
            Value = value;
        }

        public ushort Address { get; }

        public byte Value { get; }

        public string AddressHexString => Address.ToHexString();

        public string ValueHexString => Value.ToHexString();
    }
}
