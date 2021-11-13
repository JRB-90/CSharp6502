using CS6502.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace CS6502.Tests
{
    [TestClass]
    public class ClockTests
    {
        [TestMethod]
        public void CanConstruct()
        {
            ClockGenerator clock = new ClockGenerator();
        }

        [TestMethod]
        public void CanConstructWithArguments()
        {
            ClockGenerator clock = new ClockGenerator(ClockMode.FreeRunning);
            clock.Mode.Should().Be(ClockMode.FreeRunning);
            clock.TargetFrequency.Should().Be(-1);
        }

        [TestMethod]
        public void CanHalfCycle()
        {
            ClockGenerator clock = new ClockGenerator(ClockMode.StepHalfCycle);
            clock.CLK.State.Should().BeFalse();
            clock.Cycle();
            clock.CLK.State.Should().BeTrue();
            clock.Cycle();
            clock.CLK.State.Should().BeFalse();
        }

        [TestMethod]
        public void CanFullCycle()
        {
            ClockGenerator clock = new ClockGenerator(ClockMode.StepFullCycle);
            clock.CLK.State.Should().BeFalse();
            clock.Cycle();
            clock.CLK.State.Should().BeFalse();
            clock.Cycle();
            clock.CLK.State.Should().BeFalse();

            clock = new ClockGenerator(ClockMode.StepHalfCycle);
            clock.CLK.State.Should().BeFalse();
            clock.Cycle();
            clock.CLK.State.Should().BeTrue();

            clock.Mode = ClockMode.StepFullCycle;
            clock.Cycle();
            clock.CLK.State.Should().BeTrue();
            clock.Cycle();
            clock.CLK.State.Should().BeTrue();
        }

        int transitionCount = 0;
        Pin sync_n;

        [TestMethod]
        public void CanInstructionCycle()
        {
            ClockGenerator clock = new ClockGenerator(ClockMode.StepInstruction);
            sync_n = new Pin();
            sync_n.State = TriState.True;
            Wire syncWire = new Wire(WirePull.PullUp);
            syncWire.ConnectPin(sync_n);
            clock.SYNC_N = syncWire;
            clock.CLK.StateChanged += CLK_StateChanged;

            clock.RDY_N.State.Should().BeTrue();
            clock.Cycle();

            while (sync_n.State == TriState.True)
            {
                Thread.Sleep(0);
            }

            clock.RDY_N.State.Should().BeFalse();
        }

        private void CLK_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            if (transitionCount < 10)
            {
                sync_n.State = TriState.False;
            }
        }

        [TestMethod]
        public void CanFreeRun()
        {
            ClockGenerator clock = new ClockGenerator(10);
            clock.Mode.Should().Be(ClockMode.FreeRunning);
            clock.TargetFrequency.Should().Be(10);
            clock.IsRunning.Should().BeFalse();
            clock.Start();
            clock.IsRunning.Should().BeTrue();
            Thread.Sleep(1000);
            clock.IsRunning.Should().BeTrue();
            clock.Stop();
            clock.IsRunning.Should().BeFalse();
        }

        [TestMethod]
        public void DoesThrowInvalidOperations()
        {
            ClockGenerator clock = new ClockGenerator(ClockMode.StepHalfCycle);
            Action action = () => clock.Start();
            action.Should().Throw<InvalidOperationException>();
            clock.Mode = ClockMode.StepFullCycle;
            action.Should().Throw<InvalidOperationException>();
            clock.Mode = ClockMode.StepInstruction;
            action.Should().Throw<InvalidOperationException>();

            clock = new ClockGenerator(ClockMode.FreeRunning);
            action.Should().NotThrow<InvalidOperationException>();
            clock.IsRunning.Should().BeTrue();
            action = () => clock.Mode = ClockMode.StepHalfCycle;
            action.Should().Throw<InvalidOperationException>();
        }
    }
}
