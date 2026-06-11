using System;
using System.Collections.Generic;
using System.Linq;

namespace Zoo.GameEvents;

public class GameEventsController : IObserver
{
    private List<GameEvent> events = new();
    private Random rnd = new Random();

    public void Start()
    {
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
            var eventsToTrigger = events
                .Where(x => (x.EventType == GameEventType.StartOfTurn && turnEvent.IsStartOfTurn) || 
                            (x.EventType == GameEventType.EndOfTurn && !turnEvent.IsStartOfTurn))
                .OrderBy(x => x.Priority)
                .ToList();

            foreach (var gameEvent in eventsToTrigger)
            {
                TryStartEvent(gameEvent);
            }
        }
    }

    private void TryStartEvent(GameEvent gameEvent)
    {
        var random = rnd.NextDouble();
        if (random > gameEvent.EventChance()) return;
        gameEvent.Trigger();
    }
}