namespace Zoo.Commands;

public class CommandDisplayMap(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "mapa";
    public override string ActionDescription() => "Wyswietla mape zoo";
    

    public override bool Execute(List<string> args)
    {
        controller.GameDisplay.DisplayMap();
        return true;
    }
}