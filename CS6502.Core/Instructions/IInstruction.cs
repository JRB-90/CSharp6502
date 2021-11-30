namespace CS6502.Core
{
    /// <summary>
    /// Interface to define the behaviour of all 6502 instructions.
    /// </summary>
    internal interface IInstruction
    {
        string Name { get; }

        byte Opcode { get; }

        AddressingMode AddressingMode { get; }

        OperationType OperationType { get; }

        bool IsInstructionComplete { get; }

        CpuMicroCode Execute(int instructionCycle);
    }
}
