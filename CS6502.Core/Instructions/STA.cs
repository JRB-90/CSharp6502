using System;

namespace CS6502.Core
{
    internal class STA : InstructionBase
    {
        public static STA CreateSTA(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new STA(0x85, addressingMode);

                case AddressingMode.ZeroPageX:
                    return new STA(0x95, addressingMode);

                case AddressingMode.Absolute:
                    return new STA(0x8D, addressingMode);

                case AddressingMode.AbsoluteX:
                    return new STA(0x9D, addressingMode);

                case AddressingMode.AbsoluteY:
                    return new STA(0x99, addressingMode);

                case AddressingMode.XIndirect:
                    return new STA(0x81, addressingMode);

                case AddressingMode.IndirectY:
                    return new STA(0x91, addressingMode);

                default:
                    throw new ArgumentException($"STA does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status)
        {
            if (AddressingMode == AddressingMode.ZeroPage ||
                     AddressingMode == AddressingMode.ZeroPageX)
            {
                return ZeroPage(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.Absolute ||
                     AddressingMode == AddressingMode.AbsoluteX ||
                     AddressingMode == AddressingMode.AbsoluteY)
            {
                return Absolute(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.XIndirect ||
                     AddressingMode == AddressingMode.IndirectY)
            {
                return Indirect(signalEdge, instructionCycle);
            }
            else
            {
                throw new ArgumentException($"STA does not support {AddressingMode.ToString()} addressing mode");
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
                    CpuMicroCode cpuMicroCode =
                        new CpuMicroCode(
                           MicroCodeInstruction.LatchDILIntoDOR,
                           MicroCodeInstruction.SetToWrite
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
                            MicroCodeInstruction.LatchAIntoDOR,
                            MicroCodeInstruction.TransferPCToPCS
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode Absolute(SignalEdge signalEdge, int instructionCycle)
        {
            int startingCycle = 3;
            if (AddressingMode == AddressingMode.AbsoluteX ||
                AddressingMode == AddressingMode.AbsoluteY)
            {
                startingCycle = 4;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle)
                {
                    CpuMicroCode cpuMicroCode =
                        new CpuMicroCode(
                            MicroCodeInstruction.SetToWrite,
                            MicroCodeInstruction.LatchDILIntoDOR                            
                        );

                    if (AddressingMode == AddressingMode.Absolute)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.TransferDILToPCHS);
                        cpuMicroCode.Add(MicroCodeInstruction.TransferPCSToAddressBus);
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementPC);
                    }

                    return cpuMicroCode;
                }
                else if (instructionCycle == startingCycle + 1)
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
                            MicroCodeInstruction.LatchAIntoDOR
                        );
                }
            }

            return new CpuMicroCode();
        }

        private CpuMicroCode Indirect(SignalEdge signalEdge, int instructionCycle)
        {
            int startingCycle = 4;
            if (AddressingMode == AddressingMode.XIndirect ||
                AddressingMode == AddressingMode.IndirectY)
            {
                startingCycle = 5;
            }

            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycle == startingCycle)
                {
                    if (AddressingMode == AddressingMode.XIndirect)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.TransferDILToPCHS,
                                MicroCodeInstruction.TransferPCSToAddressBus,
                                MicroCodeInstruction.LatchDILIntoDOR,
                                MicroCodeInstruction.SetToWrite
                            );
                    }
                    else if (AddressingMode == AddressingMode.IndirectY)
                    {
                        return
                            new CpuMicroCode(
                                MicroCodeInstruction.LatchDILIntoDOR,
                                MicroCodeInstruction.SetToWrite
                            );
                    }
                }
                else if (instructionCycle == startingCycle + 1)
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
                            MicroCodeInstruction.LatchAIntoDOR
                        );
                }
            }

            return new CpuMicroCode();
        }

        private STA(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "STA",
                opcode,
                addressingMode)
        {
        }
    }
}
