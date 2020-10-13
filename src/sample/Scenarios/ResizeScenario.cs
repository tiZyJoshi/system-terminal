using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Scenarios
{
    [SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Used.")]
    sealed class ResizeScenario : IScenario
    {
        public async Task RunAsync()
        {
            await Task.Yield();

            Terminal.OutLine("Listening for resize events.");
            Terminal.OutLine();

            Terminal.Resize += (sender, e) =>
                Terminal.OutLine("Width = {0}, Height = {1}", e.Size.Width, e.Size.Height);

            await Task.Delay(Timeout.Infinite).ConfigureAwait(false);
        }
    }
}
