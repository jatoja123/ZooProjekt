using Zoo.Economy;

namespace Zoo.Animals;

public class Dolphin : Animal, IWaterAnimal
{
    public Dolphin(string name) : base(name, GoodType.FoodMeat)
    {
    }
}
