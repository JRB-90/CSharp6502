using CS6502.UIConsole.Models;
using CS6502.UIConsole.Shared;

namespace CS6502.UIConsole.ViewModels
{
    internal class CS6502ViewModel : ViewModelBase
    {
        const double UPDATE_FREQ_HZ = 60.0;
        const double UPDATE_INTERVAL_MS = (1.0 / UPDATE_FREQ_HZ) * 1000.0;
        const int MIN_FREQ = 1;
        const int MAX_FREQ = 10000;

        readonly DialogService dialogService;
        readonly CS6502Model cpu;

        public CS6502ViewModel(DialogService dialogService)
        {
            this.dialogService = dialogService;
            cpu = new CS6502Model();

            Console = 
                new ConsoleViewModel(
                    cpu,
                    UPDATE_INTERVAL_MS,
                    320,
                    256,
                    8,
                    8
                );

            CpuState =
                new CpuStateViewModel(
                    cpu, 
                    UPDATE_INTERVAL_MS
                );

            CycleControl =
                new CycleControlViewModel(
                    dialogService,
                    cpu
                );

            ClockControl =
                new ClockControlViewModel(
                    cpu,
                    UPDATE_INTERVAL_MS,
                    MIN_FREQ,
                    MAX_FREQ
                );

            MemoryView = 
                new MemoryViewModel(
                    cpu,
                    UPDATE_INTERVAL_MS
                );
        }

        public ConsoleViewModel Console { get; }

        public CpuStateViewModel CpuState { get; }

        public CycleControlViewModel CycleControl { get; }

        public ClockControlViewModel ClockControl { get; }

        public MemoryViewModel MemoryView { get; }
    }
}
