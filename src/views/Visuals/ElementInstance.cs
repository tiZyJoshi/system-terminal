namespace System.Views.Visuals
{
    public abstract class ElementInstance<T> : VisualInstance<T>
        where T : Element
    {
        private protected ElementInstance()
        {
        }

        private protected override void Update(Reconciler reconciler, Visual visual)
        {
            base.Update(reconciler, visual);

            Update((T)visual);
        }

        protected abstract void Update(T visual);
    }
}
