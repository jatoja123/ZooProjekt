using Zoo.Economy;

namespace Zoo.Animals;

public class Snake : Animal
{
    public Snake(string name) : base(name, GoodType.FoodMeat, AgeRatio.FAST)
    {
        EnvironmentalNeeds.Add(new Zoo.Environment.CageType(Zoo.Environment.CageTypeEnum.Land));
        EnvironmentalNeeds.Add(new Zoo.Environment.TemperatureRequirement(29));
    }
}
