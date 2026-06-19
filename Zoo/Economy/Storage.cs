using Zoo.Score;

namespace Zoo.Economy;

public enum GoodType
{
    FoodMeat,
    FoodPlant,
    FoodMixed,
    WATER,
    MEDICINE
}

public class Storage: IScoreable
{
    private const int StorageScoreMultiplier = 10;
    private const int DeafultLimit = 10;

    private Dictionary<GoodType, int> amounts =
        Enum.GetValues<GoodType>().ToDictionary(t => t, _ => 0);
    private Dictionary<GoodType, int> limits =
        Enum.GetValues<GoodType>().ToDictionary(t => t, _ => DeafultLimit);

    public int GetAmount(GoodType type) => amounts[type];
    public int GetLimit(GoodType type) => limits[type];
    
    public bool Add(GoodType type, int amount)
    {
        int spaceLeft = limits[type] - amounts[type];

        if (spaceLeft < amount)
        {
            return false;
        }

        amounts[type] += amount;
        return true;
    }

    /// Zwraca informacje o tym ile zostało REALNIE zużyte
    public int Use(GoodType type, int amount)
    {
        if (amount < 0) return 0;
        if (amounts[type] < amount) amount = amounts[type];
        amounts[type] -= amount;
        return amount;
    }

    public void ExpandLimit(int amount)
    {   
        foreach (var t in Enum.GetValues<GoodType>())
        {
            limits[t] += amount;
        }
    }

    public int CalculateScore()
    {
        int score = 0;
        foreach (var t in Enum.GetValues<GoodType>())
        {
            score += GetAmount(t);
            score += (GetLimit(t) - DeafultLimit) / 2; // dodatkowe punkty za ulepszenia w limicie
        }
        return score * StorageScoreMultiplier;
    }
}