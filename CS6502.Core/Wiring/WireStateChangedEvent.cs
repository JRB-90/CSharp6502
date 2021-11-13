using System;

namespace CS6502.Core
{
    public delegate void WireStateChangedEventHandler(object sender, WireStateChangedEventArgs e);
    public class WireStateChangedEventArgs : EventArgs
    {
        public WireStateChangedEventArgs(
            bool oldState,
            bool newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        public bool OldState { get; }
        public bool NewState { get; }
    }
}
