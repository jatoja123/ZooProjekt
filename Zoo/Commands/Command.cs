using System.Collections.Generic;

namespace Zoo.Commands;

public abstract class Command
{
    private GameController controller;
    public Command(GameController controller)
    {
        this.controller = controller;
    }
    
    public virtual int ActionCost => 0;
    public abstract string ActionCommand();
    public abstract string ActionDescription();
    protected abstract bool Execute(List<string> args);

    public bool ExecuteCommand(List<string> args)
    {
        if (!controller.CanExecuteAction(ActionCost)) return false;
        if (Execute(args))
        {
            controller.UseActionPoints(ActionCost);
            return true;
        }
        return false;
    }
    

    public virtual List<string> GetAvailableOptions()
    {
        return new List<string>();
    }
}