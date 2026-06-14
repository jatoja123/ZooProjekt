using System.Collections.Generic;

namespace Zoo.Commands;

public class CommandExitShop(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "wyjdz";
    public override string ActionDescription() => "Wychodzi ze sklepu. Uzycie: wyjdz";

    public override bool Execute(List<string> args)
    {
        GameController.PlayerActions = GameController.MainActions;
        controller.GameDisplay.DisplayInfo("Sklep");
        controller.GameDisplay.DisplayInfo($"Opuszczono sklep.");

        return true;
    }
}