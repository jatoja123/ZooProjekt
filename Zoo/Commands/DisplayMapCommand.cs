namespace Zoo.Commands;

public class DisplayMapCommand(GameController controller) : ICommand
{
    public int ActionCost() => 0;

    public string ActionCommand() => "mapa";

    public bool Execute()
    {
        controller.GameDisplay.DisplayMap();
        return true;
    }
}