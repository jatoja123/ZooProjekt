using System;

namespace Zoo.GameEvents;

public class GameEventDefinition
{
    public required string Name { get; init; }
    public required float Chance { get; init; }
    public required GameEventType Type { get; init; }
    public EventPriority Priority { get; init; } = EventPriority.Normal;
    public Action Trigger { get; init; } = () => { };
    public Func<PendingDecision>? DecisionFactory { get; init; }

    public bool RequiresDecision => DecisionFactory != null;
}