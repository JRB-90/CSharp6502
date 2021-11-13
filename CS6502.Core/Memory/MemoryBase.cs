namespace CS6502.Core
{
    /// <summary>
    /// Base class for sharing functionality accross all memory implementations.
    /// </summary>
    public abstract class MemoryBase : IMemory
    {
        public MemoryBase(
            int size,
            int addressWidth,
            int dataWidth,
            byte dataFillValue)
        {
            Size = size;
            data = new byte[size];
            data.Populate<byte>(dataFillValue);
            dataPins = Pin.CreatePinArray(dataWidth);
            dataPins.SetAllTo(TriState.HighImpedance);
            CS_N = new Wire(WirePull.PullUp);
            AddressBus = new Bus(addressWidth);
            DataBus = new Bus(dataWidth);
        }

        public MemoryBase(
            int size,
            Bus addressBus,
            Bus dataBus)
        {
            Size = size;
            data = new byte[size];
            data.Populate<byte>(0xFF);
            dataPins = Pin.CreatePinArray(dataBus.Size);
            dataPins.SetAllTo(TriState.HighImpedance);
            CS_N = new Wire(WirePull.PullUp);
            AddressBus = addressBus;
            DataBus = dataBus;
        }

        public MemoryBase(
            string path,
            int addressWidth,
            int dataWidth)
        {
            data = MemoryTools.LoadDataFromFile(path);
            Size = data.Length;
            dataPins = Pin.CreatePinArray(dataWidth);
            dataPins.SetAllTo(TriState.HighImpedance);
            CS_N = new Wire(WirePull.PullUp);
            AddressBus = new Bus(addressWidth);
            DataBus = new Bus(dataWidth);
        }

        public int Size { get; }

        public int AddressWidth => AddressBus.Size;

        public int DataWidth => DataBus.Size;

        public Wire CS_N
        {
            get => cs_n;
            set
            {
                cs_n = value;
                cs_n.StateChanged += Cs_n_StateChanged;
            }
        }

        public Bus AddressBus
        {
            get => addressBus;
            set
            {
                addressBus = value;
                addressBus.StateChanged += Bus_StateChanged;
            }
        }

        public Bus DataBus
        {
            get => dataBus;
            set
            {
                dataBus?.DisconnectPins(dataPins);
                dataBus = value;
                dataBus.ConnectPins(dataPins);
                dataBus.StateChanged += Bus_StateChanged;
            }
        }

        public byte[] Data => data;

        protected abstract void InputChanged();

        private void Cs_n_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            InputChanged();
        }

        private void Bus_StateChanged(object sender, BusStateChangedEventArgs e)
        {
            InputChanged();
        }

        protected Pin[] dataPins;
        protected byte[] data;
        private Wire cs_n;
        private Bus addressBus;
        private Bus dataBus;
    }
}
