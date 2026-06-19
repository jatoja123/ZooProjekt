using System.Collections.Generic;

namespace Zoo.Commands;

public enum CommandVisibility { Console, GUI, Always }

public abstract class Command
{
    protected GameController controller;
    private CommandVisibility visibility;
    
    public Command(GameController controller, CommandVisibility visibility = CommandVisibility.Always)
    {
        this.controller = controller;
        this.visibility = visibility;
    }
    
    public virtual int ActionCost => 0;
    public abstract string ActionCommand();
    public abstract string ActionDescription();
    protected abstract bool Execute(List<string> args);
    
    public void SetVisibility(CommandVisibility visibility) => this.visibility = visibility;

    public bool IsVisible()
    {
        if (visibility == CommandVisibility.Always) return true;
        if(GameController.RunInConsole) return  visibility == CommandVisibility.Console;
        return visibility == CommandVisibility.GUI;
    }

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