using System.Collections.Generic;

namespace Zoo.Commands;

public class CommandOpenShop(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "sklep";
    public override string ActionDescription() => "Otwiera sklep. Uzycie: sklep";

    public override bool Execute(List<string> args)
    {
        GameController.PlayerActions = GameController.ShopActions;
        controller.GameDisplay.DisplayInfo("Sklep");
        controller.GameDisplay.DisplayInfo($"Stan konta: {controller.MoneyController.Money}$");

        foreach (var action in GameController.ShopActions)
        {
            controller.GameDisplay.DisplayInfo($"{action.ActionCommand()} - {action.ActionDescription()}");
        }
        return true;
    }


}