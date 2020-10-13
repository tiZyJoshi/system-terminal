namespace System.Views.Eventing
{
    public interface INotifiable<T>
    {
        bool Notify(T args);
    }
}
