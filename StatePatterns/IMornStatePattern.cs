namespace MornLib.StatePatterns
{
    public interface IMornStatePattern<T>
    {
        IMornStatePattern<T> Execute(T t);
    }
}
