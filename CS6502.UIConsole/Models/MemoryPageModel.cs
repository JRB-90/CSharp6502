using System;
using System.Collections.Generic;

namespace CS6502.UIConsole.Models
{
    internal class MemoryPageModel
    {
        public MemoryPageModel(
            ushort startAddress, 
            byte[] pageValues)
        {
            if (pageValues.Length != 256)
            {
                throw new ArgumentException("Page data must contain a whole page (256b) of data");
            }

            List<MemoryAddressModel[]> pageData = new List<MemoryAddressModel[]>();

            for (int i = 0; i < 16; i++)
            {
                MemoryAddressModel[] values = new MemoryAddressModel[16];

                for (int j = 0; j < 16; j++)
                {
                    int index = i * 16 + j;
                    values[j] = 
                        new MemoryAddressModel(
                            (ushort)(startAddress + index), 
                            pageValues[index]
                        );
                }

                pageData.Add(values);
            }

            PageData = pageData;
        }

        public IReadOnlyList<MemoryAddressModel[]> PageData { get; }
    }
}
