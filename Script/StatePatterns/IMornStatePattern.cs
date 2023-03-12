namespace MornLib.StatePatterns
{
    public interface IMornStatePattern<T>
    {
        void OnEnter();
        IMornStatePattern<T> Execute(T t);
        void OnExit();
    }
}
