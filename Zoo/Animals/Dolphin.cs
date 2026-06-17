using Zoo.Economy;

namespace Zoo.Animals;

public class Dolphin : Animal, IWaterAnimal
{
    public Dolphin(string name) : base(name, GoodType.FoodMeat)
    {
        EnvironmentalNeeds.Add(new Zoo.Environment.CageType(Zoo.Environment.CageTypeEnum.Water));
        EnvironmentalNeeds.Add(new Zoo.Environment.TemperatureRequirement(10, 25));
    }
}
