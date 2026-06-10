using System.Collections.Generic;

namespace Zoo.Commands;

public class CommandBuildHabitat(GameController controller) : Command
{
    public override int ActionCost => 5;
    public override string ActionCommand() => "buduj";
    public override string ActionDescription() => "Buduje wybieg na mapie. Uzycie: buduj <x> <y> <ladowe/wodne>";
    public override bool RequiresCoordinates => true;
    
    public override bool Execute(List<string> args)
    {
        if (args.Count != 3)
        {
            controller.GameDisplay.DisplayWarning("Zly format akcji");
            return false;
        }
        if(!int.TryParse(args[0], out var x) || !int.TryParse(args[1], out var y))
        {
            controller.GameDisplay.DisplayWarning("Zły format pozycji");
            return false;
        }
        
        var habitatType = args[2];
        if (!controller.Map.ChangeEnvironment(x, y, habitatType))
        {
            controller.GameDisplay.DisplayWarning("Nie udalo sie zbudowac wybiegu w tym miejscu");
            return false;
        }
        return true;
    }

    public override List<string> GetAvailableOptions()
    {
        return new List<string> { "ladowe", "wodne" };
    }
}