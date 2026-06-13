using Zoo.Economy;

namespace Zoo.Commands.Animals;


public class CommandAnimalFeed(GameController controller) : Command
{
    public override int ActionCost => 1;
    public override string ActionCommand() => "nakarm";
    public override string ActionDescription() => "Karmi zwierzę na wybranym wybiegu. Uzycie: nakarm <x> <y> <index zwierza> <ilosc jedzenia>";
    
    public override bool Execute(List<string> args)
    {
        if (args.Count != 4)
        {
            controller.GameDisplay.DisplayWarning("Zly format akcji");
            return false;
        }
        if(!int.TryParse(args[0], out var x) || !int.TryParse(args[1], out var y))
        {
            controller.GameDisplay.DisplayWarning("Zły format pozycji");
            return false;
        }
        if(!int.TryParse(args[2], out var idx))
        {
            controller.GameDisplay.DisplayWarning("Zły format indeksu");
            return false;
        }
        if(!int.TryParse(args[3], out var count))
        {
            controller.GameDisplay.DisplayWarning("Zły format ilosci");
            return false;
        }
        
        var animal = controller.AnimalsController.GetAnimal(x, y, idx);
        if (animal == null)
        {
            controller.GameDisplay.DisplayWarning("Nie znaleziono zwierza na wybranej pozycji");
            return false;
        }
        var foodUsed = controller.Storage.Use(animal.foodType, count);
        foodUsed = animal.Feed(foodUsed);
        controller.GameDisplay.DisplayInfo($"Nakarmiono {animal.Name} o {foodUsed}");
        return true;
    }

    public override List<string> GetAvailableOptions()
    {
        return new List<string> { "ladowe", "wodne" };
    }
}