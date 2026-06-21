using System;
using System.Collections.Generic;
using Zoo.Animals;
using Zoo.Economy;

namespace Zoo.Commands;

public class CommandBuyAnimal(GameController controller) : Command(controller)
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "kup_zwierze";
    public override string ActionDescription() => $"Kupuje wybrane zwierze. Koszt: {ShopPrices.AnimalPrice}$. Wybierz typ zwierzecia.";

    protected override bool Execute(List<string> args)
    {
        if(args.Count < 1)
        {
            controller.ConsoleDisplay.DisplayWarning("Nie podano typu zwierzecia");
            return false;
        }


        if (controller.MoneyController.Money < ShopPrices.AnimalPrice)
        {
            controller.ConsoleDisplay.DisplayWarning("Niewystarczajaca ilosc pieniedzy");
            return false;
        }
        
        
        var AnimalTypeName = char.ToUpper(args[0][0]) + args[0][1..].ToLower();


        var animalType = AnimalsController.AnimalTypes
            .FirstOrDefault(t => t.Name == AnimalTypeName);

        if (animalType == null)
        {
            controller.ConsoleDisplay.DisplayWarning($"Nie znaleziono zwierzecia o nazwie {AnimalTypeName}");
            return false;
        }

        var animal = (Animal?)Activator.CreateInstance(animalType, AnimalNamesHelper.RandomName());

        if (animal == null)
        {
            controller.ConsoleDisplay.DisplayWarning("Nie udalo sie stworzyc zwierzecia");
            return false;
        }

        //umieszczanie zwierzaka ?? dopracować
        controller.MoneyController.Spend(ShopPrices.AnimalPrice);
        controller.AnimalsController.AddAnimal(animal);
        controller.ConsoleDisplay.DisplayInfo($"Kupiono zwierze: {animal.Name} za {ShopPrices.AnimalPrice}$");
        return true;
    }

    public override List<string> GetAvailableOptions()
    {
        return AnimalsController.AnimalTypes.Select(t => t.Name).ToList();
    }
}