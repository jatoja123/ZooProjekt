namespace Zoo.Commands;

public class CommandDisplayMap(GameController controller) : Command(controller)
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "mapa";
    public override string ActionDescription() => "Wyswietla mape zoo";
    

    protected override bool Execute(List<string> args)
    {
        controller.ConsoleDisplay.DisplayMap();
        return true;
    }
}