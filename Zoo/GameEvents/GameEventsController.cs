using System;
using System.Collections.Generic;
using System.Linq;

namespace Zoo.GameEvents;

public class GameEventsController : IObserver
{
    private const int MaxRandomEventsInTurn = 3;
    
    private List<GameEvent> events = new();
    private Random rnd = new Random();

    public void Start()
    {
        events.AddRange(GameEventCatalog.CreateAll());
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
            
            var notRandomEvents = eventsToTrigger.Where(x => x.EventChance() >= 1).ToList();
            foreach (var gameEvent in notRandomEvents)
            {
                TryStartEvent(gameEvent);
            }
            
            // uruchom max ilość losowych eventów
            var randomEvents = eventsToTrigger.Where(x => x.EventChance() < 1).ToList();
            randomEvents = randomEvents.Shuffle().Take(MaxRandomEventsInTurn).ToList();
            foreach (var gameEvent in randomEvents)
            {
                TryStartEvent(gameEvent);
            }
        }
    }

    private void TryStartEvent(GameEvent gameEvent)
    {
        if (gameEvent.EventChance() >= 1)
        {
            gameEvent.Trigger();
            return;
        }
        
        var random = rnd.NextDouble();
        if (random > gameEvent.EventChance()) return;
        gameEvent.Trigger();
    }
}