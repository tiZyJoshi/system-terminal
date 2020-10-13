using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Views
{
    public abstract class DialogView<TApplication, TState, TResult> : View<TApplication, TState>
        where TApplication : Application
    {
        public bool IsComplete => _result == null;

        public TResult Result => (_result ?? throw new InvalidOperationException("Dialog was not completed.")).Value!;

        volatile StrongBox<TResult>? _result;

        protected DialogView(TApplication application)
            : base(application)
        {
        }

        protected void Complete(TResult result)
        {
            _result = new(result);

            Close();
        }

        public bool Query()
        {
            Open();

            Dispatcher.Completion.GetAwaiter().GetResult();

            return IsComplete;
        }

        public async ValueTask<bool> QueryAsync()
        {
            Open();

            await Dispatcher.Completion.ConfigureAwait(true);

            return IsComplete;
        }
    }
}
