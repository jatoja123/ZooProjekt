using System.Collections.Generic;
using Zoo.Economy;

namespace Zoo.Commands;

public class CommandBuyWater(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "kupwode";
    public override string ActionDescription() => "Kupuje wodę. Użycie: kupwode <ilość>";

    public override bool Execute(List<string> args)
    {
        if (args.Count != 1 || !int.TryParse(args[0], out var amount) || amount <= 0)
        {
            controller.GameDisplay.DisplayWarning("Zły format ilości");
            return false;
        }

        int cost = amount * ShopPrices.WaterPrice;
        if (controller.MoneyController.Money < cost)
        {
            controller.GameDisplay.DisplayWarning("Niewystarczająca ilość pieniędzy");
            return false;
        }


        bool fullyAdded = controller.Storage.Add(GoodType.WATER, amount);

        if (!fullyAdded)
            controller.GameDisplay.DisplayWarning("Zakup przekracza limit magazynu, nie zakupiono");
        else
            controller.MoneyController.Spend(cost);
        controller.GameDisplay.DisplayInfo($"Kupiono {amount} wody za {cost}$");
        return true;
    }
}