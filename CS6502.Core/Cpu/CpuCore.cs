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
            branchShift = 0x00;
            pcls = 0x00;
            pchs = 0x80;
            dil = 0x80;
            x = 0xC0;
            sp = 0xBD;
            p = new StatusRegister(0x16);
            alu = new ALU();
            decodeLogic = new DecodeLogic();
            interuptControl = new InteruptControl();
        }

        public InteruptControl InteruptControl => interuptControl;

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

            interuptControl.Cycle();
            CpuMicroCode aluMicroCode = alu.Cycle(signalEdge);
            p.CarryFlag = alu.CarryFlag;
            p.OverflowFlag = alu.OverflowFlag;

            CpuMicroCode cpuMicroCode = 
                decodeLogic.Cycle(
                    signalEdge, 
                    p, 
                    interuptControl
                );

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
                case MicroCodeInstruction.TransferSPIntoPCHS:
                    pchs = sp;
                    break;
                case MicroCodeInstruction.BRK:
                    break;
                #endregion

                #region Status
                case MicroCodeInstruction.ClearCarry:
                    p.CarryFlag = false;
                    alu.CarryFlag = false;
                    break;
                case MicroCodeInstruction.SetCarry:
                    p.CarryFlag = true;
                    alu.CarryFlag = true;
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
                    alu.OverflowFlag = false;
                    break;
                case MicroCodeInstruction.ClearZero:
                    p.ZeroFlag = false;
                    break;
                case MicroCodeInstruction.SetZero:
                    p.ZeroFlag = true;
                    break;
                case MicroCodeInstruction.ClearNegative:
                    p.NegativeFlag = false;
                    break;
                case MicroCodeInstruction.SetNegative:
                    p.NegativeFlag = true;
                    break;
                case MicroCodeInstruction.TransferDataIntoP:
                    p.Value = (byte)(dil & 0b11011111);
                    break;
                case MicroCodeInstruction.ShiftLowABitIntoCarry:
                    p.CarryFlag = Convert.ToBoolean(a & 0b00000001);
                    alu.CarryFlag = p.CarryFlag;
                    break;
                case MicroCodeInstruction.ShiftHighABitIntoCarry:
                    p.CarryFlag = Convert.ToBoolean(a & 0b10000000);
                    alu.CarryFlag = p.CarryFlag;
                    break;
                case MicroCodeInstruction.ShiftLowDILBitIntoCarry:
                    p.CarryFlag = Convert.ToBoolean(dil & 0b00000001);
                    alu.CarryFlag = p.CarryFlag;
                    break;
                case MicroCodeInstruction.ShiftHighDILBitIntoCarry:
                    p.CarryFlag = Convert.ToBoolean(dil & 0b10000000);
                    alu.CarryFlag = p.CarryFlag;
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
                case MicroCodeInstruction.UpdateFlagsOnHold:
                    p.SetFlagsFromData(alu.Hold);
                    break;
                case MicroCodeInstruction.TransferHoldToDOR:
                    dor = alu.Hold;
                    break;
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
                case MicroCodeInstruction.IncrementA:
                    alu.B = a;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.IncrementX:
                    alu.B = x;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.IncrementY:
                    alu.B = y;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.DecrementA:
                    alu.B = a;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.DecrementX:
                    alu.B = x;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.DecrementY:
                    alu.B = y;
                    alu.ExecuteInstruction(instruction, p);
                    break;

                case MicroCodeInstruction.INC:
                    alu.B = dil;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.DEC:
                    alu.B = dil;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.ADC:
                    alu.A = a;
                    alu.B = dil;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.SBC:
                    alu.A = a;
                    alu.B = dil;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.AND:
                    alu.A = a;
                    alu.B = dil;
                    p.SetFlagsFromData(dil);
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.ORA:
                    alu.A = a;
                    alu.B = dil;
                    p.SetFlagsFromData(dil);
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.EOR:
                    alu.A = a;
                    alu.B = dil;
                    p.SetFlagsFromData(dil);
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.ASL:
                    alu.B = dil;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.ASL_A:
                    alu.B = a;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.LSR:
                    alu.B = dil;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.LSR_A:
                    alu.B = a;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.ROL:
                    alu.B = dil;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.ROL_A:
                    alu.B = a;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.ROR:
                    alu.B = dil;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.ROR_A:
                    alu.B = a;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.BIT:
                    p.NegativeFlag = Convert.ToBoolean(dil & 0b10000000);
                    p.OverflowFlag = Convert.ToBoolean(dil & 0b01000000);
                    p.ZeroFlag = (dil == 0);
                    alu.OverflowFlag = p.OverflowFlag;
                    alu.A = a;
                    alu.B = dil;
                    alu.ExecuteInstruction(instruction, p);
                    break;
                case MicroCodeInstruction.CMP_A:
                    alu.A = a;
                    alu.B = dil;
                    alu.ExecuteInstruction(MicroCodeInstruction.CMP, p);
                    break;
                case MicroCodeInstruction.CMP_X:
                    alu.A = x;
                    alu.B = dil;
                    alu.ExecuteInstruction(MicroCodeInstruction.CMP, p);
                    break;
                case MicroCodeInstruction.CMP_Y:
                    alu.A = y;
                    alu.B = dil;
                    alu.ExecuteInstruction(MicroCodeInstruction.CMP, p);
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
                case MicroCodeInstruction.TransferPCHSToSP:
                    sp = pchs;
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
                case MicroCodeInstruction.IncrementPCLS:
                    pcls++;
                    break;
                case MicroCodeInstruction.IncrementPCHS:
                    pchs++;
                    break;
                case MicroCodeInstruction.DecrementPCLS:
                    pcls--;
                    break;
                case MicroCodeInstruction.DecrementPCHS:
                    pchs--;
                    break;
                #endregion

                #region Address
                case MicroCodeInstruction.TransferIrqVecToAB:
                    abl = 0xFE;
                    abh = 0xFF;
                    break;
                case MicroCodeInstruction.TransferPCToAddressBus:
                    abl = pcl;
                    abh = pch;
                    break;
                case MicroCodeInstruction.TransferPCSToAddressBus:
                    abl = pcls;
                    abh = pchs;
                    break;
                case MicroCodeInstruction.TransferPCHSToABL:
                    abl = pchs;
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
                case MicroCodeInstruction.DecrementAB_NoCarry:
                    abl--;
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
                case MicroCodeInstruction.TransferABLToSP:
                    sp = abl;
                    break;
                case MicroCodeInstruction.LatchBranchShift:
                    branchShift = (sbyte)dil;
                    break;
                case MicroCodeInstruction.Branch:
                    ushort newPC = (ushort)((int)PC + branchShift);
                    pcl = (byte)(newPC & 0x00FF);
                    pch = (byte)(newPC >> 8);
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
                case MicroCodeInstruction.LatchPCLIntoDOR:
                    dor = pcl;
                    break;
                case MicroCodeInstruction.LatchPCHIntoDOR:
                    dor = pch;
                    break;
                case MicroCodeInstruction.LatchDILIntoSP:
                    sp = dil;
                    break;
                case MicroCodeInstruction.LatchStatusIntoDOR:
                    dor = (byte)(p.Value | 0b00100000);
                    break;
                #endregion

                default:
                    throw new InvalidOperationException($"Micro Code Instruction [{instruction.ToString()}] not supported");
            }
        }

        private ushort PC => (ushort)(pch << 8 | pcl);
        private byte Data => RW == RWState.Read ? dil : dor;

        private sbyte branchShift;
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
        private InteruptControl interuptControl;
    }
}
