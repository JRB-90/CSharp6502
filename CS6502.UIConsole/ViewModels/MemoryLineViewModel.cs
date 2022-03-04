using CS6502.Core;
using ReactiveUI;
using System;
using System.Linq;

namespace CS6502.UIConsole.ViewModels
{
    internal class MemoryLineViewModel : ViewModelBase
    {
        public MemoryLineViewModel(
            ushort address,
            byte[] values)
        {
            AddressHexString = address.ToHexString();
            UpdateValues(values);
        }

        public string AddressHexString { get; }

        public string[] ValueHexStrings
        {
            get => valueHexStrings;
            private set => this.RaiseAndSetIfChanged(ref valueHexStrings, value);
        }

        public void UpdateValues(byte[] values)
        {
            if (values.Length != 16)
            {
                throw new ArgumentException("Incorrect number of values passed, should be 16");
            }

            ValueHexStrings = 
                values
                .Select(v => v.ToHexString())
                .ToArray();
        }

        private string[] valueHexStrings;
    }
}
