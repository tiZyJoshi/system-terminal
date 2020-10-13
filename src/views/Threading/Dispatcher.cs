using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace System.Views.Threading
{
    public sealed class Dispatcher
    {
        sealed class DispatcherSynchronizationContext : SynchronizationContext
        {
            readonly Dispatcher _dispatcher;

            public DispatcherSynchronizationContext(Dispatcher dispatcher)
            {
                _dispatcher = dispatcher;
            }

            public override DispatcherSynchronizationContext CreateCopy()
            {
                return new(_dispatcher);
            }

            public override void Post(SendOrPostCallback d, object? state)
            {
                // This method should return immediately, and only raise the UnhandledException
                // event on the dispatcher if an exception occurs while running the callback.
                _dispatcher.Post(() => d(state));
            }

            public override void Send(SendOrPostCallback d, object? state)
            {
                // This method should wait for the callback to finish and, if an exception occurs,
                // propagate it back to the caller.
                _dispatcher.Send(() => d(state));
            }
        }

        public event EventHandler<UnhandledExceptionEventArgs>? UnhandledException;

        public static Dispatcher? Current => _current;

        public View View { get; }

        public Task Completion => _queue.Reader.Completion;

        [ThreadStatic]
        static Dispatcher? _current;

        readonly Channel<Action> _queue = Channel.CreateUnbounded<Action>(new UnboundedChannelOptions
        {
            SingleReader = true,
        });

        internal Dispatcher(View view)
        {
            View = view;
        }

        internal void Start()
        {
            _ = TerminalUtility.StartThread("Terminal View Dispatcher", DispatchLoop);
        }

        internal void Stop()
        {
            _ = _queue.Writer.TryComplete();
        }

        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "Intentional.")]
        void DispatchLoop()
        {
            // This method implements the core of our synchronization context. Use of async/await is
            // banned here since this is where continuations ultimately arrive to be executed, and
            // we also do not want to accidentally end up on the thread pool.

            _current = this;

            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(this));

            while (true)
            {
                Action action;

                try
                {
                    action = _queue.Reader.ReadAsync().Preserve().GetAwaiter().GetResult();
                }
                catch (ChannelClosedException)
                {
                    // Stop was called; leave the dispatch loop.
                    break;
                }

                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    var args = new UnhandledExceptionEventArgs(this, ex);

                    UnhandledException?.Invoke(this, args);

                    if (args.Catch)
                        continue;

                    View.Application.OnUnhandledException(args);

                    if (!args.Catch)
                        throw;
                }
            }
        }

        public void Post(Action action)
        {
            try
            {
                // This completes immediately since it is an unbounded channel.
                _queue.Writer.WriteAsync(action).Preserve().GetAwaiter().GetResult();
            }
            catch (ChannelClosedException)
            {
            }
        }

        public void Send(Action action)
        {
            SendAsync(action).GetAwaiter().GetResult();
        }

        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "Intentional.")]
        public Task SendAsync(Action action)
        {
            var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            try
            {
                Post(() =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }

                    tcs.SetResult();
                });
            }
            catch (InvalidOperationException ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }
    }
}
