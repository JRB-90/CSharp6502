using System;

namespace CS6502.Core
{
    internal class CPX : InstructionBase
    {
        public static CPX CreateCPX(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new CPX(0xE0, addressingMode);

                case AddressingMode.ZeroPage:
                    return new CPX(0xE4, addressingMode);

                case AddressingMode.Absolute:
                    return new CPX(0xEC, addressingMode);

                default:
                    throw new ArgumentException($"CPX does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status)
        {
            if (AddressingMode == AddressingMode.Immediate)
            {
                return Immediate(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.ZeroPage)
            {
                return ZeroPage(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.Absolute)
            {
                return Absolute(signalEdge, instructionCycle);
            }
            else
            {
                throw new ArgumentException($"CPX does not support {AddressingMode.ToString()} addressing mode");
            }
        }

        private CpuMicroCode Immediate(SignalEdge signalEdge, int instructionCycle)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                IsInstructionComplete = true;

                if (instructionCycle == 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.CMP_X,
                            MicroCodeInstruction.IncrementPC,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode ZeroPage(SignalEdge signalEdge, int instructionCycle)
        {
            int startingCycle = 2;
            if (AddressingMode == AddressingMode.ZeroPageX)
            {
                startingCycle = 3;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferZPDataToAB,
                            MicroCodeInstruction.LatchDataIntoDIL,
                            MicroCodeInstruction.SetToRead,
                            MicroCodeInstruction.IncrementPC
                        );
                }
                if (instructionCycle == startingCycle + 1)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.CMP_X,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }
            else
            {
                if (instructionCycle == startingCycle)
                {
                    return
                        new CpuMicroCode(
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
                            MicroCodeInstruction.CMP_X,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }
            else
            {
                if (instructionCycle == 3)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDataIntoDIL
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CPX(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "CPX",
                opcode,
                addressingMode)
        {
        }
    }
}
