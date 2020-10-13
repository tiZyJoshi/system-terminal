namespace System.Views.Eventing
{
    public readonly struct ScreenResizeEvent
    {
        public Size Size { get; }

        internal ScreenResizeEvent(Size size)
        {
            Size = size;
        }
    }
}
