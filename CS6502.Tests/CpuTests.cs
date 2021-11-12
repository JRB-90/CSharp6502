using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace CS6502.Tests
{
    [TestClass]
    public class CpuTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            int theAnswer = 42;
            theAnswer.Should().Be(42);
        }
    }
}
