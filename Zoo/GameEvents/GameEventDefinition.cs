using System;

namespace Zoo.GameEvents;

public class GameEventDefinition
{
    public required string Name { get; init; }
    public required float Chance { get; init; }
    public required GameEventType Type { get; init; }
    public EventPriority Priority { get; init; } = EventPriority.Normal;
    public required Action Trigger { get; init; }
}