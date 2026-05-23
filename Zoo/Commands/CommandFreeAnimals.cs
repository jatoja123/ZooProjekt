using Zoo.Animals;

namespace Zoo.Commands;

public class CommandFreeAnimals(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "wolne";
    public override string ActionDescription() => "Pokazuje zwierzęta bez wybiegu / wsadza wolne zwierzę na wybieg. Użycie: wolne / wolne <index zwierzęcia> <x> <y>";
    

    public override bool Execute(List<string> args)
    {
        if (args.Count == 3)
        {
            if (!int.TryParse(args[0], out var idx) || !int.TryParse(args[1], out var x) ||
                !int.TryParse(args[2], out var y))
            {
                controller.GameDisplay.DisplayWarning("Zły format akcji");
                return false;
            }

            if (!MoveFreeAnimal(idx, x, y))
            {
                controller.GameDisplay.DisplayWarning("Nie udało się przenieść zwierzęcia w to miejsce");
                return false;
            }
            return true;
        }

        var freeAnimals = controller.AnimalsController.FreeAnimals;
        if (freeAnimals.Count == 0)
        {
            controller.GameDisplay.DisplayInfo("Brak zwierząt bez wybiegu");
            return true;
        }
        
        controller.GameDisplay.DisplayMessage("Zwierzęta bez wybiegu:");
        int i = 0;
        foreach (var animal in freeAnimals)
        {
            controller.GameDisplay.DisplayInfo($"({i}) {animal.GetType().Name}");
            i++;
        }
        return true;
    }

    private bool MoveFreeAnimal(int index, int x, int y)
    {
        var location = controller.Map.GetLocation(x, y);
        if (location == null || location is not LocationHabitat habitat) return false;
        var animal = controller.AnimalsController.FreeAnimals[index];
        return habitat.AddAnimal(animal);
    }
}