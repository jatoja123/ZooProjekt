namespace Zoo.GameEvents;

public class GameEventsController : IObserver
{
    private List<GameEvent> events = new ();
    private Random rnd = new Random();

    public void Start()
    {
        // Dodaj stałe eventy
        AddEvent(new EventNewAnimalArrives());
        AddEvent(new EventAnimalsNotInHabitatDie());
    }

    public void AddEvent(GameEvent gameEvent)
    {
        events.Add(gameEvent);
    }
    
    public void ReceiveEvent(NotifyEvent notifyEvent)
    {
        if (notifyEvent is TurnEvent turnEvent)
        {
            foreach (var gameEvent in events.Where(x => (x.EventType == GameEventType.StartOfTurn && turnEvent.IsStartOfTurn) 
                                                                    || (x.EventType == GameEventType.EndOfTurn && !turnEvent.IsStartOfTurn)))
            {
                TryStartEvent(gameEvent);
            }
            
        }
    }

    private void TryStartEvent(GameEvent gameEvent)
    {
        var random = (float)rnd.Next(0, 100+1) / 100;
        if (random > gameEvent.EventChance()) return;
        gameEvent.Trigger();
    }
}