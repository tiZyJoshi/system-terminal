using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Views.Threading;
using System.Views.Visuals;

namespace System.Views
{
    public abstract class View
    {
        public Application Application { get; }

        public Dispatcher Dispatcher { get; }

        internal LinkedListNode<View>? Node { get; set; }

        public ViewState State => (ViewState)_state;

        volatile int _state;

        private protected View(Application application)
        {
            Application = application ?? throw new ArgumentNullException(nameof(application));
            Dispatcher = new(this);
        }

        public void Open()
        {
            // TODO: This has a race condition.
            if (Application.IsStopping)
                throw new InvalidOperationException("Cannot open view after application has shut down.");

            switch ((ViewState)Interlocked.CompareExchange(ref _state, (int)ViewState.Opened, (int)ViewState.Created))
            {
                case ViewState.Opened:
                    return;
                case ViewState.Closed:
                    throw new InvalidOperationException("View cannot be opened again.");
            }

            Application.Views.Open(this);
            Dispatcher.Start();
            Dispatcher.Post(Opening);

            // Start rendering.
            Refresh(false);
        }

        public void Close()
        {
            switch ((ViewState)Interlocked.CompareExchange(ref _state, (int)ViewState.Closed, (int)ViewState.Opened))
            {
                case ViewState.Created:
                    throw new InvalidOperationException("View cannot be closed before being opened.");
                case ViewState.Closed:
                    return;
            }

            Dispatcher.Post(Closing);
            Dispatcher.Stop();
            Application.Views.Close(this);
        }

        public abstract void Refresh(bool full);

        protected virtual void Opening()
        {
        }

        protected virtual void Closing()
        {
        }
    }

    public abstract class View<TApplication, TState, TMessage> : View
        where TApplication : Application
    {
        public new TApplication Application => (TApplication)base.Application;

        TState? _state;

        bool _initialized;

        PageInstance? _instance;

        protected View(TApplication application)
            : base(application)
        {
        }

        protected virtual TState Initialize()
        {
            return InitializeAsync().Preserve().GetAwaiter().GetResult();
        }

        protected virtual ValueTask<TState> InitializeAsync()
        {
            return TerminalUtility.CreateValueTask(Initialize);
        }

        protected virtual TState Update(TState state, TMessage message)
        {
            return UpdateAsync(state, message).Preserve().GetAwaiter().GetResult();
        }

        protected virtual ValueTask<TState> UpdateAsync(TState state, TMessage message)
        {
            return TerminalUtility.CreateValueTask(() => Update(state, message));
        }

        protected virtual Page Render(TState state)
        {
            return RenderAsync(state).Preserve().GetAwaiter().GetResult();
        }

        protected virtual ValueTask<Page> RenderAsync(TState state)
        {
            return TerminalUtility.CreateValueTask(() => Render(state));
        }

        protected void Dispatch(TMessage message)
        {
            _ = message ?? throw new ArgumentNullException(nameof(message));

            Dispatcher.Post(async () =>
            {
                await InitializeInternalAsync().ConfigureAwait(true);

                _state = await UpdateAsync(_state!, message).ConfigureAwait(true);

                await RenderInternalAsync(false).ConfigureAwait(true);
            });
        }

        public override sealed void Refresh(bool full)
        {
            Dispatcher.Post(async () => await RenderInternalAsync(full).ConfigureAwait(true));
        }

        async ValueTask InitializeInternalAsync()
        {
            if (!_initialized)
            {
                _state = await InitializeAsync().ConfigureAwait(true);
                _initialized = true;
            }
        }

        async ValueTask RenderInternalAsync(bool force)
        {
            await InitializeInternalAsync().ConfigureAwait(true);

            var layout = await RenderAsync(_state!).ConfigureAwait(true);

            if (layout == null)
                throw new InvalidOperationException("A valid page object must be returned.");

            if (_instance == null)
                _instance = (PageInstance)layout.Instantiate();

            _instance = (PageInstance)_instance.Reconcile(new(this, force), layout);

            // TODO: Drawing.
        }
    }

    public abstract class View<TApplication, TState> : View<TApplication, TState, object>
        where TApplication : Application
    {
        protected View(TApplication application)
            : base(application)
        {
        }
    }
}
