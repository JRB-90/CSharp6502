namespace CS6502.Core
{
    /// <summary>
    /// Interface defining the behaviour of all 6502 cpu variants.
    /// </summary>
    public interface ICpu
    {
        Wire IRQ_N { get; set; }

        Wire NMI_N { get; set; }

        Wire RES_N { get; set; }

        Wire RW_N { get; set; }

        Wire SYNC_N { get; set; }

        Wire RDY_N { get; set; }

        Wire PHI2 { get; set; }

        Wire PHI1O { get; set; }

        Wire PHI2O { get; set; }

        Bus AddressBus { get; set; }

        Bus DataBus { get; set; }

        CpuRegisters Registers { get; }
    }
}
