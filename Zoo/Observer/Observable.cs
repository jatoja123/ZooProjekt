namespace Zoo;

public abstract class Observable
{
    private List<IObserver> observers = new ();

    public void Subscribe(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Unsubscribe(IObserver observer)
    {
        observers.Remove(observer);
    }

    protected void Notify(NotifyEvent notifyEvent)
    {
        foreach (var observer in observers)
        {
            observer.ReceiveEvent(notifyEvent);
        }
    }
    
}