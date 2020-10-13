using System.Views;

namespace Sample.Scenarios.View
{
    sealed class SampleApplication : Application
    {
        public static SampleApplication Instance { get; } = new();

        SampleApplication()
        {
        }

        protected override SampleMainView CreateRootView()
        {
            return new();
        }
    }
}
