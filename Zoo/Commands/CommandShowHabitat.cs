using System.Collections.Generic;
using System.Linq;
using Zoo.Needs;

namespace Zoo.Commands;

public class CommandShowHabitat(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "sprawdz";
    public override string ActionDescription() => "Wyświetla informacje o lokacji i zwierzętach. Użycie: sprawdz <x> <y>";
    public override bool RequiresCoordinates => true;

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
        if (location == null)
        {
            controller.GameDisplay.DisplayWarning("Lokacja nie istnieje");
            return false;
        }

        controller.GameDisplay.DisplayInfo($"Lokacja: {location.Name()} [{location.Symbol()}]");

        if (location is LocationHabitat habitat)
        {
            if (habitat.Animals.Count == 0)
            {
                controller.GameDisplay.DisplayInfo("Wybieg jest pusty.");
            }
            else
            {
                controller.GameDisplay.DisplayInfo("Zwierzęta na wybiegu:");
                for (int i = 0; i < habitat.Animals.Count; i++)
                {
                    var animal = habitat.Animals[i];
                    
                    string needsStr = animal.AnimalNeeds.Count > 0 
                        ? string.Join(" | ", animal.AnimalNeeds.Select(n => $"{n.Type}: {n.GetValue()}/{n.GetMaxValue()}")) 
                        : "Brak potrzeb";

                    controller.GameDisplay.DisplayInfo($"[{i}] {animal.GetType().Name} | {needsStr}");
                }
            }
        }
        return true;
    }
}