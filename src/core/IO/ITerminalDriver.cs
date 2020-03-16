namespace System.IO
{
    interface ITerminalDriver
    {
        ITerminalReader StdIn { get; }

        ITerminalWriter StdOut { get; }

        ITerminalWriter StdError { get; }

        int Width { get; }

        int Height { get; }

        void SetRawMode(bool raw, bool discard);
    }
}
