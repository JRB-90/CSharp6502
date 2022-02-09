using CS6502.UIConsole.Models;
using CS6502.UIConsole.Shared;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace CS6502.UIConsole.ViewModels
{
    internal class ClockControlViewModel : ViewModelBase
    {
        readonly CS6502Model cpu;

        public ClockControlViewModel(
            CS6502Model cpu,
            double updateInterval,
            int minFreq,
            int maxFreq)
        {
            this.cpu = cpu;
            MinFreq = minFreq;
            MaxFreq = maxFreq;
            TargetFrequency = MaxFreq;
            ActualFrequency = -1;

            // Instantaneous
            cpu.RunFrequency
                .Sample(TimeSpan.FromMilliseconds(updateInterval))
                .Subscribe(freq => ActualFrequency = freq);

            //Rolling average(1s)
            //cpu.RunFrequency
            //    .BackBuffer(TimeSpan.FromMilliseconds(updateInterval))
            //    .Select(list => list.Average())
            //    .Subscribe(freq => ActualFrequency = (int)freq);
        }

        public int MinFreq { get; }

        public int MaxFreq { get; }

        public int TargetFrequency
        {
            get => targetFrequency;
            set
            {
                this.RaiseAndSetIfChanged(ref targetFrequency, value, nameof(TargetFrequency));
                cpu.SetTargetFrequency(targetFrequency);
            }
        }

        public int ActualFrequency
        {
            get => actualFrequency;
            set => this.RaiseAndSetIfChanged(ref actualFrequency, value, nameof(ActualFrequency));
        }

        private int targetFrequency;
        private int actualFrequency;
    }
}
