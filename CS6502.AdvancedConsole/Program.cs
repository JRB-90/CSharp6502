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
                new BasicCpuSystem(
                    MemoryTools.LoadDataFromFile("C:\\Development\\Sim6502\\asm\\asmtest\\build\\loadStoreTests.bin")
                );

            uint memStart = 0x0000;
            uint memEnd = 0x001F;

            var mem = 
                system.GetCurrentMemoryState(
                    new AddressSpace(memStart, memEnd)
                );

            var state = system.GetCurrentCycleState();

            var doc = BuildDoc(memStart, mem, state, system.ClockGenerator.Mode);
            ConsoleRenderer.RenderDocument(doc);

            ConsoleKeyInfo key = System.Console.ReadKey(true);

            while (key.Key != ConsoleKey.Q)
            {
                if (key.Key == ConsoleKey.Spacebar)
                {
                    system.Cycle(false);
                }
                else if (key.Key == ConsoleKey.H)
                {
                    system.ClockGenerator.Mode = ClockMode.StepHalfCycle;
                }
                else if (key.Key == ConsoleKey.F)
                {
                    system.ClockGenerator.Mode = ClockMode.StepFullCycle;
                }
                else if (key.Key == ConsoleKey.I)
                {
                    system.ClockGenerator.Mode = ClockMode.StepInstruction;
                }

                mem =
                    system.GetCurrentMemoryState(
                        new AddressSpace(memStart, memEnd)
                    );

                state = system.GetCurrentCycleState();

                doc = BuildDoc(memStart, mem, state, system.ClockGenerator.Mode);
                System.Console.Clear();
                ConsoleRenderer.RenderDocument(doc);

                key = System.Console.ReadKey(true);
            }
        }

        static Document BuildDoc(uint memStart, byte[] mem, CycleState state, ClockMode clockMode)
        {
            return
                new Document(
                    new Div()
                    {
                        Margin = new Thickness(0, 0, 0, 1),
                        Children =
                        {
                            new Grid()
                            {
                                Color = ConsoleColor.Gray,
                                Columns =
                                {
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1)
                                },
                                Children =
                                {
                                    GetCellsFromMemory((int)memStart, mem)
                                }
                            }
                        }
                    },
                    new Div()
                    {
                        Margin = new Thickness(0, 0, 0, 1),
                        Children =
                        {
                            new Grid()
                            {
                                Color= ConsoleColor.Gray,
                                Columns =
                                {
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1),
                                    GridLength.Star(1)
                                },
                                Children =
                                {
                                    GetCellsFromState(state)
                                }
                            }
                        }
                    },
                    new Div()
                    {
                        Children =
                        {
                            new Stack()
                            {
                                Orientation = Orientation.Horizontal,
                                Children =
                                {
                                    new Span("Step Mode -> "),
                                    new Span(clockMode.ToString())
                                }
                            }
                        }
                    }
                );
        }

        static IEnumerable<Cell> GetCellsFromMemory(int memStart, byte[] data)
        {
            List<Cell> cells = new List<Cell>();
            ushort currentAddress = (ushort)memStart;

            cells.Add(new Cell("Memory State") { Color = ConsoleColor.DarkCyan, ColumnSpan = 17, Align = Align.Center });
            cells.Add(new Cell("Page") { Color = ConsoleColor.DarkCyan, Align = Align.Center} );

            for (int i = 0; i < 16; i++)
            {
                cells.Add(new Cell($"{i:X}") { Color = ConsoleColor.Cyan, Align = Align.Center });
            }

            for (int i = 0; i < data.Length; i ++)
            {
                if (i % 16 == 0)
                {
                    cells.Add(new Cell(currentAddress.ToHexString()) { Color = ConsoleColor.Cyan, Align = Align.Center });
                }

                cells.Add(new Cell(data[i].ToHexString()) { Align = Align.Center });
                currentAddress++;
            }

            return cells;
        }

        static IEnumerable<Cell> GetCellsFromState(CycleState state)
        {
            List<Cell> cells = new List<Cell>();
            cells.Add(new Cell("CPU State") { Color = ConsoleColor.DarkCyan, ColumnSpan = 10, Align = Align.Center });

            cells.Add(new Cell("Cycle") { Color = ConsoleColor.Cyan });
            cells.Add(new Cell("RW") { Color = ConsoleColor.Cyan });
            cells.Add(new Cell("A") { Color = ConsoleColor.Cyan });
            cells.Add(new Cell("X") { Color = ConsoleColor.Cyan });
            cells.Add(new Cell("Y") { Color = ConsoleColor.Cyan });
            cells.Add(new Cell("IR") { Color = ConsoleColor.Cyan });
            cells.Add(new Cell("SP") { Color = ConsoleColor.Cyan });
            cells.Add(new Cell("PC") { Color = ConsoleColor.Cyan });
            cells.Add(new Cell("ADDR") { Color = ConsoleColor.Cyan });
            cells.Add(new Cell("DATA") { Color = ConsoleColor.Cyan });

            cells.Add(new Cell(state.CycleID));
            cells.Add(new Cell((state.RW == 0 ? "W" : "R")));
            cells.Add(new Cell(state.A.ToHexString()));
            cells.Add(new Cell(state.X.ToHexString()));
            cells.Add(new Cell(state.Y.ToHexString()));
            cells.Add(new Cell(state.IR.ToHexString()));
            cells.Add(new Cell(state.SP.ToHexString()));
            cells.Add(new Cell(state.PC.ToHexString()));
            cells.Add(new Cell(state.Address.ToHexString()));
            cells.Add(new Cell(state.Data.ToHexString()));

            return cells;
        }
    }
}
