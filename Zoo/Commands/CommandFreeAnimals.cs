using System.Collections.Generic;
using Zoo.Animals;

namespace Zoo.Commands;

public class CommandFreeAnimals(GameController controller) : Command(controller)
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "wolne";
    public override string ActionDescription() => "Pokazuje zwierzeta bez wybiegu / wsadza wolne zwierze na wybieg. Uzycie: wolne / wolne <index zwierzecia> <x> <y>";

    protected override bool Execute(List<string> args)
    {
        ClearExecutionMessage();

        if (args.Count == 3)
        {
            if (!int.TryParse(args[0], out var idx) || !int.TryParse(args[1], out var x) ||
                !int.TryParse(args[2], out var y))
            {
                SetExecutionMessage("Zly format akcji");
                controller.ConsoleDisplay.DisplayWarning("Zly format akcji");
                return false;
            }

            if (!MoveFreeAnimal(idx, x, y, out var failMessage))
            {
                SetExecutionMessage(failMessage);
                if (!string.IsNullOrWhiteSpace(failMessage))
                {
                    controller.ConsoleDisplay.DisplayWarning(failMessage);
                }
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

    private bool MoveFreeAnimal(int index, int x, int y, out string failMessage)
    {
        var freeAnimals = controller.AnimalsController.FreeAnimals;
        if (index < 0 || index >= freeAnimals.Count)
        {
            failMessage = "Nie istnieje zwierze o podanym indeksie";
            return false;
        }

        var location = controller.Map.GetLocation(x, y);
        if (location == null || location is not LocationHabitat habitat)
        {
            failMessage = "Nie można umieścić zwierzecia na tej lokalizacji";
            return false;
        }

        var animal = freeAnimals[index];
        if (!habitat.AddAnimal(animal, out failMessage))
        {
            return false;
        }

        failMessage = "";
        return true;
    }
}
