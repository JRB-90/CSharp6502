namespace CS6502.Core
{
    internal abstract class LoadInstructionBase : MemoryInstructionBase
    {
        public LoadInstructionBase(
            string name, 
            byte opcode, 
            AddressingMode addressingMode,
            MicroCodeInstruction loadInstruction) 
          :
            base(name, opcode, addressingMode, RWState.Read, loadInstruction)
        {
            this.loadInstruction = loadInstruction;
        }

        private MicroCodeInstruction loadInstruction;
    }
}
