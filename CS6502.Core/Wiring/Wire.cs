using System;
using System.Collections.Generic;

namespace CS6502.Core
{
    /// <summary>
    /// Represents a wire that many pins can attach to.
    /// The wire resolves it's state for any observer's by looking at the
    /// combination of output states of all the pins attached to it.
    /// </summary>
    public class Wire
    {
        public Wire(WirePull wirePull)
        {
            this.wirePull = wirePull;
            pins = new List<Pin>();
        }

        public bool State
        {
            get
            {
                cachedState = CalculateWireState();

                return cachedState;
            }
        }

        public WirePull WirePull
        {
            get => wirePull;
            set
            {
                bool oldState = State;
                wirePull = value;
                CheckNewState(oldState);
            }
        }

        public IReadOnlyList<Pin> ConnectedPins => pins;

        public event WireStateChangedEventHandler StateChanged;

        public void ConnectPin(Pin pin)
        {
            if (!pins.Contains(pin))
            {
                bool oldState = State;
                pin.StateChanged += Pin_StateChanged;
                pins.Add(pin);
                CheckNewState(oldState);
            }
        }

        public void DisconnectPin(Pin pin)
        {
            if (pins.Contains(pin))
            {
                bool oldState = State;
                pin.StateChanged -= Pin_StateChanged;
                pins.Remove(pin);
                CheckNewState(oldState);
            }
        }

        public Pin CreateConnectedPin()
        {
            Pin pin = new Pin();
            ConnectPin(pin);

            return pin;
        }

        public void ClearConnectedPins()
        {
            bool oldState = State;
            pins.Clear();
            CheckNewState(oldState);
        }

        public override string ToString()
        {
            return State ? "1" : "0";
        }

        private void CheckNewState(bool oldState)
        {
            bool newState = State;
            if (newState != oldState)
            {
                StateChanged?.Invoke(this, new WireStateChangedEventArgs(oldState, newState));
            }
        }

        private bool CalculateWireState()
        {
            if (wirePull == WirePull.PullDown)
            {
                foreach (var pin in ConnectedPins)
                {
                    if (pin.State == TriState.True)
                    {
                        return true;
                    }
                }

                return false;
            }
            else if (wirePull == WirePull.PullUp)
            {
                TriState currentState = TriState.HighImpedance;

                foreach (var pin in ConnectedPins)
                {
                    if (pin.State == TriState.False &&
                        currentState != TriState.True)
                    {
                        currentState = TriState.False;
                    }
                    else if (pin.State == TriState.True)
                    {
                        return true;
                    }
                }

                if (currentState == TriState.False)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                throw new InvalidOperationException($"Wire pull {wirePull} not supported");
            }
        }

        private void Pin_StateChanged(object sender, PinStateChangedEventArgs e)
        {
            CheckNewState(cachedState);
        }

        private WirePull wirePull;
        private List<Pin> pins;
        private bool cachedState;
    }
}
