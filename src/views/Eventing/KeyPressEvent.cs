namespace System.Views.Eventing
{
    public readonly struct KeyPressEvent
    {
        public Key Key { get; }

        internal KeyPressEvent(Key key)
        {
            Key = key;
        }
    }
}
