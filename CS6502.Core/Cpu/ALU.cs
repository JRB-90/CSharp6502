using System;
using System.Collections.Generic;

namespace CS6502.Core
{
    /// <summary>
    /// Represents the ALU of a 6502 CPU.
    /// Does not emulate exactly the internals of the real ALU,
    /// but does function in a cycle accurate way from an external
    /// point of view.
    /// </summary>
    internal class ALU
    {
        public ALU()
        {
            instructionQueue = new Queue<MicroCodeInstruction>();
        }

        public byte B { get; set; }

        public byte A { get; set; }

        public byte Hold { get; private set; }

        public bool OverflowFlag { get; set; }

        public bool CarryFlag { get; set; }

        public void ExecuteInstruction(MicroCodeInstruction instruction)
        {
            instructionQueue.Enqueue(instruction);
        }

        public CpuMicroCode Cycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionQueue.Count > 0)
                {
                    MicroCodeInstruction instruction = instructionQueue.Dequeue();
                    switch (instruction)
                    {
                        case MicroCodeInstruction.IncrementA:
                            A = 1;
                            Hold = (byte)(B + A);
                            return new CpuMicroCode(MicroCodeInstruction.TransferHoldToA);

                        case MicroCodeInstruction.IncrementX:
                            A = 1;
                            Hold = (byte)(B + A);
                            return new CpuMicroCode(MicroCodeInstruction.TransferHoldToX);

                        case MicroCodeInstruction.IncrementY:
                            A = 1;
                            Hold = (byte)(B + A);
                            return new CpuMicroCode(MicroCodeInstruction.TransferHoldToY);

                        case MicroCodeInstruction.DecrementA:
                            A = 1;
                            Hold = (byte)(B - A);
                            return new CpuMicroCode(MicroCodeInstruction.TransferHoldToA);

                        case MicroCodeInstruction.DecrementX:
                            A = 1;
                            Hold = (byte)(B - A);
                            return new CpuMicroCode(MicroCodeInstruction.TransferHoldToX);

                        case MicroCodeInstruction.DecrementY:
                            A = 1;
                            Hold = (byte)(B - A);
                            return new CpuMicroCode(MicroCodeInstruction.TransferHoldToY);

                        case MicroCodeInstruction.INC:
                            // TODO
                            break;

                        case MicroCodeInstruction.DEC:
                            // TODO
                            break;

                        case MicroCodeInstruction.ADC:
                            int addRes = A + B + (CarryFlag ? 1 : 0);
                            if (addRes > byte.MaxValue)
                            {
                                CarryFlag = true;
                            }
                            else
                            {
                                CarryFlag = false;
                            }
                            Hold = (byte)addRes;
                            return new CpuMicroCode(MicroCodeInstruction.TransferHoldToA);

                        case MicroCodeInstruction.SBC:
                            int subRes = A - B - (CarryFlag ? 0 : 1);
                            if (subRes >= 0)
                            {
                                CarryFlag = true;
                            }
                            else
                            {
                                CarryFlag = false;
                            }
                            Hold = (byte)subRes;
                            return new CpuMicroCode(MicroCodeInstruction.TransferHoldToA);

                        default:
                            throw new InvalidOperationException($"Instruction [{instruction.ToString()}] not a supported ALU instruction");
                    }
                }

                return new CpuMicroCode();
            }
            else
            {
                // In the future I may revisit this and attempt
                // to make the internals of the ALU cycle accurate
                return new CpuMicroCode();
            }
        }

        private void SetFlags(ALUOperation operation)
        {
            switch (operation)
            {
                case ALUOperation.ADD:
                    if (((int)A + (int)B) > (int)byte.MaxValue)
                    {
                        CarryFlag = true;
                    }
                    if ((A & 0b10000000) == (B & 0b10000000))
                    {
                        if ((Hold & 0b10000000) != (B & 0b10000000))
                        {
                            OverflowFlag = true;
                        }
                    }
                    break;

                case ALUOperation.SUB:
                    if (((int)A - (int)B) < 0)
                    {
                        CarryFlag = true;
                    }
                    if ((A & 0b10000000) != (B & 0b10000000))
                    {
                        if ((Hold & 0b10000000) != (B & 0b10000000))
                        {
                            OverflowFlag = true;
                        }
                    }
                    break;

                default:
                    throw new InvalidOperationException($"ALU operation [{operation.ToString()}] not supported for flag setting");
            }
        }

        private Queue<MicroCodeInstruction> instructionQueue;
    }
}
