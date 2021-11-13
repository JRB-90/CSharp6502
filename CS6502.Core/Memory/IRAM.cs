namespace CS6502.Core
{
    /// <summary>
    /// Interface to define behaviour of Random Access Memory (RAM).
    /// </summary>
    public interface IRAM : IMemory
    {
        /// <summary>
        /// Output enable pin. 
        /// </summary>
        Wire OE_N { get; set; }

        /// <summary>
        /// Write enable pin. 
        /// Note: Active low for write, active high for read.
        /// </summary>
        Wire RW_N { get; set; }
    }
}
