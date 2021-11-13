namespace CS6502.Core
{
    /// <summary>
    /// Represents a continous span of memory addresses.
    /// </summary>
    public class AddressSpace
    {
        public AddressSpace(uint startAddress, uint endAddress)
        {
            StartAddress = startAddress;
            EndAddress = endAddress;
        }

        public uint StartAddress { get; }

        public uint EndAddress { get; }

        public bool IsInAddressSpace(uint address)
        {
            if (address >= StartAddress &&
                address <= EndAddress)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
