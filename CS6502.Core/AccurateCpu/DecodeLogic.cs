namespace CS6502.Core
{
    /// <summary>
    /// Class to represent the decoding logic ROM inside a 6502 CPU.
    /// It is responsible for decoding the instruction and then
    /// sequencing the rest of the register transfers inside the
    /// chip to execute the instruction correctly.
    /// </summary>
    internal class DecodeLogic
    {
        public DecodeLogic()
        {
            RW = RWState.Read;
            Sync = EnableState.Disabled;
            LatchIR(0x00);
        }

        public byte IR => currentInstruction.Opcode;

        public RWState RW { get; private set; }

        public EnableState Sync { get; private set; }

        public RTInstruction Cycle(SignalEdge signalEdge)
        {
            // TODO

            return new RTInstruction();
        }

        public void LatchIR(byte value)
        {
            currentInstruction = InstructionDecoder.DecodeOpcode(value);
            instructionCycleCounter = 0;
        }

        private IInstruction currentInstruction;
        private int instructionCycleCounter;
        private DecodeState state;
    }
}
