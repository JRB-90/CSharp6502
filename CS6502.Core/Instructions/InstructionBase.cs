using System;

namespace CS6502.Core
{
    /// <summary>
    /// Abstract base class to provide shared functionality amongst all instructions.
    /// </summary>
    public abstract class InstructionBase : IInstruction
    {
        public InstructionBase(
            string name,
            byte opcode,
            AddressingMode addressingMode,
            OperationType operationType)
        {
            Name = name;
            Opcode = opcode;
            AddressingMode = addressingMode;
            OperationType = operationType;
            CurrentCycle = 0;
        }

        public string Name { get; }

        public byte Opcode { get; }

        public AddressingMode AddressingMode { get; }

        public OperationType OperationType { get; }

        public int CurrentCycle { get; }

        public virtual void Execute(CpuRegisters registers)
        {
        }

        public override string ToString()
        {
            return $"{Name} - {Opcode.ToHexString()} - {AddressingMode.ToString()}";
        }
    }
}
