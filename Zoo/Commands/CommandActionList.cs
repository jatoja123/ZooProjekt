namespace Zoo.Commands;

public class CommandActionList(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "akcje";
    public override string ActionDescription() => "Wyświetla dostepne akcje.";

    public override bool Execute(List<string> args)
    {
        foreach (var action in GameController.PlayerActions)
        {
            controller.GameDisplay.DisplayInfo($"{action.ActionCommand()} [{action.ActionCost}] {action.ActionDescription()}");
        }
        return true;
    }
}