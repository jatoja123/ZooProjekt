namespace Zoo.Commands;

public class CommandActionList(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "akcje";
    public override string ActionDescription() => "Wyświetla dostepne akcje.";

    public override bool Execute(List<string> args)
    {
        var actions = GameController.PlayerActions;
        actions.AddRange(GameController.MapActions);
        actions.AddRange(GameController.AnimalActions);
        foreach (var action in actions)
        {
            controller.GameDisplay.DisplayInfo($"{action.ActionCommand()} [{action.ActionCost}] {action.ActionDescription()}");
        }
        return true;
    }
}