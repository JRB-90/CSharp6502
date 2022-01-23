using System;

namespace CS6502.Core
{
    internal class LDX : InstructionBase
    {
        public static LDX CreateLDX(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.Immediate:
                    return new LDX(0xA2, addressingMode);

                case AddressingMode.ZeroPage:
                    return new LDX(0xA6, addressingMode);

                case AddressingMode.ZeroPageY:
                    return new LDX(0xB6, addressingMode);

                case AddressingMode.Absolute:
                    return new LDX(0xAE, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new LDX(0xBE, addressingMode);

                default:
                    throw new ArgumentException($"LDX does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(
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
                     AddressingMode == AddressingMode.ZeroPageY)
            {
                return ZeroPage(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.Absolute ||
                     AddressingMode == AddressingMode.AbsoluteY)
            {
                return Absolute(signalEdge, instructionCycle, wasPageBoundaryCrossed);
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
                            MicroCodeInstruction.LatchDILIntoX,
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
            if (AddressingMode == AddressingMode.ZeroPageY)
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
                    else if (AddressingMode == AddressingMode.ZeroPageY)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementABByY_NoCarry);
                    }

                    return cpuMicroCode;
                }
                if (instructionCycle == startingCycle + 1)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDILIntoX
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

        private CpuMicroCode Absolute(
            SignalEdge signalEdge, 
            int instructionCycle,
            bool wasPageBoundaryCrossed)
        {
            int startingCycle = 3;
            if (AddressingMode == AddressingMode.AbsoluteY)
            {
                startingCycle = 4;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle)
                {
                    CpuMicroCode cpuMicroCode =
                        new CpuMicroCode(
                            MicroCodeInstruction.SetToRead
                        );

                    if (AddressingMode == AddressingMode.Absolute)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.TransferDILToPCHS);
                        cpuMicroCode.Add(MicroCodeInstruction.TransferPCSToAddressBus);
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementPC);
                    }

                    if (AddressingMode == AddressingMode.AbsoluteY)
                    {
                        if (wasPageBoundaryCrossed)
                        {
                            cpuMicroCode.Add(MicroCodeInstruction.IncrementABH);
                            cpuMicroCode.Add(MicroCodeInstruction.ClearPageBoundaryCrossed);
                        }
                        else
                        {
                            IsInstructionComplete = true;
                            cpuMicroCode.Add(MicroCodeInstruction.LatchDataIntoDIL);
                            cpuMicroCode.Add(MicroCodeInstruction.LatchDILIntoX);
                            cpuMicroCode.Add(MicroCodeInstruction.TransferPCToPCS);
                        }
                    }

                    return cpuMicroCode;
                }
                else if (instructionCycle == startingCycle + 1)
                {
                    IsInstructionComplete = true;

                    return
                        new CpuMicroCode(
                            MicroCodeInstruction.LatchDataIntoDIL,
                            MicroCodeInstruction.LatchDILIntoX,
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
            }

            return new CpuMicroCode();
        }

        private LDX(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "LDX",
                opcode,
                addressingMode)
        {
        }
    }
}
