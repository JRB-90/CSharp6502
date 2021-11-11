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
    }
}
