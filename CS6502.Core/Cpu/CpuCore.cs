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

        public RWState RW { get; private set; }

        public EnableState Sync => decodeLogic.Sync;

        public ushort Address => (ushort)(abh << 8 | abl);

        public byte DataOut => dor;

        public byte DataIn
        {
            get => dil;
            set
            {
                if (RW == RWState.Read)
                {
                    dil = value;
                }
            }
        }

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
                $"{Data.ToHexString()}";
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
                    Data
                );
        }

        public void Cycle(SignalEdge signalEdge)
        {
            if (latchIREnable)
            {
                decodeLogic.LatchIR(dil);
                latchIREnable = false;
            }
            CpuMicroCode cpuMicroCode = decodeLogic.Cycle(signalEdge);
            ExecuteCycleMicroCode(cpuMicroCode);
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
                #region CPU State
                case MicroCodeInstruction.SetToRead:
                    RW = RWState.Read;
                    break;
                case MicroCodeInstruction.SetToWrite:
                    RW = RWState.Write;
                    break;
                #endregion

                // Status

                #region IR
                case MicroCodeInstruction.LatchIRToData:
                    latchIREnable = true;
                    break;
                #endregion

                #region Registers
                case MicroCodeInstruction.LatchDILIntoA:
                    a = dil;
                    break;
                case MicroCodeInstruction.LatchDILIntoX:
                    x = dil;
                    break;
                case MicroCodeInstruction.LatchDILIntoY:
                    y = dil;
                    break;
                case MicroCodeInstruction.IncrementA:
                    if (a == byte.MaxValue)
                    {
                        p.CarryFlag = true;
                    }
                    a++;
                    break;
                case MicroCodeInstruction.IncrementX:
                    if (x == byte.MaxValue)
                    {
                        p.CarryFlag = true;
                    }
                    x++;
                    break;
                case MicroCodeInstruction.IncrementY:
                    if (y == byte.MaxValue)
                    {
                        p.CarryFlag = true;
                    }
                    y++;
                    break;
                #endregion

                #region PC
                case MicroCodeInstruction.TransferDILToPCLS:
                    pcls = dil;
                    break;
                case MicroCodeInstruction.TransferDILToPCHS:
                    pchs = dil;
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
                case MicroCodeInstruction.TransferZPDataToAB:
                    abl = dil;
                    abh = 0x00;
                    break;
                case MicroCodeInstruction.TransferDILToABL:
                    abl = dil;
                    break;
                case MicroCodeInstruction.TransferDILToABH:
                    abh = dil;
                    break;
                #endregion

                #region Data
                case MicroCodeInstruction.LatchDataIntoDIL:
                    dil = DataIn;
                    break;
                case MicroCodeInstruction.LatchDILIntoDOR:
                    dor = dil;
                    break;
                case MicroCodeInstruction.LatchAIntoDOR:
                    dor = a;
                    break;
                case MicroCodeInstruction.LatchXIntoDOR:
                    dor = x;
                    break;
                case MicroCodeInstruction.LatchYIntoDOR:
                    dor = y;
                    break;
                #endregion

                default:
                    throw new InvalidOperationException($"Micro Code Instruction [{instruction.ToString()}] not supported");
            }
        }

        private ushort PC => (ushort)(pch << 8 | pcl);
        private byte Data => RW == RWState.Read ? dil : dor;

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
        private byte dor;
        private byte dil;
        private byte hold;
        private ALU alu;
        private StatusRegister p;
        private DecodeLogic decodeLogic;
    }
}
