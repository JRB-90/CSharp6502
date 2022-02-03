using System;

namespace CS6502.Core
{
    /// <summary>
    /// Represents a generic Read Only Memory (ROM) device.
    /// </summary>
    public class GenericROM : MemoryBase, IROM
    {
        public GenericROM(
            int size,
            int addressWidth,
            int dataWidth)
          :
            base(
                size,
                addressWidth,
                dataWidth,
                0xFF
            )
        {
            OE_N = new Wire(WirePull.PullUp);
        }

        public GenericROM(
            byte[] data,
            int addressWidth,
            int dataWidth)
          :
            base(
                data,
                addressWidth,
                dataWidth
            )
        {
            OE_N = new Wire(WirePull.PullUp);
        }

        public Wire OE_N
        {
            get => oe_n;
            set
            {
                oe_n = value;
                oe_n.StateChanged += EnablePin_StateChanged;
            }
        }

        public void LoadData(byte[] data)
        {
            if (data.Length != Size)
            {
                throw new ArgumentException("Data to load into ROM of incorrect size");
            }

            this.data = data;
        }

        protected override void InputChanged()
        {
            if (CS_N.State == false &&
                OE_N.State == false)
            {
                ushort addr = AddressBus.ToUshort();
                byte dataValue = data[addr];
                dataPins.SetTo(dataValue);
            }
            else
            {
                dataPins.SetAllTo(TriState.HighImpedance);
            }
        }

        private void EnablePin_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            InputChanged();
        }

        private Wire oe_n;
    }
}
