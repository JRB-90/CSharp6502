using CS6502.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace CS6502.Tests
{
    [TestClass]
    public class MemoryTests
    {
        [TestMethod]
        public void CanConstructROM()
        {
            IROM rom = new GenericROM(0, 0, 0);
        }

        [TestMethod]
        public void CanConstructROMWithArguments()
        {
            IROM rom = new GenericROM(256, 8, 8);
            rom.Data.Should().HaveCount(256);
            for (int i = 0; i < rom.Data.Length; i++)
            {
                rom.Data[i].Should().Be(0xFF);
            }
        }

        [TestMethod]
        public void DoesROMReadWork()
        {
            IROM rom = new GenericROM(256, 8, 8);

            Pin cs_n = new Pin(TriState.True);
            Pin oe_n = new Pin(TriState.True);
            rom.CS_N.ConnectPin(cs_n);
            rom.OE_N.ConnectPin(oe_n);

            Bus addressBus = rom.AddressBus;
            Pin[] addressPins = addressBus.CreateAndConnectPinArray();

            addressPins.SetTo(0x00);
            rom.DataBus.ToByte().Should().Be(0x00);
            addressPins.SetTo(0xFF);
            rom.DataBus.ToByte().Should().Be(0x00);

            cs_n.State = TriState.False;
            addressPins.SetTo(0x00);
            rom.DataBus.ToByte().Should().Be(0x00);
            addressPins.SetTo(0xFF);
            rom.DataBus.ToByte().Should().Be(0x00);

            cs_n.State = TriState.True;
            oe_n.State = TriState.False;
            addressPins.SetTo(0x00);
            rom.DataBus.ToByte().Should().Be(0x00);
            addressPins.SetTo(0xFF);
            rom.DataBus.ToByte().Should().Be(0x00);

            cs_n.State = TriState.False;
            addressPins.SetTo(0x00);
            rom.DataBus.ToByte().Should().Be(0xFF);
            addressPins.SetTo(0xFF);
            rom.DataBus.ToByte().Should().Be(0xFF);
        }

        [TestMethod]
        public void DoesConstructFromFileWork()
        {
            string path = Path.GetTempPath() + "test.bin";
            byte[] data = CreateFileWithRandomData(path);

            IROM rom = new GenericROM(path, 8, 8);
            rom.Data.Should().HaveCount(256);

            for (int i = 0; i < data.Length; i++)
            {
                rom.Data[i].Should().Be(data[i]);
            }
        }

        private byte[] CreateFileWithRandomData(string path)
        {
            Random r = new Random();
            byte[] data = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                data[i] = (byte)r.Next(0x00, 0xFF);
            }
            File.WriteAllBytes(path, data);

            return data;
        }

        [TestMethod]
        public void CanConstructRAM()
        {
            IRAM ram = new GenericRAM(0, 0, 0);
        }

        [TestMethod]
        public void CanConstructRAMWithArguments()
        {
            IRAM ram = new GenericRAM(256, 8, 8);
            ram.Data.Should().HaveCount(256);
            for (int i = 0; i < ram.Data.Length; i++)
            {
                ram.Data[i].Should().Be(0x00);
            }
        }

        [TestMethod]
        public void DoesRAMReadWork()
        {
            IRAM ram = new GenericRAM(256, 8, 8);

            Pin cs_n = new Pin(TriState.True);
            Pin oe_n = new Pin(TriState.True);
            Pin rw_n = new Pin(TriState.True);
            ram.CS_N.ConnectPin(cs_n);
            ram.OE_N.ConnectPin(oe_n);
            ram.RW_N.ConnectPin(rw_n);

            Bus addressBus = ram.AddressBus;
            Pin[] addressPins = addressBus.CreateAndConnectPinArray();

            addressPins.SetTo(0x00);
            ram.DataBus.ToByte().Should().Be(0x00);
            addressPins.SetTo(0xFF);
            ram.DataBus.ToByte().Should().Be(0x00);

            cs_n.State = TriState.False;
            addressPins.SetTo(0x00);
            ram.DataBus.ToByte().Should().Be(0x00);
            addressPins.SetTo(0xFF);
            ram.DataBus.ToByte().Should().Be(0x00);

            cs_n.State = TriState.True;
            oe_n.State = TriState.False;
            addressPins.SetTo(0x00);
            ram.DataBus.ToByte().Should().Be(0x00);
            addressPins.SetTo(0xFF);
            ram.DataBus.ToByte().Should().Be(0x00);

            cs_n.State = TriState.False;
            addressPins.SetTo(0x00);
            ram.DataBus.ToByte().Should().Be(0x00);
            addressPins.SetTo(0xFF);
            ram.DataBus.ToByte().Should().Be(0x00);
        }

        [TestMethod]
        public void DoesRAMWriteWork()
        {
            IRAM ram = new GenericRAM(256, 8, 8);

            Pin cs_n = new Pin(TriState.True);
            Pin oe_n = new Pin(TriState.True);
            Pin rw_n = new Pin(TriState.True);
            ram.CS_N.ConnectPin(cs_n);
            ram.OE_N.ConnectPin(oe_n);
            ram.RW_N.ConnectPin(rw_n);

            Bus addressBus = ram.AddressBus;
            Pin[] addressPins = addressBus.CreateAndConnectPinArray();
            Pin[] dataPins = ram.DataBus.CreateAndConnectPinArray();

            addressPins.SetTo(0x00);
            ram.DataBus.ToByte().Should().Be(0x00);
            addressPins.SetTo(0xFF);
            ram.DataBus.ToByte().Should().Be(0x00);

            addressPins.SetTo(0x50);
            dataPins.SetTo(0x42);
            rw_n.State = TriState.False;
            rw_n.State = TriState.True;
            dataPins.SetAllTo(TriState.HighImpedance);
            ram.DataBus.ToByte().Should().Be(0x00);

            dataPins.SetTo(0x42);
            oe_n.State = TriState.False;
            rw_n.State = TriState.False;
            rw_n.State = TriState.True;
            dataPins.SetAllTo(TriState.HighImpedance);
            ram.DataBus.ToByte().Should().Be(0x00);

            dataPins.SetTo(0x42);
            cs_n.State = TriState.False;
            rw_n.State = TriState.False;
            rw_n.State = TriState.True;
            dataPins.SetAllTo(TriState.HighImpedance);
            ram.DataBus.ToByte().Should().Be(0x42);
        }
    }
}
