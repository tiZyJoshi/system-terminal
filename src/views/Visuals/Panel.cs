using System.Views.Collections;

namespace System.Views.Visuals
{
    public abstract class Panel : Element
    {
        public ViewDictionary<Element> Children { get; } = new();

        public Element this[string key]
        {
            get => Children[key];
            set => Children[key] = value;
        }
    }
}
