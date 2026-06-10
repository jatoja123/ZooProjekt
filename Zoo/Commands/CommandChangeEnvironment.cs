using System.Collections.Generic;

namespace Zoo.Commands;

public class CommandChangeEnvironment(GameController controller) : Command
{
    public override int ActionCost => 5;
    public override string ActionCommand() => "zmien";
    public override string ActionDescription() => $"Zmienia typ wybiegu. Uzycie: {ActionCommand()} <x> <y> <nowy typ>";
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
            controller.GameDisplay.DisplayWarning("Zly format pozycji");
            return false;
        }
        
        var newType = args[2];
        if (!controller.Map.ChangeEnvironment(x, y, newType))
        {
            controller.GameDisplay.DisplayWarning("Nie udalo się zmienic lokacji");
            return false;
        }
        return true;
    }

    public override List<string> GetAvailableOptions()
    {
        return new List<string> { "ladowe", "wodne" };
    }
}