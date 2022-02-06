using CS6502.UIConsole.Shared;
using System;
using System.IO;
using System.Threading;

namespace CS6502.UIConsole.ViewModels
{
    internal class CS6502ViewModel : ViewModelBase, IDisposable
    {
        const string PROG_PATH = @"C:\Development\Sim6502\asm\consoleasm\consoleTests.bin";

        public CS6502ViewModel()
        {
            Console = 
                new ConsoleViewModel(
                    320,
                    256,
                    8,
                    8
                );

            cpuManager = new CpuManager();
            cpuManager.LoadProgram(File.ReadAllBytes(PROG_PATH));

            CpuState =
                new CpuStateViewModel(
                    cpuManager.GetCurrentCycleState(),
                    () => Start(),
                    () => Stop(),
                    () => Reset()
                );
        }

        public void Dispose()
        {
            isRunning = false;
            cpuThread.Join();
        }

        public ConsoleViewModel Console { get; }

        public CpuStateViewModel CpuState { get; }

        private void CpuWorker()
        {
            cpuManager.Start();

            while (isRunning)
            {
                Console.SetCharData(cpuManager.GetVRAMCharData());
                CpuState.State = cpuManager.GetCurrentCycleState();
                Thread.Sleep(1 / 30);
            }

            cpuManager.Stop();
        }

        private void Start()
        {
            if (!isRunning)
            {
                isRunning = true;
                cpuThread = new Thread(CpuWorker);
                cpuThread.Start();
            }
        }

        private void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                cpuThread.Join();
            }
        }

        private void Reset()
        {
            Stop();
            cpuManager = new CpuManager();
            cpuManager.LoadProgram(File.ReadAllBytes(PROG_PATH));
            Console.SetCharData(cpuManager.GetVRAMCharData());
            CpuState.State = cpuManager.GetCurrentCycleState();
        }

        private CpuManager cpuManager;
        private Thread cpuThread;
        private bool isRunning;
    }
}
