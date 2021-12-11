using CS6502.Core;
using System;
using Alba.CsConsoleFormat;

namespace CS6502.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicCpuSystem system =
                new BasicCpuSystem("C:\\Development\\Sim6502\\asm\\asmtest\\build\\main.bin");

            int memStart = 0x8000;
            int memEnd = 0x800F;
            var mem = system.GetCurrentMemoryState(new AddressSpace(0x8000, 0x801F));

            var doc =
                new Document(
                    new Grid
                    {
                        Color = ConsoleColor.Gray,
                        Columns =
                        {
                            GridLength.Auto,
                            GridLength.Auto,
                            GridLength.Auto,
                            GridLength.Auto,
                            GridLength.Auto,
                            GridLength.Auto,
                            GridLength.Auto,
                            GridLength.Auto,
                            GridLength.Auto
                        },
                        Children = 
                        {
                            GetCellsForMemory(memStart, mem)
                        }
                    }
                );

            ConsoleRenderer.RenderDocument(doc);

            ConsoleKeyInfo key = System.Console.ReadKey(true);

            while (key.Key != ConsoleKey.Q)
            {
                if (key.Key == ConsoleKey.Spacebar)
                {
                    system.Cycle(false);
                }

                ConsoleRenderer.RenderDocument(doc);
                key = System.Console.ReadKey(true);
            }
        }

        static IEnumerable<Cell> GetCellsForMemory(int memStart, byte[] data)
        {
            List<Cell> cells = new List<Cell>();
            ushort currentAddress = (ushort)memStart;

            for (int i = 0; i < data.Length; i ++)
            {
                if (i % 8 == 0)
                {
                    cells.Add(new Cell(currentAddress.ToHexString()) { Color = ConsoleColor.Cyan });
                }

                cells.Add(new Cell(data[i].ToHexString()));
                currentAddress++;
            }

            return cells;
        }
    }
}
