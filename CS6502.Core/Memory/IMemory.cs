namespace CS6502.Core
{
    /// <summary>
    /// Interface for defining the behaviour of memory chips.
    /// </summary>
    public interface IMemory
    {
        /// <summary>
        /// Size of the memory in bytes.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// /// <summary>
        /// Number of lines on the address bus, representing the bit size of the
        /// accepted address.
        /// </summary>
        int AddressWidth { get; }

        /// <summary>
        /// Number of lines on the data bus, representing the bit size of the
        /// accepted data.
        /// </summary>
        int DataWidth { get; }

        /// <summary>
        /// The chip select pin.
        /// </summary>
        Wire CS_N { get; set; }

        /// <summary>
        /// Address bus.
        /// </summary>
        Bus AddressBus { get; set; }

        /// <summary>
        /// Data bus.
        /// </summary>
        Bus DataBus { get; set; }

        /// <summary>
        /// Internal stored memory data.
        /// </summary>
        byte[] Data { get; }
    }
}
