using CS6502.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;
using System;

namespace CS6502.Tests
{
    [TestClass]
    public class BusTests
    {
        [TestMethod]
        public void CanConstruct()
        {
            Bus bus = new Bus();
        }

        [TestMethod]
        public void CanConstructWithArguments()
        {
            Bus firstBus = new Bus(8);
            firstBus.Wires.Count.Should().Be(8);

            Bus secondBus = new Bus(firstBus);
            secondBus.Wires.Count.Should().Be(firstBus.Wires.Count);
            for (int i = 0; i < secondBus.Size; i++)
            {
                secondBus.Wires[i].Should().BeSameAs(firstBus.Wires[i]);
            }

            Wire[] wireArray = firstBus.Wires.ToArray();
            Bus thirdBus = new Bus(wireArray);
            for (int i = 0; i < thirdBus.Size; i++)
            {
                thirdBus.Wires[i].Should().BeSameAs(firstBus.Wires[i]);
            }

            List<Wire> wireList = firstBus.Wires.ToList();
            thirdBus = new Bus(wireList);
            for (int i = 0; i < thirdBus.Size; i++)
            {
                thirdBus.Wires[i].Should().BeSameAs(firstBus.Wires[i]);
            }

            Bus fourthBus = new Bus(firstBus, 0, 3);
            fourthBus.Size.Should().Be(3);
            for (int i = 0; i < 3; i++)
            {
                fourthBus.Wires[i].Should().BeSameAs(firstBus.Wires[i]);
            }

            Bus fifthBus = new Bus(firstBus, 2, 4);
            fifthBus.Size.Should().Be(4);
            for (int i = 0; i < 4; i++)
            {
                fifthBus.Wires[i].Should().BeSameAs(firstBus.Wires[i + 2]);
            }

            Action action = () => new Bus(firstBus, 5, 1);
            action.Should().NotThrow();

            action = () => new Bus(firstBus, 7, 1);
            action.Should().NotThrow();

            action = () => new Bus(firstBus, 7, 2);
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void CanConnectPins()
        {
            Bus bus = new Bus(8);
            Pin[] pins = Pin.CreatePinArray(8);
            bus.ConnectPins(pins);
            for (int i = 0; i < 8; i++)
            {
                bus.Wires[i].ConnectedPins.Should().HaveCount(1);
                bus.Wires[i].ConnectedPins[0].Should().BeSameAs(pins[i]);
            }
        }

        [TestMethod]
        public void CanCreateConnectedPins()
        {
            Bus bus = new Bus(8);
            Pin[] pinArray = bus.CreateAndConnectPinArray();
            for (int i = 0; i < 8; i++)
            {
                bus.Wires[i].ConnectedPins.Should().HaveCount(1);
                bus.Wires[i].ConnectedPins[0].Should().BeSameAs(pinArray[i]);
            }

            bus = new Bus(8);
            List<Pin> pinList = bus.CreateAndConnectPinList();
            for (int i = 0; i < 8; i++)
            {
                bus.Wires[i].ConnectedPins.Should().HaveCount(1);
                bus.Wires[i].ConnectedPins[0].Should().BeSameAs(pinList[i]);
            }
        }

        [TestMethod]
        public void DoExtensionsWork()
        {
            Bus bus = new Bus(8);
            Pin[] pins = bus.CreateAndConnectPinArray();
            pins.SetAllTo(TriState.False);
            pins.SetTo(0xAA);
            bus.ToByte().Should().Be(0xAA);
            pins.SetTo(0x55);
            bus.ToByte().Should().Be(0x55);

            bus = new Bus(16);
            pins = bus.CreateAndConnectPinArray();
            Action action = () => bus.ToByte();
            action
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Too many wires to resolve to a byte, must be <= 8");

            pins.SetAllTo(TriState.False);
            pins.SetTo(0xAAAA);
            bus.ToUshort().Should().Be(0xAAAA);
            pins.SetTo(0x5555);
            bus.ToUshort().Should().Be(0x5555);

            bus = new Bus(32);
            pins = bus.CreateAndConnectPinArray();
            action = () => bus.ToUshort();
            action
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Too many wires to resolve to a ushort, must be <= 16");

            pins.SetAllTo(TriState.False);
            pins.SetTo(0xAAAAAAAA);
            bus.ToUint().Should().Be(0xAAAAAAAA);
            pins.SetTo(0x55555555);
            bus.ToUint().Should().Be(0x55555555);

            bus = new Bus(33);
            pins = bus.CreateAndConnectPinArray();
            action = () => bus.ToUint();
            action
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Too many wires to resolve to a uint, must be <= 32");
        }

        [TestMethod]
        public void DoesRaiseEvents()
        {
            Bus bus = new Bus(8);
            Pin[] pinArray = bus.CreateAndConnectPinArray();

            using var monitoredBus = bus.Monitor();
            pinArray[0].State = TriState.True;
            monitoredBus.Should().Raise(nameof(Bus.StateChanged));

            monitoredBus.Clear();
            pinArray[0].State = TriState.True;
            monitoredBus.Should().NotRaise(nameof(Bus.StateChanged));
        }
    }
}
