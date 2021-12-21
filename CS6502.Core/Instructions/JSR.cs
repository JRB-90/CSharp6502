using System;

namespace CS6502.Core
{
    internal class JSR : InstructionBase
    {
        public JSR()
          :
            base(
                "JSR",
                0x20,
                AddressingMode.Implied)
        {
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.IncrementPC,
                            MicroCodeInstruction.LatchDataIntoDIL,
                            MicroCodeInstruction.TransferDILToPCLS,
                            MicroCodeInstruction.TransferSPToAB,
                            MicroCodeInstruction.TransferSPIntoPCHS,
                            MicroCodeInstruction.LatchDILIntoSP
                        );
                }
                if (instructionCycle == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDILIntoDOR,
                            MicroCodeInstruction.SetToWrite
                        );
                }
                if (instructionCycle == 4)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.DecrementPCHS,
                            MicroCodeInstruction.TransferPCHSToABL,
                            MicroCodeInstruction.LatchDILIntoDOR
                        );
                }
                if (instructionCycle == 5)
                {
                    return 
                        new CpuMicroCode(
                            MicroCodeInstruction.SetToRead,
                            MicroCodeInstruction.TransferPCToAddressBus
                        );
                }
                if (instructionCycle == 6)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.DecrementPCHS,
                            MicroCodeInstruction.TransferPCHSToSP,
                            MicroCodeInstruction.TransferDILToPCHS
                        );
                }
            }
            else
            {
                if (instructionCycle == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchPCHIntoDOR
                        );
                }
                if (instructionCycle == 4)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchPCLIntoDOR
                        );
                }
                if (instructionCycle == 5)
                {
                    return new CpuMicroCode();
                }
            }

            return new CpuMicroCode();
        }
    }
}
