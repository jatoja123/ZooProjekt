namespace Zoo.Environment;

public abstract class EnvironmentalNeed
{
    public virtual bool ISTemperatureSatisfied(LocationHabitat habitat) => true;
    public virtual bool IsenviromentSatisfied(LocationHabitat habitat) => true;
}
