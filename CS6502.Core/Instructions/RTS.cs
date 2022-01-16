using System;

namespace CS6502.Core
{
    internal class RTS : InstructionBase
    {
        public RTS()
          :
            base(
                "RTS",
                0x60,
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
                            MicroCodeInstruction.TransferSPToAB,
                            MicroCodeInstruction.IncrementPC
                        );
                }
                if (instructionCycle == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.IncrementAB_NoCarry
                        );
                }
                if (instructionCycle == 4)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferDILToPCLS,
                            MicroCodeInstruction.IncrementAB_NoCarry,
                            MicroCodeInstruction.TransferABLToSP,
                            MicroCodeInstruction.SetNAFlag
                        );
                }
                if (instructionCycle == 5)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferDILToPCHS,
                            MicroCodeInstruction.IncrementAB_NoCarry,
                            MicroCodeInstruction.TransferPCSToPC_NoIncrement,
                            MicroCodeInstruction.TransferPCToAddressBus,
                            MicroCodeInstruction.ClearNAFlag
                        );
                }
                if (instructionCycle == 6)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.IncrementPC,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }
    }
}
