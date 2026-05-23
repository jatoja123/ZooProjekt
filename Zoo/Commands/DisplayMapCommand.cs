namespace Zoo.Commands;

public class DisplayMapCommand(GameController controller) : Command
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "mapa";
    public override string ActionDescription() => "Wyświetla mapę zoo";
    

    public override bool Execute(List<string> args)
    {
        controller.GameDisplay.DisplayMap();
        return true;
    }
}