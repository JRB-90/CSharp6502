using System;

namespace CS6502.Core
{
    /// <summary>
    /// Class designed to replicate the internal register structure of a
    /// 6502 CPU, as in the diagram below:
    /// http://www.baltissen.org/images/6502.png
    /// </summary>
    internal class CpuCore
    {
        public CpuCore()
        {
            // TODO - Set initial states

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
                $"{(RW == RWState.Write ? 1 : 0)}{delimiter}" +
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
                    Convert.ToByte(RW == RWState.Write),
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
            // TODO - Precycle ops?
            RTInstruction instruction = decodeLogic.Cycle(signalEdge);
            SetDataBuffers();
            PerformRegisterTransfers(instruction);
        }

        private void SetDataBuffers()
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

        private void PerformRegisterTransfers(RTInstruction instruction)
        {
            // TODO
        }

        private ushort PC => (ushort)(pch << 8 | pcl);

        #region RTL

        private void LatchDataIntoA()
        {
            a = dl;
        }

        private void LatchAIntoData()
        {
            dl = a;
        }

        private void LatchDataIntoX()
        {
            x = dl;
        }

        private void LatchXIntoData()
        {
            dl = x;
        }

        private void LatchDataIntoY()
        {
            y = dl;
        }

        private void LatchYIntoData()
        {
            dl = y;
        }

        private void LatchDataIntoIR()
        {
            ir = dl;
        }

        private void LatchIRIntoData()
        {
            dl = ir;
        }

        private void LatchDataIntoSP()
        {
            sp = dl;
        }

        private void LatchSPIntoData()
        {
            dl = sp;
        }

        private void LatchDataIntoPCL()
        {
            pcl = dl;
        }

        private void LatchPCLIntoData()
        {
            dl = pcl;
        }

        private void LatchDataIntoPCH()
        {
            pch = dl;
        }

        private void LatchPCHIntoData()
        {
            dl = pch;
        }

        private void LatchDataIntoP()
        {
            p.SetFlagsFromData(dl);
        }

        private void LatchPIntoData()
        {
            dl = p.Value;
        }

        private void LatchPCIntoAddress()
        {
            abl = pcl;
            abh = pch;
        }

        #endregion

        private byte a;
        private byte x;
        private byte y;
        private byte ir;
        private byte sp;
        private byte pcl;
        private byte pch;
        private byte abl;
        private byte abh;
        private byte dl;
        private StatusRegister p;
        private DecodeLogic decodeLogic;
    }
}
