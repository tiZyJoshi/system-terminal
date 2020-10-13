using System.Collections.Generic;
using System.Views.Collections;

namespace System.Views.Visuals
{
    public abstract class PanelInstance<T> : ElementInstance<T>
        where T : Panel
    {
        protected ViewDictionary<VisualInstance> Children { get; } = new();

        private protected override void Update(Reconciler reconciler, Visual visual)
        {
            base.Update(reconciler, visual);

            var oldChildren = new Dictionary<string, VisualInstance>(Children);

            Children.Clear();

            foreach (var (key, child) in ((T)visual).Children)
            {
                if (!oldChildren.Remove(key, out var instance))
                {
                    instance = child.Instantiate();

                    if (instance == null)
                        throw new InvalidOperationException("A valid visual instance must be returned.");

                    reconciler.Attach(instance, this);
                }

                instance = instance.Reconcile(reconciler, child);

                IsDirty |= instance.IsDirty;

                Children.Add(key, instance);
            }

            foreach (var (_, child) in oldChildren)
                reconciler.Detach(child);

            Update((T)visual);
        }
    }
}
