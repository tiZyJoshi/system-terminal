using System.IO;

namespace Serilog.Sinks.Terminal
{
    public sealed class DefaultTerminalTheme : TerminalTheme
    {
        public override void Begin(TerminalWriter writer, TerminalToken token)
        {
        }

        public override void End(TerminalWriter writer, TerminalToken token)
        {
            System.Terminal.ResetAttributes();
        }
    }
}
