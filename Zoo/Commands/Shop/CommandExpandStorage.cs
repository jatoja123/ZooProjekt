using System.Collections.Generic;
using Zoo.Economy;

namespace Zoo.Commands;

public class CommandExpandStorage(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "rozszerz";
    public override string ActionDescription() => $"Rozszerza limit magazynu o {ShopPrices.ExpandAmount}. Koszt: {ShopPrices.ExpandsStorage}$. Uzycie: rozszerz <M/P/B/water/medicine>";

    public override bool Execute(List<string> args)
    {
        if (args.Count != 1)
        {
            controller.GameDisplay.DisplayWarning("Zla liczba argumentow akcji");
            return false;
        }

        GoodType type;
        switch (args[0].ToUpper())
        {
            case "M": type = GoodType.FoodMeat; break;
            case "P": type = GoodType.FoodPlant; break;
            case "B": type = GoodType.FoodMixed; break;
            case "WATER": type = GoodType.WATER; break;
            case "MEDICINE": type = GoodType.MEDICINE; break;
            default:
                controller.GameDisplay.DisplayWarning("Nieznany typ magazynu (M/P/B/water/medicine)");
                return false;
        }

        if (controller.MoneyController.Money < ShopPrices.ExpandsStorage)
        {
            controller.GameDisplay.DisplayWarning("Niewystarczająca ilosc pieniedzy");
            return false;
        }

        controller.MoneyController.Spend(ShopPrices.ExpandsStorage);
        controller.Storage.ExpandLimit(type, ShopPrices.ExpandAmount);
        controller.GameDisplay.DisplayInfo($"Rozszerzono limit {type} o {ShopPrices.ExpandAmount} za {ShopPrices.ExpandsStorage}$");
        return true;
    }
}