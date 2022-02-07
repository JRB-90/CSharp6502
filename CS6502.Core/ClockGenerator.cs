using System;
using System.Threading;

namespace CS6502.Core
{
    /// <summary>
    /// Tool for generating clock signals for 6502 systems.
    /// </summary>
    public class ClockGenerator : IDisposable
    {
        public ClockGenerator()
        {
            TargetFrequency = -1;
            mode = ClockMode.StepHalfCycle;
            clk = new Pin(TriState.True);
            CLK = new Wire(WirePull.PullDown);
            CLK.ConnectPin(clk);
            SYNC_N = new Wire(WirePull.PullUp);
            rdy_n = new Pin(TriState.True);
            RDY_N = new Wire(WirePull.PullUp);
            RDY_N.ConnectPin(rdy_n);
        }

        public ClockGenerator(ClockMode mode)
        {
            TargetFrequency = -1;
            this.mode = mode;
            clk = new Pin(TriState.True);
            CLK = new Wire(WirePull.PullDown);
            CLK.ConnectPin(clk);
            SYNC_N = new Wire(WirePull.PullUp);
            rdy_n = new Pin(TriState.True);
            RDY_N = new Wire(WirePull.PullUp);
            RDY_N.ConnectPin(rdy_n);
        }

        public ClockGenerator(int targetFrequency)
        {
            TargetFrequency = targetFrequency;
            mode = ClockMode.FreeRunning;
            clk = new Pin(TriState.True);
            CLK = new Wire(WirePull.PullDown);
            CLK.ConnectPin(clk);
            SYNC_N = new Wire(WirePull.PullUp);
            rdy_n = new Pin(TriState.True);
            RDY_N = new Wire(WirePull.PullUp);
            RDY_N.ConnectPin(rdy_n);
        }

        public event EventHandler ClockTicked;

        public void Dispose()
        {
            if (mode == ClockMode.FreeRunning)
            {
                Stop();
            }
        }

        public int TargetFrequency { get; }

        public bool IsRunning => isRunning;

        public ClockMode Mode
        {
            get => mode;
            set
            {
                if (!isRunning)
                {
                    mode = value;
                }
                else
                {
                    throw new InvalidOperationException("Cannot change clock mode whilst running");
                }
            }
        }

        public Wire CLK { get; }

        public Wire SYNC_N
        {
            get => sync_n;
            set
            {
                sync_n = value;
            }
        }

        public Wire RDY_N { get; }

        public void Reset()
        {
            clk.State = TriState.False;
            if (mode == ClockMode.FreeRunning)
            {
                Stop();
            }
        }

        public void Cycle()
        {
            switch (mode)
            {
                case ClockMode.StepHalfCycle:
                    clk.State = clk.State == TriState.False ? TriState.True : TriState.False;
                    break;
                case ClockMode.StepFullCycle:
                    clk.State = clk.State == TriState.False ? TriState.True : TriState.False;
                    clk.State = clk.State == TriState.False ? TriState.True : TriState.False;
                    break;
                case ClockMode.StepInstruction:
                    rdy_n.State = TriState.True;
                    clk.State = clk.State == TriState.False ? TriState.True : TriState.False;

                    for (int i = 0; i < 100; i++)
                    {
                        if (SYNC_N.State == false)
                        {
                            rdy_n.State = TriState.False;

                            return;
                        }
                        else
                        {
                            clk.State = clk.State == TriState.False ? TriState.True : TriState.False;
                        }
                    }
                    throw new InvalidOperationException("Sync pin didn't pull low after 50 cycles, is it connected correctly?");
                default:
                    throw new InvalidOperationException($"Cannot manual cycle in {mode.ToString()} mode");
            }

            ClockTicked?.Invoke(this, new EventArgs());
        }

        public void Start()
        {
            if (mode == ClockMode.FreeRunning)
            {
                isRunning = true;
                clockThread = new Thread(ClockWorker);
                clockThread.Start();
            }
            else
            {
                throw new InvalidOperationException("Clock not in free running mode");
            }
        }

        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                clockThread.Join();
            }
        }

        private void ClockWorker()
        {
            long tickInterval = 0;
            long lastTime = DateTime.Now.Ticks; ;
            long currentTime;

            if (TargetFrequency > 0)
            {
                double period = 1.0 / (double)TargetFrequency;
                tickInterval = (long)((double)TimeSpan.TicksPerSecond * period);
            }

            while (isRunning)
            {
                currentTime = DateTime.Now.Ticks;
                long delta = currentTime - lastTime;

                if (delta >= tickInterval)
                {
                    clk.State = clk.State == TriState.False ? TriState.True : TriState.False;
                    lastTime = currentTime;
                    ClockTicked?.Invoke(this, new EventArgs());
                }

                Thread.Sleep(0);
            }
        }

        private bool isRunning;
        private Thread clockThread;
        private ClockMode mode;
        private Pin clk;
        private Pin rdy_n;
        private Wire sync_n;
    }
}
