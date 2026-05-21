namespace Zoo.Commands;

public class ChangeEnvironmentCommand(GameController controller) : ICommand
{
    public int ActionCost() => 5;
    public string ActionCommand() => "zmien";
    
    public bool Execute()
    {
        var response = controller.GameDisplay.GetPlayerResponse("<x> <y> <nowy typ>");
        var parts = response.Split(' ');
        if (parts.Length != 3)
        {
            controller.GameDisplay.DisplayWarning("Zły format akcji");
            return false;
        }
        
        var x = int.Parse(parts[0]);
        var y = int.Parse(parts[1]);
        var newType = parts[2];
        if (!controller.Map.ChangeEnvironment(x, y, newType))
        {
            controller.GameDisplay.DisplayWarning("Nie udało się zmienić lokacji");
            return false;
        }
        return true;
    }
}