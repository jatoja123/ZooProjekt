using Zoo.Economy;

namespace Zoo.Animals;

public class Turtle : Animal, IWaterAnimal
{
    public Turtle(string name) : base(name, GoodType.FoodMixed)
    {
    }
}
