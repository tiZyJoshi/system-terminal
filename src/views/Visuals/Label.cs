namespace System.Views.Visuals
{
    public sealed class Label : Control
    {
        sealed class LabelInstance : ControlInstance<Label>
        {
            string _text = null!;

            protected override void Update(Label visual)
            {
                SetValue(ref _text, visual.Text);
            }
        }

        string _text = string.Empty;

        public string Text
        {
            get => _text;
            set => _text = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected internal override VisualInstance Instantiate()
        {
            return new LabelInstance();
        }
    }
}
