using System.Collections.Generic;
using System.Linq;
using Zoo.Needs;

namespace Zoo.Commands;

public class CommandShowHabitat(GameController controller) : Command(controller)
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "sprawdz";
    public override string ActionDescription() => "Wydwietla informacje o lokacji i zwierzetach. Użycie: sprawdz <x> <y>";

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
        if (location == null)
        {
            controller.ConsoleDisplay.DisplayWarning("Lokacja nie istnieje");
            return false;
        }

        controller.ConsoleDisplay.DisplayInfo($"Lokacja: {location.Name()} [{location.Symbol()}]");

        if (location is LocationHabitat habitat)
        {
            if (habitat.Animals.Count == 0)
            {
                controller.ConsoleDisplay.DisplayInfo("Wybieg jest pusty.");
            }
            else
            {
                controller.ConsoleDisplay.DisplayInfo("Zwierzeta na wybiegu:");
                for (int i = 0; i < habitat.Animals.Count; i++)
                {
                    var animal = habitat.Animals[i];
                    
                    string needsStr = animal.AnimalNeeds.Count > 0 
                        ? string.Join(" | ", animal.AnimalNeeds.Select(n => $"{n.Type}: {n.Value}/{n.MaxValue}")) 
                        : "Brak potrzeb";

                    controller.ConsoleDisplay.DisplayInfo($"[{i}] {animal.Name} | {needsStr}");
                }
            }
        }
        return true;
    }
}