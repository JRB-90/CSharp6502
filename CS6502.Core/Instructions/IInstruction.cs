namespace CS6502.Core
{
    /// <summary>
    /// Interface to define the behaviour of all 6502 instructions.
    /// </summary>
    public interface IInstruction
    {
        string Name { get; }

        byte Opcode { get; }

        AddressingMode AddressingMode { get; }

        OperationType OperationType { get; }

        int CurrentCycle { get; }

        void Execute(CpuRegisters registers);
    }
}
