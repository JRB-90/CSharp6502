using System;

namespace CS6502.Core
{
    internal abstract class StoreInstructionBase : MemoryInstructionBase
    {
        public StoreInstructionBase(
            string name, 
            byte opcode, 
            AddressingMode addressingMode,
            MicroCodeInstruction storeInstruction) 
          : 
            base(name, opcode, addressingMode, RWState.Write, storeInstruction)
        {
            this.storeInstruction = storeInstruction;
        }

        private MicroCodeInstruction storeInstruction;
    }
}
