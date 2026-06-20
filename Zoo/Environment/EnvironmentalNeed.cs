namespace Zoo.Environment;

public abstract class EnvironmentalNeed
{
    public virtual bool IsTemperatureSatisfied(LocationHabitat habitat) => true;
    public virtual bool IsEnviromentSatisfied(LocationHabitat habitat) => true;
}
