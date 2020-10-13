namespace System.Views.Visuals
{
    public abstract class WrapperInstance<T> : ElementInstance<T>
        where T : Wrapper
    {
        protected VisualInstance Child { get; private set; } = null!;

        private protected override void Update(Reconciler reconciler, Visual visual)
        {
            base.Update(reconciler, visual);

            var container = (T)visual;

            if (Child == null)
            {
                Child = container.Child.Instantiate();

                if (Child == null)
                    throw new InvalidOperationException("A valid visual instance must be returned.");

                reconciler.Attach(Child, this);
            }

            Child = Child.Reconcile(reconciler, container.Child);
            IsDirty = Child.IsDirty;

            Update((T)visual);
        }
    }
}
