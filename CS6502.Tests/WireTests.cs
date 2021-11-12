using CS6502.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CS6502.Tests
{
    [TestClass]
    public class WireTests
    {
        [TestMethod]
        public void CanConstruct()
        {
            Wire wire = new Wire(WirePull.PullDown);
            wire.WirePull.Should().Be(WirePull.PullDown);
        }

        [TestMethod]
        public void CanConnectPins()
        {
            Wire wire = new Wire(WirePull.PullDown);
            wire.ConnectedPins.Should().BeEmpty();
            Pin pin = new Pin();
            wire.ConnectPin(pin);
            wire.ConnectedPins.Should().Contain(pin);
        }

        [TestMethod]
        public void CanDiconnectPins()
        {
            Wire wire = new Wire(WirePull.PullDown);
            wire.ConnectedPins.Should().BeEmpty();
            Pin pin = new Pin();
            wire.ConnectPin(pin);
            wire.ConnectedPins.Should().Contain(pin);
            wire.DisconnectPin(pin);
            wire.ConnectedPins.Should().NotContain(pin);
        }

        [TestMethod]
        public void CanResolveWireCorrectly()
        {
            Wire wire = new Wire(WirePull.PullDown);
            wire.State.Should().Be(false);

            Pin[] pins = new Pin[3];
            for (int i = 0; i < pins.Length; i++)
            {
                pins[i] = new Pin();
                wire.ConnectPin(pins[i]);
            }

            pins.SetAllTo(TriState.HighImpedance);
            wire.State.Should().Be(false);

            pins[0].State = TriState.False;
            wire.State.Should().Be(false);

            pins[1].State = TriState.False;
            wire.State.Should().Be(false);

            pins[2].State = TriState.False;
            wire.State.Should().Be(false);

            pins[0].State = TriState.True;
            wire.State.Should().Be(true);

            pins[0].State = TriState.False;
            pins[2].State = TriState.True;
            wire.State.Should().Be(true);

            pins[2].State = TriState.False;
            pins[1].State = TriState.True;
            wire.State.Should().Be(true);

            wire = new Wire(WirePull.PullUp);
            wire.State.Should().Be(true);

            for (int i = 0; i < pins.Length; i++)
            {
                wire.ConnectPin(pins[i]);
            }

            pins.SetAllTo(TriState.HighImpedance);
            wire.State.Should().Be(true);

            pins[0].State = TriState.False;
            wire.State.Should().Be(false);

            pins[1].State = TriState.False;
            wire.State.Should().Be(false);

            pins[2].State = TriState.False;
            wire.State.Should().Be(false);

            pins[0].State = TriState.True;
            wire.State.Should().Be(true);

            pins[0].State = TriState.False;
            pins[2].State = TriState.True;
            wire.State.Should().Be(true);

            pins[2].State = TriState.False;
            pins[1].State = TriState.True;
            wire.State.Should().Be(true);
        }

        [TestMethod]
        public void DoesRaiseEvents()
        {
            Wire wire = new Wire(WirePull.PullDown);
            Pin[] pins = new Pin[3];

            for (int i = 0; i < pins.Length; i++)
            {
                pins[i] = new Pin();
                wire.ConnectPin(pins[i]);
            }

            using var monitoredWire = wire.Monitor();
            pins[0].State = TriState.True;

            monitoredWire
                .Should()
                .Raise(nameof(Wire.StateChanged))
                .WithSender(wire)
                .WithArgs<WireStateChangedEventArgs>(args =>
                    args.OldState == false &&
                    args.NewState == true
                );

            pins.SetAllTo(TriState.HighImpedance);
            monitoredWire.Clear();

            Pin p = new Pin(TriState.True);
            wire.ConnectPin(p);
            monitoredWire
                .Should()
                .Raise(nameof(Wire.StateChanged))
                .WithSender(wire)
                .WithArgs<WireStateChangedEventArgs>(args =>
                    args.OldState == false &&
                    args.NewState == true
                );

            wire.DisconnectPin(p);
            monitoredWire
                .Should()
                .Raise(nameof(Wire.StateChanged))
                .WithSender(wire)
                .WithArgs<WireStateChangedEventArgs>(args =>
                    args.OldState == true &&
                    args.NewState == false
                );
        }

        [TestMethod]
        public void DoExtensionMethodsWork()
        {
            Wire[] wires = new Wire[8];
            Pin[] pins = new Pin[8];
            for (int i = 0; i < wires.Length; i++)
            {
                wires[i] = new Wire(WirePull.PullDown);
                pins[i] = new Pin();
                wires[i].ConnectPin(pins[i]);
            }
            
            pins.SetAllTo(TriState.False);
            pins.SetTo(0xAA);
            wires.ToByte().Should().Be(0xAA);
            pins.SetTo(0x55);
            wires.ToByte().Should().Be(0x55);

            wires = new Wire[16];
            pins = new Pin[16];
            for (int i = 0; i < wires.Length; i++)
            {
                wires[i] = new Wire(WirePull.PullDown);
                pins[i] = new Pin();
                wires[i].ConnectPin(pins[i]);
            }

            Action action = () => wires.ToByte();
            action
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Too many wires to resolve to a byte, must be <= 8");

            pins.SetAllTo(TriState.False);
            pins.SetTo(0xAAAA);
            wires.ToUshort().Should().Be(0xAAAA);
            pins.SetTo(0x5555);
            wires.ToUshort().Should().Be(0x5555);

            wires = new Wire[32];
            pins = new Pin[32];
            for (int i = 0; i < wires.Length; i++)
            {
                wires[i] = new Wire(WirePull.PullDown);
                pins[i] = new Pin();
                wires[i].ConnectPin(pins[i]);
            }

            action = () => wires.ToUshort();
            action
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Too many wires to resolve to a ushort, must be <= 16");

            pins.SetAllTo(TriState.False);
            pins.SetTo(0xAAAAAAAA);
            wires.ToUint().Should().Be(0xAAAAAAAA);
            pins.SetTo(0x55555555);
            wires.ToUint().Should().Be(0x55555555);

            wires = new Wire[33];
            pins = new Pin[33];
            for (int i = 0; i < wires.Length; i++)
            {
                wires[i] = new Wire(WirePull.PullDown);
                pins[i] = new Pin();
                wires[i].ConnectPin(pins[i]);
            }

            action = () => wires.ToUint();
            action
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Too many wires to resolve to a uint, must be <= 32");
        }
    }
}
