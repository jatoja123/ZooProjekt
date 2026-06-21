using System.Collections.Generic;

namespace Zoo.Commands;

public class CommandSkipTurn(GameController controller) : Command(controller)
{
    public override int ActionCost => 0;

    public override string ActionCommand() => "pomin";
    public override string ActionDescription() => "Pomija i konczy ture";

    protected override bool Execute(List<string> args)
    {
        controller.SkipTurn();
        return true;
    }
}