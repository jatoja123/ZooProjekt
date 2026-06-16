using Zoo.Economy;

namespace Zoo.Commands.Animals;


public class CommandAnimalDrink(GameController controller) : Command(controller)
{
    public override int ActionCost => 1;
    public override string ActionCommand() => "napoj";
    public override string ActionDescription() => "Napaja zwierze na wybranym wybiegu. Uzycie: napoj <x> <y> <index zwierza> <ilosc wody>";
    
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

        var storage_water = controller.Storage.Use(GoodType.WATER, count);

        if (storage_water == 0)
        {
            controller.ConsoleDisplay.DisplayWarning("Brak wody w magazynie");
            return true;
        }

        if (storage_water < count)
        {
            controller.ConsoleDisplay.DisplayWarning($"Brak wystarczajacej ilosci wody w magazynie, wyjeto tylko {storage_water}");
        }

        var waterUsed  = animal.GiveWater(storage_water);

        if (waterUsed == 0)
        {
            controller.ConsoleDisplay.DisplayWarning($"{animal.Name} nie jest spragniony ");

            controller.Storage.Add(GoodType.WATER, storage_water);
            return true;
        }
        
        controller.ConsoleDisplay.DisplayInfo($"Napojono {animal.Name} o {waterUsed}");

        if (waterUsed < storage_water)
        {
            // zwróć niewykorzystaną wodę do magazynu
            controller.Storage.Add(GoodType.WATER, storage_water - waterUsed);
        }

        return true;
    }
}