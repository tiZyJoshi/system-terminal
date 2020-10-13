using System.Views.Eventing;

namespace System.Views.Visuals
{
    public sealed class Button : Control
    {
        sealed class ButtonInstance : ControlInstance<Button>, INotifiable<KeyPressEvent>
        {
            string _text = null!;

            bool _default;

            Action? _pressed;

            protected override void Update(Button visual)
            {
                SetValue(ref _text, visual.Text);

                _default = visual.IsDefault;
                _pressed = visual.Pressed;
            }

            bool INotifiable<KeyPressEvent>.Notify(KeyPressEvent args)
            {
                // TODO

                return true;
            }
        }

        public string Text
        {
            get => _text;
            set => _text = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool IsDefault { get; set; }

        public Action? Pressed { get; set; }

        string _text = string.Empty;

        protected internal override VisualInstance Instantiate()
        {
            return new ButtonInstance();
        }
    }
}
