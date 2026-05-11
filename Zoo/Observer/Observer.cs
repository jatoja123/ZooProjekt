namespace Zoo;

public interface IObserver
{
    public void ReceiveEvent(NotifyEvent notifyEvent);
}