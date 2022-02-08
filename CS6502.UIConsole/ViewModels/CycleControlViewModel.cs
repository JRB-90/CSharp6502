using Avalonia.Controls;
using Avalonia.Metadata;
using CS6502.UIConsole.Models;
using CS6502.UIConsole.Shared;
using ReactiveUI;

namespace CS6502.UIConsole.ViewModels
{
    internal class CycleControlViewModel : ViewModelBase
    {
        readonly DialogService dialogService;
        readonly CS6502Model cpu;

        public CycleControlViewModel(
            DialogService dialogService,
            CS6502Model cpu)
        {
            this.dialogService = dialogService;
            this.cpu = cpu;
            cpu.RunStateChanged += Cpu_RunStateChanged;
            cpu.ProgramLoadedChanged += Cpu_ProgramLoadedChanged;
        }

        public string RunStatus => 
            cpu.IsRunning ? "Running" : "Halted";

        [DependsOn(nameof(RunStatus))]
        public bool CanLoadProgram(object param)
        {
            return !cpu.IsRunning;
        }

        [DependsOn(nameof(RunStatus))]
        public bool CanStart(object param)
        {
            return !cpu.IsRunning && cpu.IsProgramLoaded;
        }

        [DependsOn(nameof(RunStatus))]
        public bool CanStop(object param)
        {
            return cpu.IsRunning;
        }

        [DependsOn(nameof(RunStatus))]
        public bool CanReset(object param)
        {
            return !cpu.IsRunning && cpu.IsProgramLoaded; ;
        }

        public async void LoadProgram()
        {
            var result =
                await dialogService.ShowOpenFileDialogAsync(
                    "Open CPU Program",
                    new FileDialogFilter()
                    {
                        Name = "Binary",
                        Extensions = { "bin" }
                    }
                );

            cpu.LoadProgram(result);
        }

        public void Start()
        {
            cpu.Start();
        }

        public void Stop()
        {
            cpu.Stop();
        }

        public void Reset()
        {
            cpu.Reset();
        }

        private void Cpu_RunStateChanged(object? sender, bool e)
        {
            this.RaisePropertyChanged(nameof(RunStatus));
        }

        private void Cpu_ProgramLoadedChanged(object? sender, bool e)
        {
            this.RaisePropertyChanged(nameof(RunStatus));
        }
    }
}
