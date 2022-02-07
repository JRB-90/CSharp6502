using CS6502.Core;
using System;

namespace CS6502.UIConsole.Models
{
    internal class MemoryModel
    {
        public MemoryModel(
            ushort ramStart,
            ushort ramEnd,
            ushort romStart,
            ushort romEnd,
            ushort vramStart,
            ushort vramEnd)
        {
            RamStart = ramStart;
            RamEnd = ramEnd;
            RomStart = romStart;
            RomEnd = romEnd;
            VramStart = vramStart;
            VramEnd = vramEnd;

            ROM =
                new GenericROM(
                    (ushort.MaxValue / 2) + 1,
                    16,
                    8
                );

            RAM =
                new GenericRAM(
                    (ushort.MaxValue / 2) + 1,
                    16,
                    8
                );
        }

        public ushort RamStart { get; }

        public ushort RamEnd { get; }

        public ushort RomStart { get; }

        public ushort RomEnd { get; }

        public ushort VramStart { get; }

        public ushort VramEnd { get; }

        public bool IsProgramLoaded { get; private set; }

        public GenericROM ROM { get; }

        public GenericRAM RAM { get; }

        public void LoadProgram(byte[] program)
        {
            lock (memoryLock)
            {
                ROM.LoadData(program);
                IsProgramLoaded = true;
            }
        }

        public byte[] GetVRAMCharData()
        {
            lock (memoryLock)
            {
                return 
                    GetGloballyAddressedBytes(
                        VramStart, 
                        (ushort)(VramEnd - VramStart)
                    );
            }
        }

        private byte[] GetGloballyAddressedBytes(
            ushort start, 
            ushort length)
        {
            lock (memoryLock)
            {
                byte[] bytes = new byte[length];

                if (start >= RamStart &&
                    start <= RamEnd)
                {
                    Array.Copy(
                        RAM.Data, 
                        start - RamStart, 
                        bytes, 
                        0, 
                        length
                    );
                }
                else
                {
                    Array.Copy(
                        ROM.Data,
                        start - RomStart,
                        bytes,
                        0,
                        length
                    );
                }

                return bytes;
            }
        }

        private static object memoryLock = new object();
    }
}
