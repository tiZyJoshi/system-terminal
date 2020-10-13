using System.Collections.Generic;
using System.Dynamic;

namespace System.Views.Visuals
{
    public abstract class VisualInstance
    {
        protected internal View? View { get; internal set; }

        protected internal VisualInstance? Parent { get; internal set; }

        internal bool IsDirty { get; set; } = true;

        private protected abstract Type Type { get; }

        readonly Dictionary<string, object?> _attached = new();

        private protected VisualInstance()
        {
        }

        internal VisualInstance Reconcile(Reconciler reconciler, Visual visual)
        {
            VisualInstance instance;

            if (Type != visual.GetType())
            {
                instance = visual.Instantiate();

                if (instance == null)
                    throw new InvalidOperationException("A valid visual instance must be returned.");

                reconciler.Detach(this);
                reconciler.Attach(instance, Parent);
            }
            else
            {
                reconciler.Update(this);

                instance = this;
            }

            instance.Update(reconciler, visual);

            return instance;
        }

        private protected virtual void Update(Reconciler reconciler, Visual visual)
        {
            // TODO: We currently assume that any attached property change should make the visual
            // dirty. While this holds true in the vast majority of cases, it is not necessarily the
            // case. It might be a good idea to have a mechanism through which a parent visual can
            // inspect updated attached properties on children and signal whether they should affect
            // the dirtiness flag.

            var oldAttached = new Dictionary<string, object?>(_attached);

            _attached.Clear();

            foreach (var (name, value) in (ExpandoObject)visual.Attached)
            {
                if (oldAttached.Remove(name, out var old))
                    IsDirty |= !Equals(old, value);
                else
                    IsDirty = true;

                _attached.Add(name, value);
            }

            if (oldAttached.Count != 0)
                IsDirty = true;
        }

        protected bool TryGetAttached<T>(string name, out T? result)
        {
            result = default;

            if (!_attached.TryGetValue(name, out var value))
                return false;

            try
            {
                result = (T)value;
            }
            catch (InvalidCastException)
            {
                return false;
            }

            return true;
        }

        protected void SetValue<T>(ref T location, T value)
        {
            IsDirty |= !EqualityComparer<T>.Default.Equals(location, location = value);
        }
    }

    public abstract class VisualInstance<T> : VisualInstance
        where T : Visual
    {
        private protected override Type Type => typeof(T);

        private protected VisualInstance()
        {
        }
    }
}
