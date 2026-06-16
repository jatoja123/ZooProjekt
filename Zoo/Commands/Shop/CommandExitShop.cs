using System.Collections.Generic;

namespace Zoo.Commands;

public class CommandExitShop(GameController controller) : Command(controller)
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "wyjdz";
    public override string ActionDescription() => "Wychodzi ze sklepu. Uzycie: wyjdz";

    protected override bool Execute(List<string> args)
    {
        GameController.PlayerActions = GameController.MainActions;
        controller.ConsoleDisplay.DisplayInfo("Sklep");
        controller.ConsoleDisplay.DisplayInfo($"Opuszczono sklep.");

        return true;
    }
}