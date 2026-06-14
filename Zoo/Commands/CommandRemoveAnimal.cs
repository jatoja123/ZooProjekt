using System.Collections.Generic;
using Zoo.Animals;

namespace Zoo.Commands;

public class CommandRemoveAnimal(GameController controller) : Command
{
    public override int ActionCost => 2;

    public override string ActionCommand() => "wyjmij";
    public override string ActionDescription() => "Wyjmuje zwierzę z wybiegu (trzeba je znowu przydzielić). Użycie: wyjmij <x> <y>";

    public override bool Execute(List<string> args)
    {
        if (args.Count != 2)
        {
            controller.GameDisplay.DisplayWarning("Zła liczba argumentów akcji");
            return false;
        }
        if (!int.TryParse(args[0], out var x) || !int.TryParse(args[1], out var y))
        {
            controller.GameDisplay.DisplayWarning("Zły format argumentów akcji");
            return false;
        }

        var location = controller.Map.GetLocation(x, y);
        if (location == null || location is not LocationHabitat habitat || habitat.Animals.Count == 0)
        {
            controller.GameDisplay.DisplayWarning("Lokacja nie zawiera zwierzęcia");
            return false;
        }
        
        var animal = habitat.Animals[0];
        habitat.RemoveAnimal(animal);
        
        return true;
    }
}