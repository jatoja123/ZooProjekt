using System.Collections.Generic;

namespace Zoo.Commands;

public class CommandChangeTemperature(GameController controller) : Command(controller)
{
    public override int ActionCost => 1;
    public override string ActionCommand() => "temperatura";
    public override string ActionDescription() => "Zmienia zakres temperatury na wybiegu. Uzycie: temperatura <x> <y> <zmiana>";
    
    protected override bool Execute(List<string> args)
    {
        if (args.Count != 3)
        {
            controller.ConsoleDisplay.DisplayWarning("Zly format akcji");
            return false;
        }
        if (!int.TryParse(args[0], out var x) || !int.TryParse(args[1], out var y))
        {
            controller.ConsoleDisplay.DisplayWarning("Zly format pozycji");
            return false;
        }
        
        var changeOption = args[2];
        if (!GetAvailableOptions().Contains(changeOption) || !int.TryParse(changeOption, out var change))
        {
            controller.ConsoleDisplay.DisplayWarning("Zly format temperatury");
            return false;
        }
        
        var location = controller.Map.GetLocation(x, y);
        if (location == null || location is not LocationHabitat habitat)
        {
            controller.ConsoleDisplay.DisplayWarning("W tym miejscu nie ma wybiegu");
            return false;
        }

        if (change < 0)
        {
            habitat.Temperature[0] += change;
        }
        else if (change > 0)
        {
            habitat.Temperature[1] += change;
        }

        return true;
    }

    public override List<string> GetAvailableOptions()
    {
        return new List<string> { "-5", "-1", "1", "5" };
    }
}