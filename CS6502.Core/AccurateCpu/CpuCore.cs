using System;

namespace CS6502.Core
{
    /// <summary>
    /// Class designed to replicate the internal register structure of a
    /// 6502 CPU, as in the diagrams below:
    /// http://www.baltissen.org/images/6502.png
    /// https://i.imgur.com/gJB7m.png
    /// </summary>
    internal class CpuCore
    {
        public CpuCore()
        {
            // TODO - Set initial states

            pcls = 0x00;
            pchs = 0x80;
            p = new StatusRegister();
            decodeLogic = new DecodeLogic();
        }

        public RWState RW => decodeLogic.RW;

        public EnableState Sync => decodeLogic.Sync;

        public ushort Address => (ushort)(abh << 8 | abl);

        public byte DataOut { get; private set; }

        public byte DataIn { get; set; }

        public string GetCurrentStateString(char delimiter)
        {
            return
                $"{(RW == RWState.Read ? 1 : 0)}{delimiter}" +
                $"{a.ToHexString()}{delimiter}" +
                $"{x.ToHexString()}{delimiter}" +
                $"{y.ToHexString()}{delimiter}" +
                $"{decodeLogic.IR.ToHexString()}{delimiter}" +
                $"{p.Value.ToHexString()}{delimiter}" +
                $"{sp.ToHexString()}{delimiter}" +
                $"{PC.ToHexString()}{delimiter}" +
                $"{Address.ToHexString()}{delimiter}" +
                $"{dl.ToHexString()}";
        }

        public CycleState GetCurrentCycleState(int cycleID)
        {
            return
                new CycleState(
                    cycleID,
                    Convert.ToByte(RW == RWState.Read),
                    a,
                    x,
                    y,
                    decodeLogic.IR,
                    p.Value,
                    sp,
                    PC,
                    Address,
                    dl
                );
        }

        public void Cycle(SignalEdge signalEdge)
        {
            if (latchIREnable)
            {
                decodeLogic.LatchIR(dl);
                latchIREnable = false;
            }
            CpuMicroCode cpuMicroCode = decodeLogic.Cycle(signalEdge);
            ExecuteCycleMicroCode(cpuMicroCode);
        }

        private void LatchData()
        {
            if (RW == RWState.Read)
            {
                dl = DataIn;
            }
            else if (RW == RWState.Write)
            {
                DataOut = dl;
            }
        }

        private void ExecuteCycleMicroCode(CpuMicroCode cpuMicroCode)
        {
            for (int i = 0; i < cpuMicroCode.MicroCode.Count; i++)
            {
                ExecuteMicroCodeInstruction(cpuMicroCode.MicroCode[i]);
            }
        }

        private void ExecuteMicroCodeInstruction(MicroCodeInstruction instruction)
        {
            switch (instruction)
            {
                #region PC
                case MicroCodeInstruction.TransferDataToPCLS:
                    pcls = dl;
                    break;
                case MicroCodeInstruction.TransferDataToPCHS:
                    pchs = dl;
                    break;
                case MicroCodeInstruction.TransferPCLToPCLS:
                    pcls = pcl;
                    break;
                case MicroCodeInstruction.TransferPCHToPCHS:
                    pchs = pch;
                    break;
                case MicroCodeInstruction.TransferPCToPCS:
                    pcls = pcl;
                    pchs = pch;
                    break;
                case MicroCodeInstruction.TransferPCLSToPCL:
                    pcl = pcls;
                    break;
                case MicroCodeInstruction.TransferPCHSToPCH:
                    pch = pchs;
                    break;
                case MicroCodeInstruction.TransferPCSToPC_NoIncrement:
                    pcl = pcls;
                    pch = pchs;
                    break;
                case MicroCodeInstruction.TransferPCSToPC_WithCarryIncrement:
                    if (pcls == byte.MaxValue)
                    {
                        pch = (byte)(pchs + 1);
                    }
                    else
                    {
                        pch = pchs;
                    }
                    pcl = (byte)(pcls + 1);
                    break;
                case MicroCodeInstruction.IncrementPC:
                    if (pcl == byte.MaxValue)
                    {
                        pch = (byte)(pch + 1);
                    }
                    pcl = (byte)(pcl + 1);
                    break;
                #endregion

                #region Address
                case MicroCodeInstruction.TransferPCToAddressBus:
                    abl = pcl;
                    abh = pch;
                    break;
                #endregion

                #region Data
                case MicroCodeInstruction.LatchDataBus:
                    LatchData();
                    break;
                #endregion

                #region IR
                case MicroCodeInstruction.LatchIRToData:
                    latchIREnable = true;
                    break;
                #endregion

                default:
                    throw new InvalidOperationException($"Micro Code Instruction [{instruction.ToString()}] not supported");
            }
        }

        private ushort PC => (ushort)(pch << 8 | pcl);

        private bool latchIREnable;
        private byte a;
        private byte x;
        private byte y;
        private byte ir;
        private byte sp;
        private byte pcl;
        private byte pch;
        private byte pcls;
        private byte pchs;
        private byte abl;
        private byte abh;
        private byte dl;
        private byte hold;
        private ALU alu;
        private StatusRegister p;
        private DecodeLogic decodeLogic;
    }
}
