using System;

namespace CS6502.Core
{
    public delegate void PinStateChangedEventHandler(object sender, PinStateChangedEventArgs e);
    public class PinStateChangedEventArgs : EventArgs
    {
        public PinStateChangedEventArgs(
            TriState oldState,
            TriState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        public TriState OldState { get; }
        public TriState NewState { get; }
    }
}
