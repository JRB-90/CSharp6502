using CS6502.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS6502.Tests
{
    [TestClass]
    public class DecodingTests
    {
        [TestMethod]
        public void CanConstruct()
        {
            AddressDecoder decoder =
                new AddressDecoder(
                    new AddressSpace(0x8000, 0xFFFF),
                    new AddressSpace(0x0000, 0x7FFF),
                    new Bus()
                );
        }

        [TestMethod]
        public void CanConstructWithArguments()
        {
            AddressDecoder decoder =
                new AddressDecoder(
                    new AddressSpace(0x8000, 0xFFFF),
                    new AddressSpace(0x0000, 0x7FFF),
                    new Bus()
                );

            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();
            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
        }

        [TestMethod]
        public void DoesDecodeCorrectly()
        {
            Bus addressBus = new Bus(16);
            Pin[] addressPins = addressBus.CreateAndConnectPinArray();
            addressPins.SetTo(0x0000);
            Pin phi = new Pin(TriState.False);
            Wire phiWire = new Wire(WirePull.PullDown);
            phiWire.ConnectPin(phi);

            AddressDecoder decoder =
                new AddressDecoder(
                    new AddressSpace(0x8000, 0xFFFF),
                    new AddressSpace(0x0000, 0x7FFF),
                    addressBus
                );

            decoder.PHI2 = phiWire;

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();

            addressPins.SetTo(0x7FFF);

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();

            addressPins.SetTo(0x8000);

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();

            addressPins.SetTo(0xFFFF);

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();

            addressPins.SetTo(0x0000);
            phi.State = TriState.True;

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeFalse();
            decoder.RamOE_N.State.Should().BeFalse();

            addressPins.SetTo(0x7FFF);

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeFalse();
            decoder.RamOE_N.State.Should().BeFalse();

            addressPins.SetTo(0x8000);

            decoder.RomCS_N.State.Should().BeFalse();
            decoder.RomOE_N.State.Should().BeFalse();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();

            addressPins.SetTo(0xFFFF);

            decoder.RomCS_N.State.Should().BeFalse();
            decoder.RomOE_N.State.Should().BeFalse();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();

            addressBus = new Bus(16);
            addressPins = addressBus.CreateAndConnectPinArray();
            addressPins.SetTo(0x0000);
            phi = new Pin(TriState.False);
            phiWire = new Wire(WirePull.PullDown);
            phiWire.ConnectPin(phi);

            decoder =
                new AddressDecoder(
                    new AddressSpace(0x8000, 0xFFFF),
                    new AddressSpace(0x0000, 0x7FFF)
                );
            decoder.AddressBus = addressBus;
            decoder.PHI2 = phiWire;

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();

            addressPins.SetTo(0x7FFF);

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();

            addressPins.SetTo(0x8000);

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();

            addressPins.SetTo(0xFFFF);

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();

            addressPins.SetTo(0x0000);
            phi.State = TriState.True;

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeFalse();
            decoder.RamOE_N.State.Should().BeFalse();

            addressPins.SetTo(0x7FFF);

            decoder.RomCS_N.State.Should().BeTrue();
            decoder.RomOE_N.State.Should().BeTrue();
            decoder.RamCS_N.State.Should().BeFalse();
            decoder.RamOE_N.State.Should().BeFalse();

            addressPins.SetTo(0x8000);

            decoder.RomCS_N.State.Should().BeFalse();
            decoder.RomOE_N.State.Should().BeFalse();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();

            addressPins.SetTo(0xFFFF);

            decoder.RomCS_N.State.Should().BeFalse();
            decoder.RomOE_N.State.Should().BeFalse();
            decoder.RamCS_N.State.Should().BeTrue();
            decoder.RamOE_N.State.Should().BeTrue();
        }
    }
}
