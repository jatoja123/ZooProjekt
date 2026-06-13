namespace Zoo.Economy;

public enum GoodType
{
    FoodM,
    FoodP,
    FoodB,
    WATER,
    MEDICINE
}

public class Storage
{
    private const int DeafultLimit = 10;

    private Dictionary<GoodType, int> amounts =
        Enum.GetValues<GoodType>().ToDictionary(t => t, _ => 0);
    private Dictionary<GoodType, int> limits =
        Enum.GetValues<GoodType>().ToDictionary(t => t, _ => DeafultLimit);

    public int GetAmount(GoodType type) => amounts[type];
    public int GetLimit(GoodType type) => limits[type];

    //dopelanianie do maxa?
    public bool Add(GoodType type, int amount)
    {
        int spaceLeft = limits[type] - amounts[type];

        if (spaceLeft == 0)
        {
            return false;
        }

        if (spaceLeft < amount)
        {
            amounts[type] = limits[type];
            return true;
        }

        amounts[type] += amount;
        return true;
    }

    public bool Use(GoodType type, int amount)
    {
        if (amounts[type] < amount) return false;
        amounts[type] -= amount;
        return true;
    }

    public void ExpandLimit(GoodType type, int amount)
    {
        limits[type] += amount;
    }
}