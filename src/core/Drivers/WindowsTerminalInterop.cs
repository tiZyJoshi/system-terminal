using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Drivers
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307", Justification = "P/Invoke.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310", Justification = "P/Invoke.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313", Justification = "P/Invoke.")]
    sealed unsafe class WindowsTerminalInterop
    {
        public struct COORD
        {
            public short X;

            public short Y;
        }

        public struct SMALL_RECT
        {
            public short Left;

            public short Top;

            public short Right;

            public short Bottom;
        }

        public struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;

            public COORD dwCursorPosition;

            public ushort wAttributes;

            public SMALL_RECT srWindow;

            public COORD dwMaximumWindowSize;
        }

        public delegate bool HandlerRoutine(uint dwCtrlType);

        const string Kernel32 = "kernel32";

        public const uint STD_INPUT_HANDLE = unchecked((uint)-10);

        public const uint STD_OUTPUT_HANDLE = unchecked((uint)-11);

        public const uint STD_ERROR_HANDLE = unchecked((uint)-12);

        public const uint ENABLE_PROCESSED_INPUT = 0x0001;

        public const uint ENABLE_LINE_INPUT = 0x0002;

        public const uint ENABLE_ECHO_INPUT = 0x0004;

        public const uint ENABLE_INSERT_MODE = 0x0020;

        public const uint ENABLE_EXTENDED_FLAGS = 0x0080;

        public const uint ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200;

        public const uint ENABLE_PROCESSED_OUTPUT = 0x0001;

        public const uint ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002;

        public const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        public const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        public const uint CTRL_C_EVENT = 0;

        public const uint CTRL_BREAK_EVENT = 1;

        public const uint FILE_TYPE_CHAR = 0x0002;

        public const int ERROR_HANDLE_EOF = 38;

        public const int ERROR_BROKEN_PIPE = 109;

        public const int ERROR_NO_DATA = 232;

        public static readonly IntPtr INVALID_HANDLE_VALUE = (IntPtr)(-1);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetStdHandle(uint nStdHandle);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint lpMode);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput,
            out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern bool SetConsoleCP(uint wCodePageID);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport(Kernel32, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool ReadConsole(IntPtr hConsoleInput, void* lpBuffer, uint nNumberOfCharsToRead,
            out uint lpNumberOfCharsRead, void* lpOverlapped);

        [DllImport(Kernel32, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool WriteConsole(IntPtr hConsoleOutput, void* lpBuffer, uint nNumberOfCharsToWrite,
            out uint lpNumberOfCharsWritten, void* lpOverlapped);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern bool FlushConsoleInputBuffer(IntPtr hConsoleInput);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine HandlerRoutine, bool Add);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern uint GetFileType(IntPtr hFile);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern bool ReadFile(IntPtr hFile, void* lpBuffer, uint nNumberOfBytesToRead,
            out uint lpNumberOfBytesRead, void* lpOverlapped);

        [DllImport(Kernel32, ExactSpelling = true, SetLastError = true)]
        public static extern bool WriteFile(IntPtr hFile, void* lpBuffer, uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten, void* lpOverlapped);
    }
}
