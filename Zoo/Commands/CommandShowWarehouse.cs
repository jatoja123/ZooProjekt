using System.Collections.Generic;
using System.Runtime;
using Zoo.Economy;


namespace Zoo.Commands;

public class CommandShowWarehouse(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "magazyn";
    public override string ActionDescription() => "Wyświetla zawartość magazynu. Użycie: magazyn";

    public override bool Execute(List<string> args)
    {
        // gdy submenu zmienic
        controller.GameDisplay.DisplayInfo("Zawartość magazynu:");

        foreach (var type in System.Enum.GetValues<GoodType>())
        {
            int amount = controller.Storage.GetAmount(type);
            int limit = controller.Storage.GetLimit(type);
            controller.GameDisplay.DisplayInfo($"{type}: {amount}/{limit}");
        }

        return true;
    }
}