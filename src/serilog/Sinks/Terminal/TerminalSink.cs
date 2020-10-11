using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Parsing;

namespace Serilog.Sinks.Terminal
{
    sealed class TerminalSink : ILogEventSink
    {
        static readonly object _lock = new();

        readonly Action<LogEvent, TerminalWriter>[] _renderers;

        readonly LogEventLevel? _standardErrorFromLevel;

        public TerminalSink(string outputTemplate, IFormatProvider? formatProvider,
            LogEventLevel? standardErrorFromLevel, TerminalTheme theme)
        {
            _standardErrorFromLevel = standardErrorFromLevel;

            var renderers = new List<Action<LogEvent, TerminalWriter>>();

            foreach (var token in new MessageTemplateParser().Parse(outputTemplate).Tokens)
            {
                Action<LogEvent, TerminalWriter> act;

                if (token is TextToken tt)
                {
                    void WriteTextToken(LogEvent ev, TerminalWriter writer)
                    {
                        using (_ = theme.Apply(writer, TerminalToken.Text))
                            writer.Write(tt.Text);
                    }

                    act = WriteTextToken;
                }
                else
                {
                    var pt = (PropertyToken)token;

                    void WriteExceptionToken(LogEvent ev, TerminalWriter writer)
                    {
                    }

                    void WriteLevelToken(LogEvent ev, TerminalWriter writer)
                    {
                    }

                    void WriteMessageToken(LogEvent ev, TerminalWriter writer)
                    {
                    }

                    void WriteNewLineToken(LogEvent ev, TerminalWriter writer)
                    {
                    }

                    void WritePropertiesToken(LogEvent ev, TerminalWriter writer)
                    {
                    }

                    void WriteTimestampToken(LogEvent ev, TerminalWriter writer)
                    {
                    }

                    void WriteEventToken(LogEvent ev, TerminalWriter writer)
                    {
                    }

                    act = pt.PropertyName switch
                    {
                        OutputProperties.ExceptionPropertyName => WriteExceptionToken,
                        OutputProperties.LevelPropertyName => WriteLevelToken,
                        OutputProperties.MessagePropertyName => WriteMessageToken,
                        OutputProperties.NewLinePropertyName => WriteNewLineToken,
                        OutputProperties.PropertiesPropertyName => WritePropertiesToken,
                        OutputProperties.TimestampPropertyName => WriteTimestampToken,
                        _ => WriteEventToken,
                    };
                }

                renderers.Add(act);
            }

            _renderers = renderers.ToArray();
        }

        public void Emit(LogEvent logEvent)
        {
            var writer = logEvent.Level < _standardErrorFromLevel ? System.Terminal.StdError : System.Terminal.StdOut;

            lock (_lock)
                foreach (var renderer in _renderers)
                    renderer(logEvent, writer);
        }
    }
}
