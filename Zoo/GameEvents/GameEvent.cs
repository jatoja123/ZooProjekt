namespace Zoo.GameEvents;

public abstract class GameEvent
{
    public virtual GameEventType EventType => GameEventType.StartOfTurn;
    public virtual EventPriority Priority => EventPriority.Normal;
    public abstract float EventChance();
    public abstract void Trigger();
}