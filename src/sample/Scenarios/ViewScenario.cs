using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Views;
using Sample.Scenarios.View;

namespace Sample.Scenarios
{
    [SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Used.")]
    sealed class ViewScenario : IScenario
    {
        public async Task RunAsync()
        {
            await Task.Yield();

            await Application.RunAsync(SampleApplication.Instance).ConfigureAwait(false);
        }
    }
}
