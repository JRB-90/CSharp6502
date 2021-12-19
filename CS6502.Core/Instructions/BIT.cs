using System;

namespace CS6502.Core
{
    internal class BIT : InstructionBase
    {
        public static BIT CreateBIT(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new BIT(0x24, addressingMode);

                case AddressingMode.Absolute:
                    return new BIT(0x2C, addressingMode);

                default:
                    throw new ArgumentException($"BIT does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            if (AddressingMode == AddressingMode.ZeroPage)
            {
                return ZeroPage(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.Absolute)
            {
                return Absolute(signalEdge, instructionCycle);
            }
            else
            {
                throw new ArgumentException($"BIT does not support {AddressingMode.ToString()} addressing mode");
            }
        }

        private CpuMicroCode ZeroPage(SignalEdge signalEdge, int instructionCycle)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferZPDataToAB,
                            MicroCodeInstruction.LatchDataIntoDIL,
                            MicroCodeInstruction.SetToRead,
                            MicroCodeInstruction.IncrementPC
                        );
                }
                else if (instructionCycle == 3)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.BIT,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode Absolute(SignalEdge signalEdge, int instructionCycle)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.SetToRead,
                            MicroCodeInstruction.TransferDILToPCHS,
                            MicroCodeInstruction.TransferPCSToAddressBus,
                            MicroCodeInstruction.IncrementPC
                        );
                }
                else if (instructionCycle == 4)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.BIT,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }

        private BIT(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "BIT",
                opcode,
                addressingMode)
        {
        }
    }
}
