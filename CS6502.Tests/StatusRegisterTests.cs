using CS6502.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS6502.Tests
{
    [TestClass]
    public class StatusRegisterTests
    {
        [TestMethod]
        public void CanConstruct()
        {
            StatusRegister status = new StatusRegister();
        }

        [TestMethod]
        public void CanConstructWithArguments()
        {
            StatusRegister status = new StatusRegister();
            status.Value.Should().Be(0x00);

            status = new StatusRegister(0xFF);
            status.Value.Should().Be(0xFF);
        }

        [TestMethod]
        public void CanToggleBits()
        {
            StatusRegister status = new StatusRegister(0xFF);

            status.CarryFlag.Should().BeTrue();
            status.ZeroFlag.Should().BeTrue();
            status.IrqFlag.Should().BeTrue();
            status.DecimalFlag.Should().BeTrue();
            status.BrkFlag.Should().BeTrue();
            status.OverflowFlag.Should().BeTrue();
            status.NegativeFlag.Should().BeTrue();

            status.CarryFlag = false;
            status.CarryFlag.Should().BeFalse();
            status.ZeroFlag = false;
            status.ZeroFlag.Should().BeFalse();
            status.IrqFlag = false;
            status.IrqFlag.Should().BeFalse();
            status.DecimalFlag = false;
            status.DecimalFlag.Should().BeFalse();
            status.BrkFlag = false;
            status.BrkFlag.Should().BeFalse();
            status.OverflowFlag = false;
            status.OverflowFlag.Should().BeFalse();
            status.NegativeFlag = false;
            status.NegativeFlag.Should().BeFalse();

            status.CarryFlag = true;
            status.CarryFlag.Should().BeTrue();
            status.ZeroFlag = true;
            status.ZeroFlag.Should().BeTrue();
            status.IrqFlag = true;
            status.IrqFlag.Should().BeTrue();
            status.DecimalFlag = true;
            status.DecimalFlag.Should().BeTrue();
            status.BrkFlag = true;
            status.BrkFlag.Should().BeTrue();
            status.OverflowFlag = true;
            status.OverflowFlag.Should().BeTrue();
            status.NegativeFlag = true;
            status.NegativeFlag.Should().BeTrue();
        }
    }
}
