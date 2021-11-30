using System.Collections.Generic;
using System.Linq;

namespace CS6502.Core
{
    /// <summary>
    /// Represents a set of internal instructions for conducting
    /// a register transfer inside the CPU.
    /// </summary>
    internal class CpuMicroCode
    {
        public CpuMicroCode()
        {
            MicroCode = new List<MicroCodeInstruction>();
        }

        public CpuMicroCode(List<MicroCodeInstruction> microCode)
        {
            MicroCode = microCode;
        }

        public CpuMicroCode(params MicroCodeInstruction[] microCodes)
        {
            var microCode = new List<MicroCodeInstruction>();

            for (int i = 0; i < microCodes.Length; i++)
            {
                microCode.Add(microCodes[i]);
            }

            MicroCode = microCode;
        }

        public void Add(MicroCodeInstruction microCode)
        {
            var microCodes = MicroCode.ToList();
            microCodes.Add(microCode);
            MicroCode = microCodes;
        }

        public static CpuMicroCode operator +(CpuMicroCode a, CpuMicroCode b) =>
            new CpuMicroCode(a.MicroCode.Concat(b.MicroCode).ToList());

        public IReadOnlyList<MicroCodeInstruction> MicroCode { get; private set; }
    }
}
