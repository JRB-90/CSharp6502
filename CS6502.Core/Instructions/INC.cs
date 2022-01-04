using System;

namespace CS6502.Core
{
    internal class INC : InstructionBase
    {
        public static INC CreateINC(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new INC(0xE6, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new INC(0xF6, addressingMode);

                case AddressingMode.Absolute:
                    return new INC(0xEE, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new INC(0xFE, addressingMode);

                default:
                    throw new ArgumentException($"INC does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status,
            bool wasPageBoundaryCrossed)
        {
            if (AddressingMode == AddressingMode.ZeroPage ||
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
                throw new ArgumentException($"INC does not support {AddressingMode.ToString()} addressing mode");
            }
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
                    if (AddressingMode == AddressingMode.ZeroPage)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.TransferZPDataToAB,
                                MicroCodeInstruction.SetToRead,
                                MicroCodeInstruction.IncrementPC
                            );
                    }
                    else if (AddressingMode == AddressingMode.ZeroPageX)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDataIntoDIL,
                                MicroCodeInstruction.SetToRead,
                                MicroCodeInstruction.IncrementABByX_NoCarry
                            );
                    }
                }
                else if (instructionCycle == startingCycle + 1)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDILIntoDOR,
                            MicroCodeInstruction.SetToWrite,
                            MicroCodeInstruction.INC
                        );
                }
                else if (instructionCycle == startingCycle + 3)
                {
                    IsInstructionComplete = true;

                    return 
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }
            else
            {
                if (instructionCycle == startingCycle + 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferHoldToDOR
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode Absolute(SignalEdge signalEdge, int instructionCycle)
        {
            int startingCycle = 3;
            if (AddressingMode == AddressingMode.AbsoluteX)
            {
                startingCycle = 4;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle)
                {
                    if (AddressingMode == AddressingMode.Absolute)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.SetToRead,
                                MicroCodeInstruction.TransferDILToPCHS,
                                MicroCodeInstruction.TransferPCSToAddressBus,
                                MicroCodeInstruction.IncrementPC
                            );
                    }
                    else if (AddressingMode == AddressingMode.AbsoluteX)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.SetToRead,
                                MicroCodeInstruction.LatchDILIntoDOR
                            );
                    }
                }
                else if (instructionCycle == startingCycle + 1)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.SetToWrite,
                            MicroCodeInstruction.INC
                        );
                }
                else if (instructionCycle == startingCycle + 3)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
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
                            MicroCodeInstruction.LatchDataIntoDIL
                        );
                }
                else if (instructionCycle == startingCycle + 2)
                {
                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.TransferHoldToDOR
                        );
                }
            }

            return new CpuMicroCode();
        }

        private INC(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "INC",
                opcode,
                addressingMode)
        {
        }
    }
}
