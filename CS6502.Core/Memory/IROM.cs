namespace CS6502.Core
{
    /// <summary>
    /// Interface to define behaviour of Read Only Memory (ROM).
    /// </summary>
    public interface IROM : IMemory
    {
        /// <summary>
        /// Output enable pin, 
        /// </summary>
        Wire OE_N { get; set; }
    }
}
