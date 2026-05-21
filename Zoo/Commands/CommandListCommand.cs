namespace Zoo.Commands;

public class CommandListCommand(GameController controller) : ICommand
{
    public int ActionCost() => 0;

    public string ActionCommand() => "akcje";

    public bool Execute()
    {
        foreach (var action in GameController.PlayerActions)
        {
            controller.GameDisplay.DisplayInfo($"{action.ActionCommand()} [{action.ActionCost()}]");
        }
        return true;
    }
}