using System.Diagnostics.CodeAnalysis;

namespace System.Views.Visuals
{
    public abstract class Wrapper : Element
    {
        public Element Child
        {
            get => _child;
            [MemberNotNull(nameof(_child))]
            set => _child = value ?? throw new ArgumentNullException(nameof(value));
        }

        Element _child;

        protected Wrapper(Element child)
        {
            Child = child;
        }
    }
}
