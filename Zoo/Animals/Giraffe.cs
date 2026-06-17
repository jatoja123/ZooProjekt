using Zoo.Economy;

namespace Zoo.Animals;

public class Giraffe : Animal
{
    public Giraffe(string name) : base(name, GoodType.FoodPlant)
    {
        EnvironmentalNeeds.Add(new Zoo.Environment.CageType(Zoo.Environment.CageTypeEnum.Land));
        EnvironmentalNeeds.Add(new Zoo.Environment.TemperatureRequirement(15, 35));
    }
}
