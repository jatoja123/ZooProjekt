using System.Collections.Generic;
using Zoo.Economy;

namespace Zoo.Commands;

public class CommandBuyMedicine(GameController controller) : Command(controller)
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "kup_leki";
    public override string ActionDescription() => "Kupuje leki. Uzycie: kup_leki <ilosc>";

    protected override bool Execute(List<string> args)
    {
        if (args.Count != 1 || !int.TryParse(args[0], out var amount) || amount <= 0)
        {
            controller.ConsoleDisplay.DisplayWarning("Zly format - podaj ilosc lekow do kupienia");
            return false;
        }

        int cost = amount * ShopPrices.MedicinePrice;
        if (controller.MoneyController.Money < cost)
        {
            controller.ConsoleDisplay.DisplayWarning("Niewystarczajaca ilosc pieniedzy");
            return false;
        }


        bool fullyAdded = controller.Storage.Add(GoodType.MEDICINE, amount);

        if (!fullyAdded)
        {
            controller.ConsoleDisplay.DisplayWarning("Zakup przekracza limit magazynu, nie zakupiono");
            return false;
        }
        else{
            controller.MoneyController.Spend(cost);
            controller.ConsoleDisplay.DisplayInfo($"Kupiono {amount} lekow za {cost}$");
            return true;
        }
    }
}