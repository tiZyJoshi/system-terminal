using System.IO;

namespace Serilog.Sinks.Terminal
{
    public abstract class TerminalTheme
    {
        internal readonly ref struct Decorator
        {
            readonly TerminalTheme _theme;

            readonly TerminalWriter _writer;

            readonly TerminalToken _token;

            public Decorator(TerminalTheme theme, TerminalWriter writer, TerminalToken token)
            {
                _theme = theme;
                _writer = writer;
                _token = token;

                theme.Begin(writer, token);
            }

            public void Dispose()
            {
                _theme.End(_writer, _token);
            }
        }

        internal Decorator Apply(TerminalWriter writer, TerminalToken token)
        {
            return new(this, writer, token);
        }

        public abstract void Begin(TerminalWriter writer, TerminalToken token);

        public abstract void End(TerminalWriter writer, TerminalToken token);
    }
}
