namespace Zoo.Environment;

public class TemperatureRequirement : EnvironmentalNeed
{
    public int RequiredTemperature { get; }

    public TemperatureRequirement(int requiredTemperature)
    {
        RequiredTemperature = requiredTemperature;
    }

    public override bool IsTemperatureSatisfied(LocationHabitat habitat)
    {
        if (habitat.Temperature == null || habitat.Temperature.Count == 0) return false;

        return RequiredTemperature >= habitat.Temperature[0] && RequiredTemperature <= habitat.Temperature[1];
    }
}
