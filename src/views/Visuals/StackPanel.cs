namespace System.Views.Visuals
{
    public sealed class StackPanel : Panel
    {
        sealed class StackPanelInstance : PanelInstance<StackPanel>
        {
            Orientation _orientation;

            protected override void Update(StackPanel visual)
            {
                SetValue(ref _orientation, visual.Orientation);
            }
        }

        public Orientation Orientation
        {
            get => _orientation;
            set
            {
                if (!Enum.IsDefined(value))
                    throw new ArgumentOutOfRangeException(nameof(value));

                _orientation = value;
            }
        }

        Orientation _orientation = Orientation.Vertical;

        protected internal override VisualInstance Instantiate()
        {
            return new StackPanelInstance();
        }
    }
}
