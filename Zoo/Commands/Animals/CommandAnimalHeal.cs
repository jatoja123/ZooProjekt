using Zoo.Animals;
using Zoo.Economy;

namespace Zoo.Commands.Animals;


public class CommandAnimalHeal(GameController controller) : Command(controller)
{
    public override int ActionCost => 1;
    public override string ActionCommand() => "lecz";
    public override string ActionDescription() => "Leczy zwierze na wybiegu, zuzywając leki. Uzycie: pobaw <x> <y> <index zwierza> <ilosc lekow>";
    
    protected override bool Execute(List<string> args)
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
        
        var medicine_storage = controller.Storage.Use(animal.foodType, count);

        if (medicine_storage == 0)
        {
            controller.ConsoleDisplay.DisplayWarning("Brak lekow w magazynie");
            return true;
        }

        if (medicine_storage < count)
        {
            controller.ConsoleDisplay.DisplayWarning($"Brak wystarczajacej ilosci lekow w magazynie, uzyto tylko {medicine_storage}");
        }


        var medicine_used = animal.Feed(medicine_storage);

        if (medicine_used == 0)
        {
            controller.ConsoleDisplay.DisplayWarning($"{animal.Name} jest zdrow jak ryba ");

            controller.Storage.Add(animal.foodType, medicine_used);
            return true;
        }

        controller.ConsoleDisplay.DisplayInfo($"Wyleczono {animal.Name} o {medicine_used}");

        if (medicine_used < medicine_storage)
        {
            controller.Storage.Add(animal.foodType, medicine_storage - medicine_used);
        }
        return true;
    }
}