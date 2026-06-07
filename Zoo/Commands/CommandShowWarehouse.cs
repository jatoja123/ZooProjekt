using System.Collections.Generic;

namespace Zoo.Commands;

public class CommandShowWarehouse(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "magazyn";
    public override string ActionDescription() => "Wyświetla zawartość magazynu. Użycie: magazyn";

    public override bool Execute(List<string> args)
    {
        if (controller.Warehouse.Count == 0)
        {
            controller.GameDisplay.DisplayInfo("Magazyn jest pusty.");
            return true;
        }

        controller.GameDisplay.DisplayInfo("Zawartość magazynu:");
        for (int i = 0; i < controller.Warehouse.Count; i++)
        {
            controller.GameDisplay.DisplayInfo($"[{i}] {controller.Warehouse[i]}");
        }
        
        return true;
    }
}