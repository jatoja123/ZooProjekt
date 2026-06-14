using System.Collections.Generic;
using Zoo.Animals;

namespace Zoo.Commands;

public class CommandFreeAnimals(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "wolne";
    public override string ActionDescription() => "Pokazuje zwierzeta bez wybiegu / wsadza wolne zwierze na wybieg. Użycie: wolne / wolne <index zwierzęcia> <x> <y>";

    public override bool Execute(List<string> args)
    {
        if (args.Count == 3)
        {
            if (!int.TryParse(args[0], out var idx) || !int.TryParse(args[1], out var x) ||
                !int.TryParse(args[2], out var y))
            {
                controller.ConsoleDisplay.DisplayWarning("Zly format akcji");
                return false;
            }

            if (!MoveFreeAnimal(idx, x, y))
            {
                controller.ConsoleDisplay.DisplayWarning("Nie udalo sie przeniesc zwierzecia w to miejsce");
                return false;
            }
            return true;
        }

        var freeAnimals = controller.AnimalsController.FreeAnimals;
        if (freeAnimals.Count == 0)
        {
            controller.ConsoleDisplay.DisplayInfo("Brak zwierzat bez wybiegu");
            return true;
        }
        
        controller.ConsoleDisplay.DisplayMessage("Zwierzeta bez wybiegu:");
        int i = 0;
        foreach (var animal in freeAnimals)
        {
            controller.ConsoleDisplay.DisplayInfo($"({i}) {animal.Name}");
            i++;
        }
        return true;
    }

    public override List<string> GetAvailableOptions()
    {
        var options = new List<string>();
        var freeAnimals = controller.AnimalsController.FreeAnimals;
        
        for (int i = 0; i < freeAnimals.Count; i++)
        {
            options.Add($"{i}: {freeAnimals[i].Name}");
        }
        
        return options;
    }

    private bool MoveFreeAnimal(int index, int x, int y)
    {
        var location = controller.Map.GetLocation(x, y);
        if (location == null || location is not LocationHabitat habitat) return false;
        var animal = controller.AnimalsController.FreeAnimals[index];
        return habitat.AddAnimal(animal);
    }
}