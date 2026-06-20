namespace Zoo.GameEvents;


public class ParametrizedGameEvent : GameEvent
{
    private readonly GameEventDefinition definition;

    public ParametrizedGameEvent(GameEventDefinition definition)
    {
        this.definition = definition;
    }

    public override float EventChance() => definition.Chance;
    public override GameEventType EventType => definition.Type;
    public override EventPriority Priority => definition.Priority;

    public bool RequiresDecision => definition.RequiresDecision;

    public override void Trigger() => definition.Trigger();

    public PendingDecision CreateDecision() => definition.DecisionFactory!();
}