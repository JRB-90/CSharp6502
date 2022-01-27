using System;

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
            if (AddressingMode == AddressingMode.Immediate)
            {
                return Immediate(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.ZeroPage ||
                     AddressingMode == AddressingMode.ZeroPageX ||
                     AddressingMode == AddressingMode.ZeroPageY)
            {
                return 
                    ZeroPage(
                        signalEdge, 
                        instructionCycle
                    );
            }
            else if (AddressingMode == AddressingMode.Absolute ||
                     AddressingMode == AddressingMode.AbsoluteX ||
                     AddressingMode == AddressingMode.AbsoluteY)
            {
                return 
                    Absolute(
                        signalEdge, 
                        instructionCycle, 
                        wasPageBoundaryCrossed
                    );
            }
            else if (AddressingMode == AddressingMode.XIndirect ||
                     AddressingMode == AddressingMode.IndirectY)
            {
                return 
                    Indirect(
                        signalEdge, 
                        instructionCycle, 
                        wasPageBoundaryCrossed
                    );
            }
            else if (AddressingMode == AddressingMode.Relative)
            {
                return 
                    Relative(
                        signalEdge, 
                        instructionCycle, 
                        wasPageBoundaryCrossed, 
                        status
                    );
            }
            else
            {
                throw new ArgumentException(
                    $" Addressing Mode: {AddressingMode.ToString()} not recognised"
                );
            }
        }

        protected virtual CpuMicroCode Immediate(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return new CpuMicroCode();
        }

        protected virtual CpuMicroCode ZeroPage(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return new CpuMicroCode();
        }

        protected virtual CpuMicroCode Absolute(
            SignalEdge signalEdge,
            int instructionCycle,
            bool wasPageBoundaryCrossed)
        {
            IsInstructionComplete = true;

            return new CpuMicroCode();
        }

        protected virtual CpuMicroCode Indirect(
            SignalEdge signalEdge,
            int instructionCycle,
            bool wasPageBoundaryCrossed)
        {
            IsInstructionComplete = true;

            return new CpuMicroCode();
        }

        protected virtual CpuMicroCode Relative(
            SignalEdge signalEdge,
            int instructionCycle,
            bool wasPageBoundaryCrossed,
            StatusRegister status)
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
