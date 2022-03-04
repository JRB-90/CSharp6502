using CS6502.Core;
using CS6502.UIConsole.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace CS6502.UIConsole.ViewModels
{
    internal class MemoryViewModel : ViewModelBase
    {
        readonly CS6502Model cpu;

        public MemoryViewModel(
            CS6502Model cpu,
            double updateInterval)
        {
            this.cpu = cpu;
            ShowCode();

            timer =
                new Timer(
                    new TimerCallback(UpdateMemoryView),
                    null,
                    0,
                    500
                );
        }

        public ObservableCollection<MemoryLineViewModel> MemoryLines
        {
            get => memoryLines;
            set => this.RaiseAndSetIfChanged(ref memoryLines, value);
        }

        public MemoryPageModel MemoryPage
        {
            get => memoryPage;
            set => this.RaiseAndSetIfChanged(ref memoryPage, value, nameof(MemoryPage));
        }

        public void ShowZP()
        {
            lock (displayLock)
            {
                currentAddressSpace = new AddressSpace(0x0000, 0x00FF);
                byte[] data = cpu.GetMemoryBlock(currentAddressSpace);

                MemoryLines =
                    new ObservableCollection<MemoryLineViewModel>(
                        CreateMemoryLines(data)
                    );
            }
        }

        public void ShowCode()
        {
            lock (displayLock)
            {
                currentAddressSpace = new AddressSpace(0x8000, 0x80FF);
                byte[] data = cpu.GetMemoryBlock(currentAddressSpace);

                MemoryLines =
                    new ObservableCollection<MemoryLineViewModel>(
                        CreateMemoryLines(data)
                    );
            }
        }

        private List<MemoryLineViewModel> CreateMemoryLines(byte[] data)
        {
            List<MemoryLineViewModel> memoryLines = new List<MemoryLineViewModel>();

            for (int i = 0; i < 16; i++)
            {
                memoryLines.Add(
                    new MemoryLineViewModel(
                        (ushort)(currentAddressSpace.StartAddress + (i * 16)),
                        data.Skip(i * 16).Take(16).ToArray()
                    )
                );
            }

            return memoryLines;
        }

        private void UpdateMemoryView(object state)
        {
            lock (displayLock)
            {
                byte[] data = cpu.GetMemoryBlock(currentAddressSpace);

                for (int i = 0; i < 16; i++)
                {
                    memoryLines[i].UpdateValues(
                        data.Skip(i * 16).Take(16).ToArray()
                    );
                }

                //MemoryPage =
                //    new MemoryPageModel(
                //        (ushort)currentAddressSpace.StartAddress,
                //        data
                //    );
            }
        }

        private Timer timer;
        private MemoryPageModel memoryPage;
        private AddressSpace currentAddressSpace;
        private ObservableCollection<MemoryLineViewModel> memoryLines;

        private static object displayLock = new object();
    }
}
