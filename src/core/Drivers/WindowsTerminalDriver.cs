using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Drivers
{
    [SuppressMessage("Microsoft.Usage", "CA1001", Justification = "Intentional.")]
    sealed class WindowsTerminalDriver : TerminalDriver
    {
        sealed class WindowsTerminalReader : TerminalReader
        {
            public IntPtr Handle { get; }

            public bool IsValid { get; }

            public override bool IsRedirected => IsRedirected(Handle);

            readonly object _lock = new();

            readonly string _name;

            public WindowsTerminalReader(TerminalDriver driver, IntPtr handle, string name)
                : base(driver)
            {
                Handle = handle;
                IsValid = IsHandleValid(handle, false);
                _name = name;
            }

            public uint? GetMode()
            {
                return WindowsTerminalInterop.GetConsoleMode(Handle, out var m) ? m : null;
            }

            public bool SetMode(uint mode)
            {
                return WindowsTerminalInterop.SetConsoleMode(Handle, mode);
            }

            public bool AddMode(uint mode)
            {
                return GetMode() is uint m && WindowsTerminalInterop.SetConsoleMode(Handle, m | mode);
            }

            public bool RemoveMode(uint mode)
            {
                return GetMode() is uint m && WindowsTerminalInterop.SetConsoleMode(Handle, m & ~mode);
            }

            public override unsafe int Read(Span<byte> data)
            {
                if (data.IsEmpty || !IsValid)
                    return 0;

                uint ret;

                lock (_lock)
                {
                    fixed (byte* p = data)
                    {
                        var result = IsRedirected ?
                            WindowsTerminalInterop.ReadFile(Handle, p, (uint)data.Length, out ret, null) :
                            WindowsTerminalInterop.ReadConsole(Handle, p, (uint)data.Length, out ret, null);

                        if (result)
                            return (int)ret;
                    }
                }

                var err = Marshal.GetLastWin32Error();

                // See comments in UnixTerminalReader for the error handling rationale.
                switch (err)
                {
                    case WindowsTerminalInterop.ERROR_HANDLE_EOF:
                    case WindowsTerminalInterop.ERROR_BROKEN_PIPE:
                    case WindowsTerminalInterop.ERROR_NO_DATA:
                        break;
                    default:
                        throw new TerminalException(
                            $"Could not read from standard {_name}: {new Win32Exception(err).Message}");
                }

                return (int)ret;
            }
        }

        sealed class WindowsTerminalWriter : TerminalWriter
        {
            public IntPtr Handle { get; }

            public bool IsValid { get; }

            public override bool IsRedirected => IsRedirected(Handle);

            readonly object _lock = new();

            readonly string _name;

            public WindowsTerminalWriter(TerminalDriver driver, IntPtr handle, string name)
                : base(driver)
            {
                Handle = handle;
                IsValid = IsHandleValid(handle, true);
                _name = name;
            }

            public uint? GetMode()
            {
                return WindowsTerminalInterop.GetConsoleMode(Handle, out var m) ? m : null;
            }

            public bool SetMode(uint mode)
            {
                return WindowsTerminalInterop.SetConsoleMode(Handle, mode);
            }

            public bool AddMode(uint mode)
            {
                return GetMode() is uint m && WindowsTerminalInterop.SetConsoleMode(Handle, m | mode);
            }

            public bool RemoveMode(uint mode)
            {
                return GetMode() is uint m && WindowsTerminalInterop.SetConsoleMode(Handle, m & ~mode);
            }

            public override unsafe void Write(ReadOnlySpan<byte> data)
            {
                if (data.IsEmpty || !IsValid)
                    return;

                lock (_lock)
                {
                    fixed (byte* p = data)
                    {
                        var result = IsRedirected ?
                            WindowsTerminalInterop.WriteFile(Handle, p, (uint)data.Length, out _, null) :
                            WindowsTerminalInterop.WriteConsole(Handle, p, (uint)data.Length, out _, null);

                        if (result)
                            return;
                    }
                }

                var err = Marshal.GetLastWin32Error();

                // See comments in UnixTerminalWriter for the error handling rationale.
                switch (err)
                {
                    case WindowsTerminalInterop.ERROR_HANDLE_EOF:
                    case WindowsTerminalInterop.ERROR_BROKEN_PIPE:
                    case WindowsTerminalInterop.ERROR_NO_DATA:
                        break;
                    default:
                        throw new TerminalException(
                            $"Could not write to standard {_name}: {new Win32Exception(err).Message}");
                }
            }
        }

        public static WindowsTerminalDriver Instance { get; } = new();

        static IntPtr InHandle => WindowsTerminalInterop.GetStdHandle(WindowsTerminalInterop.STD_INPUT_HANDLE);

        static IntPtr OutHandle => WindowsTerminalInterop.GetStdHandle(WindowsTerminalInterop.STD_OUTPUT_HANDLE);

        static IntPtr ErrorHandle => WindowsTerminalInterop.GetStdHandle(WindowsTerminalInterop.STD_ERROR_HANDLE);

        public override TerminalReader StdIn => _in;

        public override TerminalWriter StdOut => _out;

        public override TerminalWriter StdError => _error;

        public override TerminalSize Size
        {
            get
            {
                if (GetSize() is TerminalSize s)
                    _size = s;

                return _size ?? throw new TerminalException("There is no terminal attached.");
            }
        }

        readonly ManualResetEventSlim _event = new();

        readonly WindowsTerminalReader _in;

        readonly WindowsTerminalWriter _out;

        readonly WindowsTerminalWriter _error;

        readonly WindowsTerminalInterop.HandlerRoutine _handler;

        TerminalSize? _size;

        WindowsTerminalDriver()
        {
            _in = new(this, InHandle, "input");
            _out = new(this, OutHandle, "output");
            _error = new(this, ErrorHandle, "error");

            _ = WindowsTerminalInterop.SetConsoleCP((uint)Encoding.CodePage);
            _ = WindowsTerminalInterop.SetConsoleOutputCP((uint)Encoding.CodePage);

            var inMode =
                WindowsTerminalInterop.ENABLE_PROCESSED_INPUT |
                WindowsTerminalInterop.ENABLE_LINE_INPUT |
                WindowsTerminalInterop.ENABLE_ECHO_INPUT |
                WindowsTerminalInterop.ENABLE_INSERT_MODE |
                WindowsTerminalInterop.ENABLE_EXTENDED_FLAGS |
                WindowsTerminalInterop.ENABLE_VIRTUAL_TERMINAL_INPUT;
            var outMode =
                WindowsTerminalInterop.ENABLE_PROCESSED_OUTPUT |
                WindowsTerminalInterop.ENABLE_WRAP_AT_EOL_OUTPUT |
                WindowsTerminalInterop.ENABLE_VIRTUAL_TERMINAL_PROCESSING;

            // Set modes on all handles in case one of them has been redirected. These calls can
            // fail if there is no console attached, but that is OK.
            _ = _in.AddMode(inMode);
            _ = _out.AddMode(outMode) || _error.AddMode(outMode);

            // Keep the delegate alive by storing it in a field.
            _handler = e => HandleBreakSignal(e == WindowsTerminalInterop.CTRL_C_EVENT);

            _ = WindowsTerminalInterop.SetConsoleCtrlHandler(_handler, true);

            // Windows currently has no SIGWINCH equivalent, so we have to poll for size changes.
            _ = TerminalUtility.StartThread("Terminal Resize Listener", () =>
            {
                while (true)
                {
                    _event.Wait();

                    // HandleResize will check whether the size is actually different from the last
                    // time the event was fired.
                    if (GetSize() is TerminalSize s)
                    {
                        _size = s;

                        HandleResize(s);
                    }

                    // TODO: Do we need to make this configurable?
                    Thread.Sleep(100);
                }
            });
        }

        protected override void ToggleResizeEvent(bool enable)
        {
            if (enable)
                _event.Set();
            else
                _event.Reset();
        }

        public override void GenerateBreakSignal(TerminalBreakSignal signal)
        {
            _ = WindowsTerminalInterop.GenerateConsoleCtrlEvent(signal switch
            {
                TerminalBreakSignal.Interrupt => WindowsTerminalInterop.CTRL_C_EVENT,
                TerminalBreakSignal.Quit => WindowsTerminalInterop.CTRL_BREAK_EVENT,
                _ => throw new ArgumentOutOfRangeException(nameof(signal)),
            }, 0);
        }

        public override void GenerateSuspendSignal()
        {
            // Windows does not have an equivalent of SIGTSTP.
        }

        static unsafe bool IsHandleValid(IntPtr handle, bool write)
        {
            if (handle == null || handle == WindowsTerminalInterop.INVALID_HANDLE_VALUE)
                return false;

            if (write)
            {
                byte dummy = 42;

                return WindowsTerminalInterop.WriteFile(handle, &dummy, 0, out _, null);
            }

            return true;
        }

        static bool IsRedirected(IntPtr handle)
        {
            return (WindowsTerminalInterop.GetFileType(handle) & WindowsTerminalInterop.FILE_TYPE_CHAR) == 0 ||
                !WindowsTerminalInterop.GetConsoleMode(handle, out _);
        }

        TerminalSize? GetSize()
        {
            static WindowsTerminalInterop.CONSOLE_SCREEN_BUFFER_INFO? Get(IntPtr handle)
            {
                return WindowsTerminalInterop.GetConsoleScreenBufferInfo(handle, out var info) ? info : null;
            }

            // Try both handles in case only one of them has been redirected.
            return (Get(_out.Handle) ?? Get(_error.Handle)) is WindowsTerminalInterop.CONSOLE_SCREEN_BUFFER_INFO info ?
                new(info.srWindow.Right - info.srWindow.Left + 1, info.srWindow.Bottom - info.srWindow.Top + 1) : null;
        }

        protected override void SetRawModeCore(bool raw, bool discard)
        {
            if (!_in.IsValid || (!_out.IsValid && !_error.IsValid))
                throw new TerminalException("There is no terminal attached.");

            var inMode =
                WindowsTerminalInterop.ENABLE_PROCESSED_INPUT |
                WindowsTerminalInterop.ENABLE_LINE_INPUT |
                WindowsTerminalInterop.ENABLE_ECHO_INPUT;
            var outMode =
                WindowsTerminalInterop.DISABLE_NEWLINE_AUTO_RETURN;

            if (!(raw ? _in.RemoveMode(inMode) && (_out.RemoveMode(outMode) || _error.RemoveMode(outMode)) :
                _in.AddMode(inMode) && (_out.AddMode(outMode) || _error.AddMode(outMode))))
                throw new TerminalException(
                    $"Could not change raw mode setting: {new Win32Exception(Marshal.GetLastWin32Error()).Message}");

            if (!WindowsTerminalInterop.FlushConsoleInputBuffer(InHandle))
                throw new TerminalException(
                    $"Could not flush input buffer: {new Win32Exception(Marshal.GetLastWin32Error()).Message}");
        }
    }
}
