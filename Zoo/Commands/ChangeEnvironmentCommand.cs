namespace Zoo.Commands;

public class ChangeEnvironmentCommand(GameController controller) : Command
{
    public override int ActionCost => 5;
    public override string ActionCommand() => "zmien";
    public override string ActionDescription() => $"Zmienia typ wybiegu. Użycie: {ActionCommand()} <x> <y> <nowy typ>";
    
    public override bool Execute(List<string> args)
    {
        if (args.Count != 3)
        {
            controller.GameDisplay.DisplayWarning("Zły format akcji");
            return false;
        }
        if(!int.TryParse(args[0], out var x) || !int.TryParse(args[1], out var y))
        {
            controller.GameDisplay.DisplayWarning("Zły format pozycji");
            return false;
        }
        
        var newType = args[2];
        if (!controller.Map.ChangeEnvironment(x, y, newType))
        {
            controller.GameDisplay.DisplayWarning("Nie udało się zmienić lokacji");
            return false;
        }
        return true;
    }
}