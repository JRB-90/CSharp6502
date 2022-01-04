namespace CS6502.Core
{
    /// <summary>
    /// Abstract base class to provide shared functionality amongst all instructions.
    /// </summary>
    internal abstract class InstructionBase : IInstruction
    {
        public InstructionBase(
            string name,
            byte opcode,
            AddressingMode addressingMode)
        {
            Name = name;
            Opcode = opcode;
            AddressingMode = addressingMode;
            IsInstructionComplete = false;
        }

        public string Name { get; }

        public byte Opcode { get; }

        public AddressingMode AddressingMode { get; }

        public bool IsInstructionComplete { get; protected set; }

        public virtual CpuMicroCode Execute(
            SignalEdge signalEdge, 
            int instructionCycle,
            StatusRegister status,
            bool wasPageBoundaryCrossed)
        {
            IsInstructionComplete = true;

            return new CpuMicroCode();
        }

        public override string ToString()
        {
            return $"{Name} - {Opcode.ToHexString()} - {AddressingMode.ToString()}";
        }
    }
}
