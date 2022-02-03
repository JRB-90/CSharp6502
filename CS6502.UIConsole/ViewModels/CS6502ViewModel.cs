using CS6502.UIConsole.Shared;
using System;
using System.IO;
using System.Threading;

namespace CS6502.UIConsole.ViewModels
{
    public class CS6502ViewModel : ViewModelBase, IDisposable
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

            isRunning = true;
            cpuThread = new Thread(CpuWorker);
            cpuThread.Start();
        }

        public void Dispose()
        {
            isRunning = false;
            cpuThread.Join();
        }

        public ConsoleViewModel Console { get; }

        private void CpuWorker()
        {
            cpuManager.Start();

            while (isRunning)
            {
                Console.SetCharData(cpuManager.GetVRAMCharData());
                Thread.Sleep(1 / 30);
            }

            cpuManager.Stop();
        }

        private CpuManager cpuManager;
        private Thread cpuThread;
        private bool isRunning;
    }
}
