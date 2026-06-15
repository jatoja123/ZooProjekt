using System;
using System.Collections.Generic;
using Zoo.Economy;

namespace Zoo.Commands;

public class CommandBuyFood(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "kupjedzenie";


    public override string ActionDescription() => "Kupuje jedzenie. Użycie: kupjedzenie <Mieso/Rosliny/Mieszane> <ilosc>";

    public override bool Execute(List<string> args)
    {
        if (args.Count != 2)
        {
            controller.ConsoleDisplay.DisplayWarning("Zla liczba arugmentpw akcji");
            return false;
        }

        GoodType type;
        switch (args[0].ToUpper())
        {
            case "Mieso": type = GoodType.FoodMeat; break;
            case "Rosliny": type = GoodType.FoodPlant; break;
            case "Mieszane": type = GoodType.FoodMixed; break;
            default:
                controller.ConsoleDisplay.DisplayWarning("Nieznany typ jedzenia - wybierz z <Mieso/Rosliny/Mieszane>");
                return false;
        }

        if (!int.TryParse(args[1], out var amount) || amount <= 0)
        {
            controller.ConsoleDisplay.DisplayWarning("Zly format ilosci");
            return false;
        }

        int cost = amount * ShopPrices.FoodPrice;

        if (controller.MoneyController.Money < cost)
        {
            controller.ConsoleDisplay.DisplayWarning("Niewystarczajaca ilosc pieniedzy");
            return false;
        }

        bool fullyAdded = controller.Storage.Add(type, amount);

        if (!fullyAdded)
        {
            controller.ConsoleDisplay.DisplayWarning($"Zakup przekracza limit magazynu, nie dokonano transakcji");
            return false;
        }
        else
        {
            controller.MoneyController.Spend(cost);
            controller.ConsoleDisplay.DisplayInfo($"Kupiono {amount} {type} za {cost}$");
        }
        return true;
    }

    public override List<string> GetAvailableOptions()
    {
        return new List<string> {"Mieso", "Rosliny", "Mieszane"};
    }
}