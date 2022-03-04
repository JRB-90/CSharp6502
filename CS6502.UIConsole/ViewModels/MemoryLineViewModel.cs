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
            valueHexStrings = new string[16];
            UpdateValues(values);
        }

        public string AddressHexString { get; }

        public string HexValue0
        {
            get => valueHexStrings[0];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[0], value, nameof(HexValue0));
        }

        public string HexValue1
        {
            get => valueHexStrings[1];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[1], value, nameof(HexValue1));
        }

        public string HexValue2
        {
            get => valueHexStrings[2];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[2], value, nameof(HexValue2));
        }

        public string HexValue3
        {
            get => valueHexStrings[3];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[3], value, nameof(HexValue3));
        }

        public string HexValue4
        {
            get => valueHexStrings[4];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[4], value, nameof(HexValue4));
        }

        public string HexValue5
        {
            get => valueHexStrings[5];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[5], value, nameof(HexValue5));
        }

        public string HexValue6
        {
            get => valueHexStrings[6];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[6], value, nameof(HexValue6));
        }

        public string HexValue7
        {
            get => valueHexStrings[7];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[7], value, nameof(HexValue7));
        }

        public string HexValue8
        {
            get => valueHexStrings[8];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[8], value, nameof(HexValue8));
        }

        public string HexValue9
        {
            get => valueHexStrings[9];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[9], value, nameof(HexValue9));
        }

        public string HexValue10
        {
            get => valueHexStrings[10];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[10], value, nameof(HexValue10));
        }

        public string HexValue11
        {
            get => valueHexStrings[11];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[11], value, nameof(HexValue11));
        }

        public string HexValue12
        {
            get => valueHexStrings[12];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[12], value, nameof(HexValue12));
        }

        public string HexValue13
        {
            get => valueHexStrings[13];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[13], value, nameof(HexValue13));
        }

        public string HexValue14
        {
            get => valueHexStrings[14];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[14], value, nameof(HexValue14));
        }

        public string HexValue15
        {
            get => valueHexStrings[15];
            set => this.RaiseAndSetIfChanged(ref valueHexStrings[15], value, nameof(HexValue15));
        }

        public void UpdateValues(byte[] values)
        {
            if (values.Length != 16)
            {
                throw new ArgumentException("Incorrect number of values passed, should be 16");
            }

            HexValue0 = values[0].ToHexString();
            HexValue1 = values[1].ToHexString();
            HexValue2 = values[2].ToHexString();
            HexValue3 = values[3].ToHexString();
            HexValue4 = values[4].ToHexString();
            HexValue5 = values[5].ToHexString();
            HexValue6 = values[6].ToHexString();
            HexValue7 = values[7].ToHexString();
            HexValue8 = values[8].ToHexString();
            HexValue9 = values[9].ToHexString();
            HexValue10 = values[10].ToHexString();
            HexValue11 = values[11].ToHexString();
            HexValue12 = values[12].ToHexString();
            HexValue13 = values[13].ToHexString();
            HexValue14 = values[14].ToHexString();
            HexValue15 = values[15].ToHexString();
        }

        private string[] valueHexStrings;
    }
}
