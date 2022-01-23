namespace CS6502.Core
{
    internal class RTI : InstructionBase
    {
        public RTI()
          :
            base(
                "RTI",
                0x40,
                AddressingMode.Implied)
        {
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status,
            bool wasPageBoundaryCrossed)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.IncrementPC,
                            MicroCodeInstruction.TransferSPToAB
                        );
                }
                else if (instructionCycle == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.IncrementAB_NoCarry
                        );
                }
                else if (instructionCycle == 4)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferDataIntoP,
                            MicroCodeInstruction.IncrementAB_NoCarry
                        );
                }
                else if (instructionCycle == 5)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferDILToPCLS,
                            MicroCodeInstruction.IncrementAB_NoCarry,
                            MicroCodeInstruction.IncrementSP,
                            MicroCodeInstruction.IncrementSP,
                            MicroCodeInstruction.IncrementSP
                        );
                }
                else if (instructionCycle == 6)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferDILToPCHS
                        );
                }
            }

            return new CpuMicroCode();
        }
    }
}
