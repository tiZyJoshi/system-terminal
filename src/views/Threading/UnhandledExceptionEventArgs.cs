namespace System.Views.Threading
{
    public sealed class UnhandledExceptionEventArgs
    {
        public Dispatcher Dispatcher { get; }

        public Exception Exception { get; }

        public bool Catch { get; set; }

        internal UnhandledExceptionEventArgs(Dispatcher dispatcher, Exception exception)
        {
            Dispatcher = dispatcher;
            Exception = exception;
        }
    }
}
