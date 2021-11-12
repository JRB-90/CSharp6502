using System;

namespace CS6502.Core
{
    public delegate void BusStateChangedEventHandler(object sender, BusStateChangedEventArgs e);
    public class BusStateChangedEventArgs : EventArgs
    {
        public BusStateChangedEventArgs()
        {
        }
    }
}
