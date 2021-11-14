using System;

namespace CS6502.Core
{
    /// <summary>
    /// Abstract base class to share funcitonality amongst all CPU implementations.
    /// </summary>
    public abstract class CpuBase : ICpu
    {
        public CpuBase(string name)
        {
            Name = name;

            IRQ_N = new Wire(WirePull.PullUp);
            NMI_N = new Wire(WirePull.PullUp);
            RES_N = new Wire(WirePull.PullUp);
            RDY_N = new Wire(WirePull.PullUp);

            RW_N = new Wire(WirePull.PullUp);
            rw_n = new Pin();
            RW_N.ConnectPin(rw_n);

            SYNC_N = new Wire(WirePull.PullUp);
            sync_n = new Pin();
            SYNC_N.ConnectPin(sync_n);

            PHI2 = new Wire(WirePull.PullDown);
            SetPhiOutput();

            PHI1O = new Wire(WirePull.PullDown);
            phi1o = new Pin();
            PHI1O.ConnectPin(phi1o);

            PHI2O = new Wire(WirePull.PullDown);
            phi2o = new Pin();
            PHI2O.ConnectPin(phi2o);

            AddressBus = new Bus(16);
            addressPins = AddressBus.CreateAndConnectPinArray();
            DataBus = new Bus(8);
            dataPins = DataBus.CreateAndConnectPinArray();
        }

        public string Name { get; }

        public Wire IRQ_N
        {
            get => irq_n;
            set
            {
                irq_n = value;
                irq_n.StateChanged += Irq_n_StateChanged;
            }
        }

        public Wire NMI_N
        {
            get => nmi_n;
            set
            {
                nmi_n = value;
                nmi_n.StateChanged += Nmi_n_StateChanged;
            }
        }

        public Wire RES_N
        {
            get => res_n;
            set
            {
                res_n = value;
                res_n.StateChanged += Res_n_StateChanged;
            }
        }

        public Wire RDY_N
        {
            get => rdy_n;
            set
            {
                rdy_n = value;
                rdy_n.StateChanged += Rdy_n_StateChanged;
            }
        }

        public Wire RW_N { get; }

        public Wire SYNC_N { get; }

        public Wire PHI2
        {
            get => phi2;
            set
            {
                phi2 = value;
                SetPhiOutput();
                phi2.StateChanged += Phi2_StateChanged;
            }
        }

        public Wire PHI1O { get; }

        public Wire PHI2O { get; }

        public Bus AddressBus
        {
            get => addressBus;
            set
            {
                addressBus.DisconnectPins(addressPins);
                addressBus = value;
                addressBus.ConnectPins(addressPins);
            }
        }

        public Bus DataBus
        {
            get => dataBus;
            set
            {
                dataBus.DisconnectPins(dataPins);
                dataBus = value;
                dataBus.ConnectPins(dataPins);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        protected void SetAddressBus(ushort address)
        {
            addressPins.SetTo(address);
        }

        protected byte ReadFromDataBus()
        {
            dataPins.SetAllTo(TriState.HighImpedance);
            rw_n.State = TriState.True;

            return dataBus.ToByte();
        }

        protected void WriteToDataBus(byte value)
        {
            dataPins.SetTo(value);
            rw_n.State = TriState.False;
        }

        protected abstract void Cycle(SignalEdge signalEdge);

        private void Irq_n_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            // TODO - Handle IRQ
        }

        private void Nmi_n_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            // TODO - Handle NMI
        }

        private void Res_n_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            // TODO - Handle RESET
        }

        private void Rdy_n_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            // TODO - Handle RDY
        }

        private void Phi2_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            SetPhiOutput();

            if (e.NewState == true)
            {
                Cycle(SignalEdge.RisingEdge);
            }
            else if (e.NewState == false)
            {
                Cycle(SignalEdge.FallingEdge);
            }
        }

        private void SetPhiOutput()
        {
            phi1o.State = PHI2.State ? TriState.False : TriState.True;
            phi2o.State = PHI2.State ? TriState.True : TriState.False;
        }

        private Wire irq_n;
        private Wire nmi_n;
        private Wire res_n;
        private Wire rdy_n;
        private Wire phi2;
        private Pin rw_n;
        private Pin sync_n;
        private Pin phi1o;
        private Pin phi2o;
        private Bus addressBus;
        private Bus dataBus;
        private Pin[] addressPins;
        private Pin[] dataPins;
    }
}
