using System.Linq;
using Zoo.Animals;

namespace Zoo.Economy;

public class MoneyController
{
    private const int StartingMoney = 100;
    private const int BaseIncomePerAnimal = 20;

    public int Money { get; private set; } = StartingMoney;

    public void CalculateIncome(AnimalsController animalsController)
    {
        int income = 0;

        var groupedByType = animalsController.Animals.GroupBy(a => a.GetType());

        //typy zwierzat
        foreach (var group in groupedByType)
        {
            var animalsOfType = group.ToList();
            for (int i = 0; i < animalsOfType.Count; i++)
            {
                var animal = animalsOfType[i];
                int baseValue = BaseIncomePerAnimal / (i + 1);
                //1-> 1, 2-> 0.5, 3-> 0.33, 4-> 0.25...
                //1- 20, 2- 10, 3 -6, 4-5, 5-4,...

                int condition = GetCondition(animal);
                income += baseValue * condition / 100;

            }
        }

        Money += income;
    }

    //tymczoasowe
    private int GetCondition(Animal animal)
    {
        return animal.AnimalNeeds.Min(n => n.GetValue());
    }
}

