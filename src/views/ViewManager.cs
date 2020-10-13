using System.Collections.Generic;
using System.Linq;

namespace System.Views
{
    public sealed class ViewManager
    {
        public Application Application { get; }

        public IReadOnlyList<View> All
        {
            get
            {
                lock (_lock)
                    return _views.ToArray();
            }
        }

        public View? Root { get; private set; }

        public View? Top
        {
            get
            {
                lock (_lock)
                    return _views.First?.Value;
            }
        }

        public View? Bottom
        {
            get
            {
                lock (_lock)
                    return _views.Last?.Value;
            }
        }

        readonly object _lock = new();

        readonly LinkedList<View> _views = new();

        internal ViewManager(Application application)
        {
            Application = application;
        }

        internal void Start(View root)
        {
            Root = root;

            Terminal.Resize += OnResize;
        }

        internal void Stop()
        {
            Terminal.Resize -= OnResize;

            Root = null;
        }

        void OnResize(object? sender, TerminalResizeEventArgs e)
        {
            // TODO: We could be much more efficient and only refresh views that actually require
            // it based on their position and other factors, but since that would require some
            // complicated synchronization and resize events should be fairly rare, this is good
            // enough for now.

            lock (_views)
                foreach (var view in _views)
                    view.Refresh(true);
        }

        internal void Open(View view)
        {
            lock (_views)
                view.Node = _views.AddFirst(view);
        }

        internal void Close(View view)
        {
            lock (_views)
                _views.Remove(view.Node!);
        }
    }
}
