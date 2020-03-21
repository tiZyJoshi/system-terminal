using System;
using System.Threading.Tasks;

namespace Sample.Scenarios
{
    sealed class RawScenario : IScenario
    {
        public Task RunAsync()
        {
            Terminal.OutLine("Entering raw mode.");
            Terminal.OutLine();

            Terminal.SetRawMode(true, true);

            try
            {
                for (var i = 0; i < 25; i++)
                {
                    Terminal.Out("0x{0:x2}", Terminal.ReadRaw());
                    Terminal.Out("\r\n");
                }
            }
            finally
            {
                Terminal.SetRawMode(false, true);
            }

            return Task.CompletedTask;
        }
    }
}