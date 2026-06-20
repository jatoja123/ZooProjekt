using Zoo.Economy;

namespace Zoo.Animals;

public class Elephant : Animal
{
    public Elephant(string name) : base(name, GoodType.FoodPlant)
    {
        EnvironmentalNeeds.Add(new Zoo.Environment.CageType(Zoo.Environment.CageTypeEnum.Land));
        EnvironmentalNeeds.Add(new Zoo.Environment.TemperatureRequirement(25));
    }
}
