using System.Collections.Generic;
using System.Runtime;
using Zoo.Economy;


namespace Zoo.Commands;

public class CommandShowWarehouse(GameController controller) : Command(controller)
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "magazyn";
    public override string ActionDescription() => "Wyswietla zawartosc magazynu. Uzycie: magazyn";

    protected override bool Execute(List<string> args)
    {
        // gdy submenu zmienic
        controller.ConsoleDisplay.DisplayInfo("Zawartosc magazynu:");

        foreach (var type in System.Enum.GetValues<GoodType>())
        {
            int amount = controller.Storage.GetAmount(type);
            int limit = controller.Storage.GetLimit(type);
            controller.ConsoleDisplay.DisplayInfo($"{type}: {amount}/{limit}");
        }

        return true;
    }
}