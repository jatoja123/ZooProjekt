using Zoo.Economy;

namespace Zoo.Commands.Animals;


public class CommandAnimalFeed(GameController controller) : Command
{
    public override int ActionCost => 1;
    public override string ActionCommand() => "nakarm";
    public override string ActionDescription() => "Karmi zwierze na wybranym wybiegu. Uzycie: nakarm <x> <y> <index zwierza> <ilosc jedzenia>";
    
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

        var food_storage = controller.Storage.Use(animal.foodType, count);

        if (food_storage == 0)
        {
            controller.ConsoleDisplay.DisplayWarning("Brak jedzenia w magazynie");
            return true;
        }

        if (food_storage < count)
        {
            controller.ConsoleDisplay.DisplayWarning($"Brak wystarczajacej ilosci jedzenia w magazynie, wyjęto tylko {food_storage}");
        }


        var foodUsed = animal.Feed(food_storage);

        if (foodUsed == 0)
        {
            controller.ConsoleDisplay.DisplayWarning($"{animal.Name} nie jest glodny ");

            controller.Storage.Add(animal.foodType, food_storage);
            return true;
        }

        controller.ConsoleDisplay.DisplayInfo($"Nakarmiono {animal.Name} o {foodUsed}");

        if (foodUsed < food_storage)
        {
            controller.Storage.Add(animal.foodType, food_storage - foodUsed);
        }
        return true;
    }
}