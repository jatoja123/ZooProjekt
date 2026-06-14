using System;
using System.Collections.Generic;
using Zoo.Animals;
using Zoo.Economy;

namespace Zoo.Commands;

public class CommandBuyAnimal(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "kupzwierze";
    public override string ActionDescription() => $"Kupuje losowe zwierze. Koszt: {ShopPrices.AnimalPrice}$";

    public override bool Execute(List<string> args)
    {
        if (controller.MoneyController.Money < ShopPrices.AnimalPrice)
        {
            controller.GameDisplay.DisplayWarning("Niewystarczajaca ilosc pieniedzy");
            return false;
        }

        var rnd = new Random();
        var animalType = AnimalsController.AnimalTypes[rnd.Next(AnimalsController.AnimalTypes.Count)];
        var animal = (Animal?)Activator.CreateInstance(animalType, AnimalNamesHelper.RandomName());

        if (animal == null)
        {
            controller.GameDisplay.DisplayWarning("Nie udalo sie stworzyc zwierzecia");
            return false;
        }


        //umieszczanie zwierzaka ?? dopracować
        controller.MoneyController.Spend(ShopPrices.AnimalPrice);
        controller.AnimalsController.AddAnimal(animal);
        controller.GameDisplay.DisplayInfo($"Kupiono zwierze: {animal.Name} za {ShopPrices.AnimalPrice}$");
        return true;
    }
}