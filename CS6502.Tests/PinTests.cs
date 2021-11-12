using CS6502.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS6502.Tests
{
    [TestClass]
    public class PinTests
    {
        [TestMethod]
        public void CanConstruct()
        {
            Pin pin = new Pin();
        }

        [TestMethod]
        public void CanConstructWithArguments()
        {
            Pin pin = new Pin(TriState.False);
            pin.State.Should().Be(TriState.False);

            pin = new Pin(TriState.True);
            pin.State.Should().Be(TriState.True);

            pin = new Pin(TriState.HighImpedance);
            pin.State.Should().Be(TriState.HighImpedance);
        }

        [TestMethod]
        public void DoesRaiseEvents()
        {
            Pin pin = new Pin(TriState.False);
            using var monitoredPin = pin.Monitor();

            pin.State = TriState.False;
            monitoredPin.Should().NotRaise(nameof(Pin.StateChanged));

            pin.State = TriState.True;
            monitoredPin
                .Should()
                .Raise(nameof(Pin.StateChanged))
                .WithSender(pin)
                .WithArgs<PinStateChangedEventArgs>(args =>
                    args.OldState == TriState.False &&
                    args.NewState == TriState.True
                );
        }

        [TestMethod]
        public void DoExtensionsWork()
        {
            Pin[] pins = new Pin[8];
            for (int i = 0; i < pins.Length; i++)
            {
                pins[i] = new Pin();
            }
            pins.SetAllTo(TriState.HighImpedance);

            foreach (Pin pin in pins)
            {
                pin.State.Should().Be(TriState.HighImpedance);
            }

            pins.SetTo(0xAA);
            pins[0].State.Should().Be(TriState.False);
            pins[1].State.Should().Be(TriState.True);
            pins[2].State.Should().Be(TriState.False);
            pins[3].State.Should().Be(TriState.True);
            pins[4].State.Should().Be(TriState.False);
            pins[5].State.Should().Be(TriState.True);
            pins[6].State.Should().Be(TriState.False);
            pins[7].State.Should().Be(TriState.True);
        }
    }
}
