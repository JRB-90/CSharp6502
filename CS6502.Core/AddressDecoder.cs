namespace CS6502.Core
{
    /// <summary>
    /// Represents an address decoding circuit, which decodes the address bus
    /// and sets the correct chip enables to create a memory map.
    /// </summary>
    public class AddressDecoder
    {
        public AddressDecoder(
            AddressSpace romLocation,
            AddressSpace ramLocation)
        {
            RomLocation = romLocation;
            RamLocation = ramLocation;
            AddressBus = new Bus();

            RomCS_N = new Wire(WirePull.PullUp);
            rom_cs_n = new Pin(TriState.True);
            RomCS_N.ConnectPin(rom_cs_n);

            RomOE_N = new Wire(WirePull.PullUp);
            rom_oe_n = new Pin(TriState.True);
            RomOE_N.ConnectPin(rom_oe_n);

            RamCS_N = new Wire(WirePull.PullUp);
            ram_cs_n = new Pin(TriState.True);
            RamCS_N.ConnectPin(ram_cs_n);

            RamOE_N = new Wire(WirePull.PullUp);
            ram_oe_n = new Pin(TriState.True);
            RamOE_N.ConnectPin(ram_oe_n);

            CalculatePinEnables();
        }

        public AddressDecoder(
            AddressSpace romLocation,
            AddressSpace ramLocation,
            Bus addressBus)
        {
            RomLocation = romLocation;
            RamLocation = ramLocation;
            AddressBus = addressBus;

            RomCS_N = new Wire(WirePull.PullUp);
            rom_cs_n = new Pin(TriState.True);
            RomCS_N.ConnectPin(rom_cs_n);

            RomOE_N = new Wire(WirePull.PullUp);
            rom_oe_n = new Pin(TriState.True);
            RomOE_N.ConnectPin(rom_oe_n);

            RamCS_N = new Wire(WirePull.PullUp);
            ram_cs_n = new Pin(TriState.True);
            RamCS_N.ConnectPin(ram_cs_n);

            RamOE_N = new Wire(WirePull.PullUp);
            ram_oe_n = new Pin(TriState.True);
            RamOE_N.ConnectPin(ram_oe_n);

            CalculatePinEnables();
        }

        public AddressSpace RomLocation { get; }

        public AddressSpace RamLocation { get; }

        public Bus AddressBus
        {
            get => addressBus;
            set
            {
                addressBus = value;
                addressBus.StateChanged += AddressBus_StateChanged;
            }
        }

        public Wire RomCS_N { get; }

        public Wire RomOE_N { get; }

        public Wire RamCS_N { get; }

        public Wire RamOE_N { get; }

        private void AddressBus_StateChanged(object sender, BusStateChangedEventArgs e)
        {
            CalculatePinEnables();
        }

        private void CalculatePinEnables()
        {
            if (RomLocation.IsInAddressSpace(AddressBus.ToUshort()))
            {
                rom_cs_n.State = TriState.False;
                rom_oe_n.State = TriState.False;
            }
            else
            {
                rom_cs_n.State = TriState.True;
                rom_oe_n.State = TriState.True;
            }

            if (RamLocation.IsInAddressSpace(AddressBus.ToUshort()))
            {
                ram_cs_n.State = TriState.False;
                ram_oe_n.State = TriState.False;
            }
            else
            {
                ram_cs_n.State = TriState.True;
                ram_oe_n.State = TriState.True;
            }
        }

        private Bus addressBus;
        private Pin rom_cs_n;
        private Pin rom_oe_n;
        private Pin ram_cs_n;
        private Pin ram_oe_n;
    }
}
