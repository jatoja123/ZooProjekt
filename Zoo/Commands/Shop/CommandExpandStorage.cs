using System.Collections.Generic;
using Zoo.Economy;

namespace Zoo.Commands;

public class CommandExpandStorage(GameController controller) : Command(controller)
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "rozszerz";
    public override string ActionDescription() => $"Rozszerza limit magazynu o {ShopPrices.ExpandAmount}. Koszt: {ShopPrices.ExpandsStorage}$.";

    protected override bool Execute(List<string> args)
    {
        if (controller.MoneyController.Money < ShopPrices.ExpandsStorage)
        {
            controller.ConsoleDisplay.DisplayWarning("Niewystarczająca ilosc pieniedzy");
            return false;
        }

        controller.MoneyController.Spend(ShopPrices.ExpandsStorage);
        controller.Storage.ExpandLimit(ShopPrices.ExpandAmount);
        controller.ConsoleDisplay.DisplayInfo($"Rozszerzono limit o {ShopPrices.ExpandAmount} za {ShopPrices.ExpandsStorage}$");
        return true;
    }
}