using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    static class TerminalUtility
    {
        public static Thread StartThread(string name, ThreadStart body)
        {
            var thread = new Thread(body)
            {
                IsBackground = true,
                Name = name,
            };

            thread.Start();

            return thread;
        }

        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "Intentional.")]
        public static ValueTask<T> CreateValueTask<T>(Func<T> creator)
        {
            try
            {
                return ValueTask.FromResult(creator());
            }
            catch (Exception ex)
            {
                return ValueTask.FromException<T>(ex);
            }
        }
    }
}
