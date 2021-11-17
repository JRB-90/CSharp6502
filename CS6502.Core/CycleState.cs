using System;

namespace CS6502.Core
{
    /// <summary>
    /// Represents the state of the cpu after a cycle.
    /// </summary>
    public class CycleState
    {
        public CycleState(
            int cycleID,
            byte rw,
            byte a,
            byte x,
            byte y,
            byte ir,
            byte p,
            byte sp,
            ushort pc,
            ushort address,
            byte data)
        {
            CycleID = cycleID;
            RW = rw;
            A = a;
            X = x;
            Y = y;
            IR = ir;
            P = p;
            SP = sp;
            PC = pc;
            Address = address;
            Data = data;
        }

        public CycleState(string csvLine)
        {
            FromCsvLine(csvLine);
        }

        public int CycleID { get; private set; }

        public byte RW { get; private set; }

        public byte A { get; private set; }

        public byte X { get; private set; }

        public byte Y { get; private set; }

        public byte IR { get; private set; }

        public byte P { get; private set; }

        public byte SP { get; private set; }

        public ushort PC { get; private set; }

        public ushort Address { get; private set; }

        public byte Data { get; private set; }

        public void FromCsvLine(string line)
        {
            string[] tokens = line.Split(',');

            CycleID = int.Parse(tokens[0]);
            RW = byte.Parse(tokens[1]);
            A = byte.Parse(tokens[2]);
            X = byte.Parse(tokens[3]);
            Y = byte.Parse(tokens[4]);
            IR = byte.Parse(tokens[5]);
            P = byte.Parse(tokens[6]);
            SP = byte.Parse(tokens[7]);
            PC = ushort.Parse(tokens[8]);
            Address = ushort.Parse(tokens[9]);
            Data = byte.Parse(tokens[10]);
        }

        public ComparisonResult Compare(CycleState cycleState)
        {
            if (CycleID != cycleState.CycleID)
            {
                throw new ArgumentException(
                    $"Cannot compare cycles, ID's do not macth {CycleID}<->{cycleState.CycleID}"
                );
            }

            return
                new ComparisonResult(
                    CycleID,
                    RW == cycleState.RW,
                    A == cycleState.A,
                    X == cycleState.X,
                    Y == cycleState.Y,
                    IR == cycleState.IR,
                    P == cycleState.P,
                    SP == cycleState.SP,
                    PC == cycleState.PC,
                    Address == cycleState.Address,
                    Data == cycleState.Data
                );
        }
    }
}
