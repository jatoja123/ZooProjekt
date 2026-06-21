using Zoo.Economy;

namespace Zoo.Animals;

public class Penguin : Animal
{
    public Penguin(string name) : base(name, GoodType.FoodMeat, AgeRatio.FAST)
    {
        EnvironmentalNeeds.Add(new Zoo.Environment.CageType(Zoo.Environment.CageTypeEnum.Water));
        EnvironmentalNeeds.Add(new Zoo.Environment.TemperatureRequirement(2));
    }
}
