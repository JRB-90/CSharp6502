using System;
using System.Collections.Generic;
using System.Linq;

namespace CS6502.Core
{
    /// <summary>
    /// Represents a bus, which is a collection of wires.
    /// This class helps to use the wire system in a more simplified way.
    /// </summary>
    public class Bus
    {
        public Bus()
        {
            wires = new List<Wire>();
        }

        public Bus(int size)
        {
            wires = new List<Wire>();
            for (int i = 0; i < size; i++)
            {
                Wire wire = new Wire(WirePull.PullDown);
                wire.StateChanged += Wire_StateChanged;
                wires.Add(wire);
            }
        }

        public Bus(Wire[] wires)
        {
            this.wires = wires.ToList();
            foreach (Wire wire in wires)
            {
                wire.StateChanged += Wire_StateChanged;
            }
        }

        public Bus(List<Wire> wires)
        {
            this.wires = wires;
            foreach (Wire wire in wires)
            {
                wire.StateChanged += Wire_StateChanged;
            }
        }

        public Bus(Bus bus)
        {
            wires = new List<Wire>();
            for (int i = 0; i < bus.Wires.Count; i++)
            {
                wires.Add(bus.Wires[i]);
                Wires[i].StateChanged += Wire_StateChanged;
            }
        }

        public Bus(Bus bus, int startIndex, int length)
        {
            if (startIndex >= bus.Size ||
                startIndex + length > bus.Size)
            {
                throw new ArgumentException($"Invalid index [{startIndex}] and length [{length}] values for bus of size {bus.Size}");
            }

            wires = new List<Wire>();
            for (int i = startIndex; i < startIndex + length; i++)
            {
                Wire w = bus.Wires[i];
                wires.Add(w);
                w.StateChanged += Wire_StateChanged;
            }
        }

        public int Size => Wires.Count;

        public IReadOnlyList<Wire> Wires => wires;

        public event BusStateChangedEventHandler StateChanged;

        public void ConnectPins(Pin[] pins)
        {
            if (pins.Length > Size)
            {
                throw new ArgumentException($"Too many pins passed in, should be <= {Size}");
            }

            for (int i = 0; i < pins.Length; i++)
            {
                Wires[i].ConnectPin(pins[i]);
            }
        }

        public void ConnectPins(List<Pin> pins)
        {
            ConnectPins(pins.ToArray());
        }

        public void DisconnectPins(Pin[] pins)
        {
            for (int i = 0; i < pins.Length; i++)
            {
                Wires[i].DisconnectPin(pins[i]);
            }
        }

        public void DisconnectPins(List<Pin> pins)
        {
            DisconnectPins(pins.ToArray());
        }

        public Pin[] CreateAndConnectPinArray()
        {
            Pin[] pins = Pin.CreatePinArray(Size);
            for (int i = 0; i < pins.Length; i++)
            {
                wires[i].ConnectPin(pins[i]);
            }

            return pins;
        }

        public List<Pin> CreateAndConnectPinList()
        {
            List<Pin> pins = Pin.CreatePinList(Size);
            for (int i = 0; i < pins.Count; i++)
            {
                wires[i].ConnectPin(pins[i]);
            }

            return pins;
        }

        public byte ToByte()
        {
            return wires.ToByte();
        }

        public ushort ToUshort()
        {
            return wires.ToUshort();
        }

        public uint ToUint()
        {
            return wires.ToUint();
        }

        public override string ToString()
        {
            string hexStr = ToUint().ToHexString();
            if (Size <= 16)
            {
                hexStr = ToUshort().ToHexString();
            }
            if (Size <= 8)
            {
                hexStr = ToByte().ToHexString();
            }

            return
                hexStr +
                " - " +
                wires.ToBinaryString();
        }

        private void Wire_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            StateChanged?.Invoke(this, new BusStateChangedEventArgs());
        }

        private List<Wire> wires;
    }
}
