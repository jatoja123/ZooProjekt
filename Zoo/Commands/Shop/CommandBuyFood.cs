using System;
using System.Collections.Generic;
using Zoo.Economy;

namespace Zoo.Commands;

public class CommandBuyFood(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "kupjedzenie";


    public override string ActionDescription() => "Kupuje jedzenie M-mięsne, P-roślinne, B-mieszane. Użycie: kupjedzenie <M/P/B> <ilość>";

    public override bool Execute(List<string> args)
    {
        if (args.Count != 2)
        {
            controller.GameDisplay.DisplayWarning("Zła liczba arugmentów akcji");
            return false;
        }

        GoodType type;
        switch (args[0].ToUpper())
        {
            case "M": type = GoodType.FoodM; break;
            case "P": type = GoodType.FoodP; break;
            case "B": type = GoodType.FoodB; break;
            default:
                controller.GameDisplay.DisplayWarning("Nieznany typ jedzenia - wybierz z (M/P/B)");
                return false;
        }

        if (!int.TryParse(args[1], out var amount) || amount <= 0)
        {
            controller.GameDisplay.DisplayWarning("Zły format ilości");
            return false;
        }

        int cost = amount * ShopPrices.FoodPrice;

        if (controller.MoneyController.Money < cost)
        {
            controller.GameDisplay.DisplayWarning("Niewystarczająca ilość pieniędzy");
            return false;
        }

        bool fullyAdded = controller.Storage.Add(type, amount);

        if (!fullyAdded)
        {
            controller.GameDisplay.DisplayWarning($"Zakup przekracza limit magazynu, nie zakupiono");
        }
        else
        {
            controller.MoneyController.Spend(cost);
            controller.GameDisplay.DisplayInfo($"Kupiono {amount} {type} za {cost}$");
        }
        return true;
    }
}