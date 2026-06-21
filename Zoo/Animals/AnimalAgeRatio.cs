namespace Zoo.Animals;

public enum AgeRatio
{
    SLOW,
    NORMAL,
    FAST
}

public static class AnimalAgeRatio
{
    public static readonly Dictionary<AgeRatio, int> AgingFactor = new()
    {
        [AgeRatio.SLOW] = 1,
        [AgeRatio.NORMAL] = 2,
        [AgeRatio.FAST] = 4
    };
}
