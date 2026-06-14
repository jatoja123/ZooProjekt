using System.Collections.Generic;
using Zoo.Economy;

namespace Zoo.Commands;

public class CommandBuyWater(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "kupwode";
    public override string ActionDescription() => "Kupuje wode. Uzycie: kupwode <ilość>";

    public override bool Execute(List<string> args)
    {
        if (args.Count != 1 || !int.TryParse(args[0], out var amount) || amount <= 0)
        {
            controller.GameDisplay.DisplayWarning("Zly format ilosci");
            return false;
        }

        int cost = amount * ShopPrices.WaterPrice;
        if (controller.MoneyController.Money < cost)
        {
            controller.GameDisplay.DisplayWarning("Niewystarczajaca ilosc pieniędzy");
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