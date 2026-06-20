using Zoo.Economy;

namespace Zoo.Animals;

public class Turtle : Animal, IWaterAnimal
{
    public Turtle(string name) : base(name, GoodType.FoodMixed)
    {
        EnvironmentalNeeds.Add(new Zoo.Environment.CageType(Zoo.Environment.CageTypeEnum.Water));
        EnvironmentalNeeds.Add(new Zoo.Environment.TemperatureRequirement(23));
    }
}
