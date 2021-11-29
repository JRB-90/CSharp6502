﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS6502.Core
{
    public class AccurateCpu : ICpu
    {
        #region Constants

        const ushort IRQ_VECTOR_HI = 0xFFFF;
        const ushort IRQ_VECTOR_LO = 0xFFFE;
        const ushort RST_VECTOR_HI = 0xFFFD;
        const ushort RST_VECTOR_LO = 0xFFFC;
        const ushort NMI_VECTOR_HI = 0xFFFB;
        const ushort NMI_VECTOR_LO = 0xFFFA;

        #endregion

        #region Constructors

        public AccurateCpu()
        {
            Name = "Accurat6502";

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

            PHI1O = new Wire(WirePull.PullDown);
            phi1o = new Pin();
            PHI1O.ConnectPin(phi1o);

            PHI2O = new Wire(WirePull.PullDown);
            phi2o = new Pin();
            PHI2O.ConnectPin(phi2o);

            PHI2 = new Wire(WirePull.PullDown);
            RecalculatePhiOutputs();

            addressBus = new Bus(16);
            addressPins = addressBus.CreateAndConnectPinArray();
            AddressBus = addressBus;

            dataBus = new Bus(8);
            dataPins = dataBus.CreateAndConnectPinArray();
            DataBus = dataBus;

            core = new CpuCore(); // TODO - Hook up wires into core..

            // TODO - Set CPU to startup state
        }

        #endregion

        #region Properties

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
                RecalculatePhiOutputs();
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

        #endregion

        #region Helper Functions

        public override string ToString()
        {
            return Name;
        }

        public string GetCurrentStateString(char delimiter)
        {
            return core.GetCurrentStateString(delimiter);
        }

        public CycleState GetCurrentCycleState(int cycleID)
        {
            return core.GetCurrentCycleState(cycleID);
        }

        private void RecalculatePhiOutputs()
        {
            phi1o.State = PHI2.State ? TriState.False : TriState.True;
            phi2o.State = PHI2.State ? TriState.True : TriState.False;
        }

        #endregion

        #region EventHandlers

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
            RecalculatePhiOutputs();

            if (e.NewState == true)
            {
                Cycle(SignalEdge.RisingEdge);
            }
            else if (e.NewState == false)
            {
                Cycle(SignalEdge.FallingEdge);
            }
        }

        #endregion

        #region Cycle Handling

        /// <summary>
        /// Handle a clock transition.
        /// </summary>
        private void Cycle(SignalEdge signalEdge)
        {
            RecalculatePhiOutputs();
            core.Cycle(signalEdge);

            // TODO - Set ext pins to correct values
            SetPinValues();
        }

        private void SetPinValues()
        {
            if (core.RW == RWState.Read)
            {
                rw_n.State = TriState.True;
                dataPins.SetTo(core.DataOut);
            }
            else if (core.RW == RWState.Write)
            {
                rw_n.State = TriState.False;
                dataPins.SetAllTo(TriState.HighImpedance);

                // This might go here or on the next cycle?
                //core.DataIn = dataBus.ToByte();
            }

            sync_n.State = core.Sync == EnableState.Enabled ? TriState.False : TriState.True;
            addressPins.SetTo(core.Address);
        }

        #endregion

        private CpuCore core;
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
