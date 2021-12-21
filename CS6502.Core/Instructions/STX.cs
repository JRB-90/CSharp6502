using System;

namespace CS6502.Core
{
    internal class STX : InstructionBase
    {
        public static STX CreateSTX(AddressingMode addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingMode.ZeroPage:
                    return new STX(0x86, addressingMode);

                case AddressingMode.ZeroPageY:
                    return new STX(0x96, addressingMode);

                case AddressingMode.Absolute:
                    return new STX(0x8E, addressingMode);

                default:
                    throw new ArgumentException($"STX does not support {addressingMode.ToString()} addressing mode");
            }
        }

        public override CpuMicroCode Execute(
            SignalEdge signalEdge,
            int instructionCycle,
            StatusRegister status)
        {
            if (AddressingMode == AddressingMode.ZeroPage ||
                AddressingMode == AddressingMode.ZeroPageY)
            {
                return ZeroPage(signalEdge, instructionCycle);
            }
            else if (AddressingMode == AddressingMode.Absolute)
            {
                return Absolute(signalEdge, instructionCycle);
            }
            else
            {
                throw new ArgumentException($"STX does not support {AddressingMode.ToString()} addressing mode");
            }
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

                           MicroCodeInstruction.LatchDILIntoDOR,
                           MicroCodeInstruction.SetToWrite
                       );

                    if (AddressingMode == AddressingMode.ZeroPage)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.TransferZPDataToAB);
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementPC);
                    }
                    else if (AddressingMode == AddressingMode.ZeroPageY)
                    {
                        cpuMicroCode.Add(MicroCodeInstruction.IncrementABByY);
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
                            MicroCodeInstruction.LatchXIntoDOR,
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
                            MicroCodeInstruction.SetToWrite,
                            MicroCodeInstruction.LatchDILIntoDOR,
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
                            MicroCodeInstruction.LatchXIntoDOR
                        );
                }
            }

            return new CpuMicroCode();
        }

        private STX(
            byte opcode,
            AddressingMode addressingMode)
          :
            base(
                "STX",
                opcode,
                addressingMode)
        {
        }
    }
}
