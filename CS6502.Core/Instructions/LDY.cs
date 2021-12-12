using System;

namespace CS6502.Core
{
    internal class LDY : InstructionBase
    {
        public static LDY CreateLDY(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new LDY(0xA0, addressingMode);

                case AddressingMode.ZeroPage:
                    return new LDY(0xA4, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new LDY(0xB4, addressingMode);

                case AddressingMode.Absolute:
                    return new LDY(0xAC, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new LDY(0xBC, addressingMode);

                default:
                    throw new ArgumentException($"LDY does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(SignalEdge signalEdge, int instructionCycle)
        {
            if (AddressingMode == AddressingMode.Immediate)
            {
                return Immediate(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.ZeroPage ||
                     AddressingMode == AddressingMode.ZeroPageX)
            {
                return ZeroPage(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.Absolute ||
                     AddressingMode == AddressingMode.AbsoluteX)
            {
                return Absolute(signalEdge, instructionCycle);
            }
            else
            {
                throw new ArgumentException($"LDA does not support {AddressingMode.ToString()} addressing mode");
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
                            MicroCodeInstruction.LatchDILIntoY,
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
                    CpuMicroCode cpuMicroCode =
                        new CpuMicroCode(
                           MicroCodeInstruction.LatchDataIntoDIL,
                           MicroCodeInstruction.SetToRead
                       );

                    if (AddressingMode == AddressingMode.ZeroPage)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.TransferZPDataToAB);
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementPC);
                    }
                    else if (AddressingMode == AddressingMode.ZeroPageX)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementABByX);
                    }

                    return cpuMicroCode;
                }
                if (instructionCycle == startingCycle + 1)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDILIntoY
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
                            MicroCodeInstruction.LatchDataIntoDIL,
                            MicroCodeInstruction.LatchDILIntoY,
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

        private LDY(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "LDY",
                opcode,
                addressingMode)
        {
        }
    }
}
