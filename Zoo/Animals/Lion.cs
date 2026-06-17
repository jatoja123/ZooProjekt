using Zoo.Economy;

namespace Zoo.Animals;

public class Lion : Animal
{
    public Lion(string name) : base(name, GoodType.FoodMeat)
    {
        EnvironmentalNeeds.Add(new Zoo.Environment.CageType(Zoo.Environment.CageTypeEnum.Land));
        EnvironmentalNeeds.Add(new Zoo.Environment.TemperatureRequirement(20, 40));
    }
}
