namespace Zoo.Environment;

public class TemperatureRequirement : EnvironmentalNeed
{
    public int MinTemp { get; }
    public int MaxTemp { get; }

    public TemperatureRequirement(int minTemp, int maxTemp)
    {
        MinTemp = minTemp;
        MaxTemp = maxTemp;
    }

    public override bool IsSatisfied(LocationHabitat habitat)
    {
        return habitat.Temperature >= MinTemp && habitat.Temperature <= MaxTemp;
    }
}
