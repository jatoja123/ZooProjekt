namespace Zoo.Environment;

public enum CageTypeEnum 
{
    Land,
    Water,
    Amphibious
}

public class CageType : EnvironmentalNeed
{
    public CageTypeEnum RequiredType { get; }

    public CageType(CageTypeEnum requiredType)
    {
        RequiredType = requiredType;
    }

    public override bool IsSatisfied(LocationHabitat habitat)
    {
        return habitat.HabitatType == RequiredType || habitat.HabitatType == CageTypeEnum.Amphibious;
    }
}
