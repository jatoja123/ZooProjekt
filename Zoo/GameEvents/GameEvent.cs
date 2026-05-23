namespace Zoo.GameEvents;

public abstract class GameEvent
{
    public virtual GameEventType EventType => GameEventType.StartOfTurn;
    public abstract float EventChance();
    public abstract void Trigger();
}