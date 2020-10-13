using System.Dynamic;

namespace System.Views.Visuals
{
    public abstract class Visual
    {
        public dynamic Attached { get; } = new ExpandoObject();

        private protected Visual()
        {
        }

        protected internal abstract VisualInstance Instantiate();
    }
}
