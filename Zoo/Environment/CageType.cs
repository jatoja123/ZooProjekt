namespace Zoo.Environment;

public enum CageTypeEnum 
{
    Land,
    Water
}

public class CageType : EnvironmentalNeed
{
    public CageTypeEnum RequiredType { get; }

    public CageType(CageTypeEnum requiredType)
    {
        RequiredType = requiredType;
    }

    public override bool IsEnviromentSatisfied(LocationHabitat habitat)
    {
        return habitat.HabitatType == RequiredType;
    }
}
