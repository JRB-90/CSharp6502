namespace CS6502.Core
{
    internal class InteruptControl
    {
        public InteruptControl()
        {
            nmi = new Wire(WirePull.PullUp);
            IRQ = new Wire(WirePull.PullUp);
        }

        public Wire NMI
        {
            get => nmi;
            set
            {
                nmi = value;
                nmi.StateChanged += Nmi_StateChanged;
            }
        }

        public Wire IRQ { get ; set; }

        public bool NmiActive { get; private set; }

        public bool IrqActive { get; private set; }

        public void Cycle()
        {
            if (IRQ.State == false ||
                brkActive == true)
            {
                IrqActive = true;
            }
            else
            {
                IrqActive = false;
            }
        }

        public void SignalBRK()
        {
            IrqActive = true;
            brkActive = true;
        }

        public void ClearBrk()
        {
            brkActive = false;
        }

        public void ClearNmi()
        {
            NmiActive = false;
        }

        private void Nmi_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            if (e.OldState == true &&
                e.NewState == false)
            {
                NmiActive = true;
            }
        }

        private Wire nmi;
        private bool brkActive;
    }
}
