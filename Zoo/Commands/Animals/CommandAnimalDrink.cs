using Zoo.Economy;

namespace Zoo.Commands.Animals;


public class CommandAnimalDrink(GameController controller) : Command
{
    public override int ActionCost => 1;
    public override string ActionCommand() => "napoj";
    public override string ActionDescription() => "Napaja zwierzę na wybranym wybiegu. Uzycie: napoj <x> <y> <index zwierza> <ilosc wody>";
    
    public override bool Execute(List<string> args)
    {
        if (args.Count != 4)
        {
            controller.ConsoleDisplay.DisplayWarning("Zly format akcji");
            return false;
        }
        if(!int.TryParse(args[0], out var x) || !int.TryParse(args[1], out var y))
        {
            controller.ConsoleDisplay.DisplayWarning("Zly format pozycji");
            return false;
        }
        if(!int.TryParse(args[2], out var idx))
        {
            controller.ConsoleDisplay.DisplayWarning("Zly format indeksu");
            return false;
        }
        if(!int.TryParse(args[3], out var count))
        {
            controller.ConsoleDisplay.DisplayWarning("Zly format ilosci");
            return false;
        }
        
        var animal = controller.AnimalsController.GetAnimal(x, y, idx);
        if (animal == null)
        {
            controller.ConsoleDisplay.DisplayWarning("Nie znaleziono zwierza na wybranej pozycji");
            return false;
        }
        var waterUsed = controller.Storage.Use(animal.foodType, count);
        waterUsed = animal.GiveWater(waterUsed);
        controller.ConsoleDisplay.DisplayInfo($"Napojono {animal.Name} o {waterUsed}");
        return true;
    }
}