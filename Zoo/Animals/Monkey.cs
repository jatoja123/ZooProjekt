using Zoo.Economy;

namespace Zoo.Animals;

public class Monkey : Animal
{
    public Monkey(string name) : base(name, GoodType.FoodMixed)
    {
        EnvironmentalNeeds.Add(new Zoo.Environment.CageType(Zoo.Environment.CageTypeEnum.Land));
        EnvironmentalNeeds.Add(new Zoo.Environment.TemperatureRequirement(18, 30));
    }
}
