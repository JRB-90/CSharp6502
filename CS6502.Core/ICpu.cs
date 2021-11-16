namespace CS6502.Core
{
    /// <summary>
    /// Interface defining the behaviour of all 6502 cpu variants.
    /// </summary>
    public interface ICpu
    {
        string Name { get; }

        Wire IRQ_N { get; set; }

        Wire NMI_N { get; set; }

        Wire RES_N { get; set; }

        Wire RDY_N { get; set; }

        Wire RW_N { get; }

        Wire SYNC_N { get; }

        Wire PHI2 { get; set; }

        Wire PHI1O { get; }

        Wire PHI2O { get; }

        Bus AddressBus { get; set; }

        Bus DataBus { get; set; }
    }
}
