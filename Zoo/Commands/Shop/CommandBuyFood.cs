using System;
using System.Collections.Generic;
using Zoo.Economy;

namespace Zoo.Commands;

public class CommandBuyFood(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "kupjedzenie";


    public override string ActionDescription() => "Kupuje jedzenie M-miesne, P-roslinne, B-mieszane. Użycie: kupjedzenie <M/P/B> <ilosc>";

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
            case "M": type = GoodType.FoodMeat; break;
            case "P": type = GoodType.FoodPlant; break;
            case "B": type = GoodType.FoodMixed; break;
            default:
                controller.ConsoleDisplay.DisplayWarning("Nieznany typ jedzenia - wybierz z (M/P/B)");
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
}