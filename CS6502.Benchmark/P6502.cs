using CS6502.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CS6502.Benchmark
{
    public static class P6502
    {
        [DllImport("p6502", EntryPoint = "SetupChipAndMemory")]
        public static extern int SetupChipAndMemory(string path);

        [DllImport("p6502", EntryPoint = "DestroyChipAndMemory")]
        public static extern int DestroyChipAndMemory();

        [DllImport("p6502", EntryPoint = "StepCpu")]
        public static extern int StepCpu();

        [DllImport("p6502", EntryPoint = "GetRW")]
        public static extern int GetRW();

        [DllImport("p6502", EntryPoint = "GetA")]
        public static extern int GetA();

        [DllImport("p6502", EntryPoint = "GetX")]
        public static extern int GetX();

        [DllImport("p6502", EntryPoint = "GetY")]
        public static extern int GetY();

        [DllImport("p6502", EntryPoint = "GetIR")]
        public static extern int GetIR();

        [DllImport("p6502", EntryPoint = "GetP")]
        public static extern int GetP();

        [DllImport("p6502", EntryPoint = "GetSP")]
        public static extern int GetSP();

        [DllImport("p6502", EntryPoint = "GetPC")]
        public static extern int GetPC();

        [DllImport("p6502", EntryPoint = "GetAddress")]
        public static extern int GetAddress();

        [DllImport("p6502", EntryPoint = "GetData")]
        public static extern int GetData();

        public static CycleState GetCurrentState(int cycleID)
        {
            byte rw = (byte)CheckForError(GetRW());
            byte a = (byte)CheckForError(GetA());
            byte x = (byte)CheckForError(GetX());
            byte y = (byte)CheckForError(GetY());
            byte ir = (byte)CheckForError(GetIR());
            byte p = (byte)CheckForError(GetP());
            byte sp = (byte)CheckForError(GetSP());
            ushort pc = (ushort)CheckForError(GetPC());
            ushort address = (ushort)CheckForError(GetAddress());
            byte data = (byte)CheckForError(GetData());

            return
                new CycleState(
                    cycleID,
                    rw,
                    a,
                    x,
                    y,
                    ir,
                    p,
                    sp,
                    pc,
                    address,
                    data
                );
        }

        private static int CheckForError(int returnValue)
        {
            if (returnValue < 0)
            {
                throw new InvalidOperationException("Error in getting value from p6502");
            }
            else
            {
                return returnValue;
            }
        }
    }
}
