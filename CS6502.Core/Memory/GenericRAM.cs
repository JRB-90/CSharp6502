namespace CS6502.Core
{
    /// <summary>
    /// Represents a generic Random Access Memory (RAM) device.
    /// </summary>
    public class GenericRAM : MemoryBase, IRAM
    {
        public GenericRAM(
            int size,
            int addressWidth,
            int dataWidth)
          :
            base(
                size,
                addressWidth,
                dataWidth,
                0x00
            )
        {
            OE_N = new Wire(WirePull.PullUp);
            RW_N = new Wire(WirePull.PullUp);
        }

        public GenericRAM(
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
            RW_N = new Wire(WirePull.PullUp);
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

        public Wire RW_N
        {
            get => rw_n;
            set
            {
                rw_n = value;
                rw_n.StateChanged += EnablePin_StateChanged;
            }
        }

        protected override void InputChanged()
        {
            if (CS_N.State == false &&
                OE_N.State == false)
            {
                if (RW_N.State)
                {
                    byte dataValue = data[AddressBus.ToUint()];
                    dataPins.SetTo(dataValue);
                }
                else
                {
                    dataPins.SetAllTo(TriState.HighImpedance);
                    byte dataValue = DataBus.ToByte();
                    uint addressValue = AddressBus.ToUint();
                    data[addressValue] = dataValue;
                }
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
        private Wire rw_n;
    }
}
