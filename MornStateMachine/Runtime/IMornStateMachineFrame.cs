namespace MornStateMachine
{
    public interface IMornStateMachineFrame
    {
        float PrevFrame { get; }
        float Frame { get; }
        bool IsFrame(int frame);
    }
}