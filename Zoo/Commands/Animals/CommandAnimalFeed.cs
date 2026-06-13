namespace Zoo.Commands.Animals;


public class CommandAnimalFeed(GameController controller) : Command
{
    public override int ActionCost => 1;
    public override string ActionCommand() => "nakarm";
    public override string ActionDescription() => "Karmi zwierzęta na wybranym wybiegu. Uzycie: nakarm <x> <y> <>";
    
    public override bool Execute(List<string> args)
    {
        if (args.Count != 3)
        {
            controller.GameDisplay.DisplayWarning("Zly format akcji");
            return false;
        }
        if(!int.TryParse(args[0], out var x) || !int.TryParse(args[1], out var y))
        {
            controller.GameDisplay.DisplayWarning("Zły format pozycji");
            return false;
        }
        
        var habitatType = args[2];
        if (!controller.Map.ChangeEnvironment(x, y, habitatType))
        {
            controller.GameDisplay.DisplayWarning("Nie udalo sie zbudowac wybiegu w tym miejscu");
            return false;
        }
        return true;
    }

    public override List<string> GetAvailableOptions()
    {
        return new List<string> { "ladowe", "wodne" };
    }
}