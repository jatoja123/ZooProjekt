using System.Collections.Generic;
using Zoo.Animals;

namespace Zoo.Commands;

public class CommandRemoveAnimal(GameController controller) : Command(controller)
{
    public override int ActionCost => 2;

    public override string ActionCommand() => "wyjmij";
    public override string ActionDescription() => "Wyjmuje zwierze z wybiegu. Uzycie: wyjmij <x> <y>";

    protected override bool Execute(List<string> args)
    {
        if (args.Count != 2)
        {
            controller.ConsoleDisplay.DisplayWarning("Zla liczba argumentow akcji");
            return false;
        }
        if (!int.TryParse(args[0], out var x) || !int.TryParse(args[1], out var y))
        {
            controller.ConsoleDisplay.DisplayWarning("Zly format argumentow akcji");
            return false;
        }

        var location = controller.Map.GetLocation(x, y);
        if (location == null || location is not LocationHabitat habitat || habitat.Animals.Count == 0)
        {
            controller.ConsoleDisplay.DisplayWarning("Lokacja nie zawiera zwierzecia");
            return false;
        }
        
        var animal = habitat.Animals[0];
        habitat.RemoveAnimal(animal);
        
        return true;
    }
}