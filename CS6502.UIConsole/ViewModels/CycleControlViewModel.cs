using Avalonia.Metadata;
using CS6502.UIConsole.Models;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace CS6502.UIConsole.ViewModels
{
    internal class CycleControlViewModel : ViewModelBase
    {
        readonly CS6502Model cpu;

        public CycleControlViewModel(CS6502Model cpu)
        {
            this.cpu = cpu;
            cpu.IsRunning
                .Subscribe(value =>
                    {
                        runStatus = value;
                        this.RaisePropertyChanged(nameof(RunStatus));
                    }
                );
        }

        public string RunStatus => runStatus ? "Running" : "Halted";

        [DependsOn(nameof(RunStatus))]
        public bool CanStart(object param)
        {
            return !runStatus;
        }

        [DependsOn(nameof(RunStatus))]
        public bool CanStop(object param)
        {
            return runStatus;
        }

        [DependsOn(nameof(RunStatus))]
        public bool CanReset(object param)
        {
            return !runStatus;
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

        private bool runStatus;
    }
}
