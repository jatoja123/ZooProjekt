using Zoo.Animals;
using Zoo.Economy;

namespace Zoo.Commands.Animals;


public class CommandAnimalPlay(GameController controller) : Command
{
    public override int ActionCost => 1;
    public override string ActionCommand() => "pobaw";
    public override string ActionDescription() => "Zabawia zwierze na wybiegu. Uzycie: pobaw <x> <y> <index zwierza>";
    
    public override bool Execute(List<string> args)
    {
        if (args.Count != 3)
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
        
        var animal = controller.AnimalsController.GetAnimal(x, y, idx);
        if (animal == null)
        {
            controller.ConsoleDisplay.DisplayWarning("Nie znaleziono zwierza na wybranej pozycji");
            return false;
        }
        
        animal.Play();
        controller.ConsoleDisplay.DisplayInfo($"Pobawiono sie z {animal.Name} - {AnimalNamesHelper.RandomPlayName()}");
        return true;
    }
}