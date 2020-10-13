namespace System.Views.Visuals
{
    sealed class PageInstance : VisualInstance<Page>
    {
        VisualInstance _root = null!;

        private protected override void Update(Reconciler reconciler, Visual visual)
        {
            var page = (Page)visual;

            if (_root == null)
            {
                _root = page.Root.Instantiate();

                if (_root == null)
                    throw new InvalidOperationException("A valid panel instance must be returned.");

                reconciler.Attach(_root, this);
            }

            _root = _root.Reconcile(reconciler, page.Root);
            IsDirty = _root.IsDirty;
        }
    }
}
