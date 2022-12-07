namespace MornLib.StatePattern
{
    public interface IMornStatePattern<T>
    {
        IMornStatePattern<T> Execute(T t);
    }
}
