using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Views
{
    public abstract class Application
    {
        protected sealed class Options
        {
            public TerminalScreen Screen
            {
                get => _screen;
                set => _screen = value ?? throw new ArgumentNullException(nameof(value));
            }

            public ShutdownMode ShutdownMode
            {
                get => _shutdownMode;
                set
                {
                    if (!Enum.IsDefined(value))
                        throw new ArgumentOutOfRangeException();

                    _shutdownMode = value;
                }
            }

            public TimeSpan TickInterval
            {
                get => _tickInterval;
                set
                {
                    var ms = (long)value.TotalMilliseconds;

                    if (ms < -1 || ms > int.MaxValue)
                        throw new ArgumentOutOfRangeException();

                    _tickInterval = value;
                }
            }

            TerminalScreen _screen = Terminal.AlternateScreen;

            ShutdownMode _shutdownMode;

            TimeSpan _tickInterval = TimeSpan.FromMilliseconds(50);
        }

        protected enum ShutdownMode
        {
            LastViewClosed,
            RootViewClosed,
        }

        public event EventHandler<Threading.UnhandledExceptionEventArgs>? UnhandledException;

        public static Application? Current { get; private set; }

        public ViewManager Views { get; }

        public bool IsRunning => _running;

        internal bool IsStopping => _stopping;

        static readonly object _lock = new();
        volatile bool _running;

        volatile bool _stopping;

        protected Application()
        {
            Views = new(this);
        }

        internal void OnUnhandledException(Threading.UnhandledExceptionEventArgs args)
        {
            UnhandledException?.Invoke(this, args);
        }

        protected virtual Options GetOptions()
        {
            return new();
        }

        protected abstract View CreateRootView();

        public static async Task RunAsync(Application application)
        {
            _ = application ?? throw new ArgumentNullException(nameof(application));

            async Task RunAsync()
            {
                var options = application.GetOptions();

                if (options == null)
                    throw new InvalidOperationException("A valid options object must be returned.");

                var root = application.CreateRootView();

                if (root == null)
                    throw new InvalidOperationException("A valid view object must be returned.");

                Current = application;

                application._running = true;

                application.Views.Start(root);

                try
                {
                    using var activator = options.Screen.Activate();

                    root.Open();

                    while (true)
                    {
                        var (exit, force) = options.ShutdownMode switch
                        {
                            ShutdownMode.LastViewClosed => (application.Views.All.Count == 0, false),
                            ShutdownMode.RootViewClosed => (root.State == ViewState.Closed, true),
                            _ => throw new Exception(),
                        };

                        if (exit)
                        {
                            application._stopping = true;

                            if (force)
                            {
                                IReadOnlyList<View> views;

                                // By this point, IsStopping is true, so View.Open will refuse to
                                // let any further views be added. This means that the view list
                                // should soon become empty. This might not be the case if a view or
                                // dispatcher is misbehaving, but that would be a bug in user code.
                                while ((views = application.Views.All).Count != 0)
                                {
                                    foreach (var view in views)
                                        view.Close();

                                    await Task.WhenAll(views.Select(v => v.Dispatcher.Completion))
                                        .ConfigureAwait(false);
                                }
                            }

                            break;
                        }

                        // TODO: Input processing.

                        await Task.Delay(options.TickInterval).ConfigureAwait(false);
                    }
                }
                finally
                {
                    application.Views.Stop();

                    application._running = false;

                    Current = null;
                }
            }

            await Task.Run(() =>
            {
                lock (_lock)
                    RunAsync().GetAwaiter().GetResult();
            }).ConfigureAwait(false);
        }
    }
}
