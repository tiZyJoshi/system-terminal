using System.Diagnostics.CodeAnalysis;

namespace System.Views.Visuals
{
    public sealed class Page : Visual
    {
        public Panel Root
        {
            get => _root;
            [MemberNotNull(nameof(_root))]
            set => _root = value ?? throw new ArgumentNullException(nameof(value));
        }

        Panel _root;

        public Page(Panel root)
        {
            Root = root;
        }

        protected internal override VisualInstance Instantiate()
        {
            return new PageInstance();
        }
    }
}
