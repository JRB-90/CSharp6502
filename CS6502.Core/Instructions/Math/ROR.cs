using System;

namespace CS6502.Core
{
    internal class ROR : ShiftInstructionBase
    {
        public static ROR CreateROR(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new ROR(0x6A, addressingMode);

                case AddressingMode.ZeroPage:
                    return new ROR(0x66, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new ROR(0x76, addressingMode);

                case AddressingMode.Absolute:
                    return new ROR(0x6E, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new ROR(0x7E, addressingMode);

                default:
                    throw new ArgumentException($"ROR does not support {addressingMode.ToString()} addressing mode");
            }
        }

        protected override CpuMicroCode Immediate(SignalEdge signalEdge, int instructionCycle)
        {
            IsInstructionComplete = true;

            return
                new CpuMicroCode(
                    MicroCodeInstruction.ROR_A,
                    MicroCodeInstruction.ShiftLowABitIntoCarry,
                    MicroCodeInstruction.TransferPCToPCS
                );
        }

        protected override CpuMicroCode ZeroPage(SignalEdge signalEdge, int instructionCycle)
        {
            int startingCycle = 2;
            if (AddressingMode == AddressingMode.ZeroPageX)
            {
                startingCycle = 3;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle + 1)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDILIntoDOR,
                            MicroCodeInstruction.SetToWrite,
                            MicroCodeInstruction.ROR,
                            MicroCodeInstruction.ShiftLowDILBitIntoCarry
                        );
                }
            }

            return
                base.ZeroPage(
                    signalEdge,
                    instructionCycle
                );
        }

        protected override CpuMicroCode Absolute(
            SignalEdge signalEdge, 
            int instructionCycle,
            bool wasPageBoundaryCrossed)
        {
            int startingCycle = 3;
            if (AddressingMode == AddressingMode.AbsoluteX)
            {
                startingCycle = 4;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle + 1)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.SetToWrite,
                            MicroCodeInstruction.ROR,
                            MicroCodeInstruction.ShiftLowDILBitIntoCarry,
                            MicroCodeInstruction.LatchDILIntoDOR
                        );
                }
            }

            return
                base.Absolute(
                    signalEdge,
                    instructionCycle,
                    wasPageBoundaryCrossed
                );
        }

        private ROR(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "ROR",
                opcode,
                addressingMode,
                MicroCodeInstruction.ROR,
                MicroCodeInstruction.ROR_A)
        {
        }
    }
}
