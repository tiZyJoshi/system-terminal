using System.Views.Visuals;

namespace System.Views
{
    sealed class Reconciler
    {
        readonly View _view;

        readonly bool _force;

        public Reconciler(View view, bool force)
        {
            _view = view;
            _force = force;
        }

        public void Attach(VisualInstance visual, VisualInstance? parent)
        {
            visual.View = _view;
            visual.Parent = parent;
        }

        public void Update(VisualInstance visual)
        {
            if (_force)
                visual.IsDirty = true;
        }

        public void Detach(VisualInstance visual)
        {
            visual.Parent = null;
            visual.View = null;
        }
    }
}
