using System.Collections.Generic;

namespace CS6502.Core
{
    /// <summary>
    /// Represents a physical pin that can be driven.
    /// All pins use tri-state logic and can be put into high impedance (Z) state.
    /// </summary>
    public class Pin
    {
        public Pin()
        {
            State = TriState.False;
        }

        public Pin(TriState initialState)
        {
            State = initialState;
        }

        public TriState State
        {
            get => state;
            set
            {
                if (value != state)
                {
                    TriState oldState = State;
                    state = value;
                    StateChanged?.Invoke(
                        this,
                        new PinStateChangedEventArgs(oldState, state)
                    );
                }
            }
        }

        public event PinStateChangedEventHandler StateChanged;

        public override string ToString()
        {
            switch (state)
            {
                case TriState.False:
                    return "0";
                case TriState.True:
                    return "1";
                case TriState.HighImpedance:
                    return "Z";
                default:
                    return "Unkown";
            }
        }

        public static Pin[] CreatePinArray(int size)
        {
            Pin[] pins = new Pin[size];
            for (int i = 0; i < size; i++)
            {
                pins[i] = new Pin();
            }

            return pins;
        }

        public static List<Pin> CreatePinList(int size)
        {
            List<Pin> pins = new List<Pin>();
            for (int i = 0; i < size; i++)
            {
                pins.Add(new Pin());
            }

            return pins;
        }

        private TriState state;
    }
}
