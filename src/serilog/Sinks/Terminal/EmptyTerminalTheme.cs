using System.IO;

namespace Serilog.Sinks.Terminal
{
    public sealed class EmptyTerminalTheme : TerminalTheme
    {
        public override void Begin(TerminalWriter writer, TerminalToken token)
        {
        }

        public override void End(TerminalWriter writer, TerminalToken token)
        {
        }
    }
}
