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
            pcls = 0x00;
            pchs = 0x80;
            dil = 0x80;
            x = 0xC0;
            sp = 0xBD;
            p = new StatusRegister(0x16);
            decodeLogic = new DecodeLogic();
            alu = new ALU();
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

            CpuMicroCode aluMicroCode = alu.Cycle(signalEdge);
            p.OverflowFlag = alu.OverflowFlag;
            p.CarryFlag = alu.CarryFlag;

            CpuMicroCode cpuMicroCode = decodeLogic.Cycle(signalEdge);
            ExecuteCycleMicroCode(aluMicroCode + cpuMicroCode);
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
                case MicroCodeInstruction.DecrementSP:
                    sp--;
                    break;
                case MicroCodeInstruction.IncrementSP:
                    sp++;
                    break;
                #endregion

                #region Status
                case MicroCodeInstruction.ClearCarry:
                    p.CarryFlag = false;
                    break;
                case MicroCodeInstruction.SetCarry:
                    p.CarryFlag = true;
                    break;
                case MicroCodeInstruction.ClearIRQ:
                    p.IrqFlag = false;
                    break;
                case MicroCodeInstruction.SetIRQ:
                    p.IrqFlag = true;
                    break;
                case MicroCodeInstruction.ClearDecimal:
                    p.DecimalFlag = false;
                    break;
                case MicroCodeInstruction.SetDecimal:
                    p.DecimalFlag = true;
                    break;
                case MicroCodeInstruction.ClearOverflow:
                    p.OverflowFlag = false;
                    break;
                case MicroCodeInstruction.TransferDataIntoP:
                    p.Value = (byte)(dil & 0b11011111);
                    break;
                #endregion

                #region IR
                case MicroCodeInstruction.LatchIRToData:
                    latchIREnable = true;
                    break;
                #endregion

                #region Registers
                case MicroCodeInstruction.LatchDILIntoA:
                    a = dil;
                    p.SetFlagsFromData(a);
                    break;
                case MicroCodeInstruction.LatchDILIntoX:
                    x = dil;
                    p.SetFlagsFromData(x);
                    break;
                case MicroCodeInstruction.LatchDILIntoY:
                    y = dil;
                    p.SetFlagsFromData(y);
                    break;
                case MicroCodeInstruction.IncrementA:
                    alu.B = a;
                    alu.ExecuteInstruction(instruction);
                    break;
                case MicroCodeInstruction.IncrementX:
                    alu.B = x;
                    alu.ExecuteInstruction(instruction);
                    break;
                case MicroCodeInstruction.IncrementY:
                    alu.B = y;
                    alu.ExecuteInstruction(instruction);
                    break;
                case MicroCodeInstruction.DecrementA:
                    alu.B = a;
                    alu.ExecuteInstruction(instruction);
                    break;
                case MicroCodeInstruction.DecrementX:
                    alu.B = x;
                    alu.ExecuteInstruction(instruction);
                    break;
                case MicroCodeInstruction.DecrementY:
                    alu.B = y;
                    alu.ExecuteInstruction(instruction);
                    break;
                case MicroCodeInstruction.TransferXToSP:
                    sp = x;
                    break;
                case MicroCodeInstruction.TransferSPToX:
                    x = sp;
                    p.SetFlagsFromData(x);
                    break;
                case MicroCodeInstruction.TransferAToX:
                    x = a;
                    p.SetFlagsFromData(x);
                    break;
                case MicroCodeInstruction.TransferXToA:
                    a = x;
                    p.SetFlagsFromData(a);
                    break;
                case MicroCodeInstruction.TransferAToY:
                    y = a;
                    p.SetFlagsFromData(y);
                    break;
                case MicroCodeInstruction.TransferYToA:
                    a = y;
                    p.SetFlagsFromData(a);
                    break;
                #endregion

                #region ALU
                case MicroCodeInstruction.TransferHoldToA:
                    a = alu.Hold;
                    p.SetFlagsFromData(a);
                    break;
                case MicroCodeInstruction.TransferHoldToX:
                    x = alu.Hold;
                    p.SetFlagsFromData(x);
                    break;
                case MicroCodeInstruction.TransferHoldToY:
                    y = alu.Hold;
                    p.SetFlagsFromData(y);
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
                case MicroCodeInstruction.TransferPCSToAddressBus:
                    abl = pcls;
                    abh = pchs;
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
                case MicroCodeInstruction.IncrementAB_NoCarry:
                    abl++;
                    break;
                case MicroCodeInstruction.IncrementABByX:
                    if (((int)abl + (int)x) > byte.MaxValue)
                    {
                        p.CarryFlag = true;
                    }    
                    abl = (byte)(abl + x);
                    break;
                case MicroCodeInstruction.IncrementABByY:
                    if (((int)abl + (int)y) > byte.MaxValue)
                    {
                        p.CarryFlag = true;
                    }
                    abl = (byte)(abl + y);
                    break;
                case MicroCodeInstruction.IncrementABByY_WithCarry:
                    if (((int)abl + (int)y) > byte.MaxValue)
                    {
                        p.CarryFlag = true;
                        abh++;
                    }
                    abl = (byte)(abl + y);
                    break;
                case MicroCodeInstruction.TransferSPToAB:
                    abh = 0x01;
                    abl = sp;
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
                case MicroCodeInstruction.LatchPIntoDOR:
                    dor = (byte)(p.Value | 0b00100000);
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
        private byte sp;
        private byte pcl;
        private byte pch;
        private byte pcls;
        private byte pchs;
        private byte abl;
        private byte abh;
        private byte dor;
        private byte dil;
        private ALU alu;
        private StatusRegister p;
        private DecodeLogic decodeLogic;
    }
}
