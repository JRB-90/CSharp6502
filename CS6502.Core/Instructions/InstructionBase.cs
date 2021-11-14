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
            AddressingMode addressingMode)
        {
            Name = name;
            Opcode = opcode;
            AddressingMode = addressingMode;
            CurrentCycle = 0;
        }

        public string Name { get; }

        public byte Opcode { get; }

        public AddressingMode AddressingMode { get; }

        public int CurrentCycle { get; }

        public void Cycle()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
