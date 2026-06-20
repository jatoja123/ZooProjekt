using System.Collections.Generic;

namespace Zoo.Commands;

public enum CommandVisibility { Console, GUI, Always }

public abstract class Command
{
    protected GameController controller;
    private CommandVisibility visibility;
    private string lastExecutionMessage = "";
    
    public Command(GameController controller, CommandVisibility visibility = CommandVisibility.Always)
    {
        this.controller = controller;
        this.visibility = visibility;
    }
    
    public virtual int ActionCost => 0;
    public abstract string ActionCommand();
    public abstract string ActionDescription();
    protected abstract bool Execute(List<string> args);
    
    public string LastExecutionMessage => lastExecutionMessage;
    public void SetVisibility(CommandVisibility visibility) => this.visibility = visibility;

    protected void SetExecutionMessage(string message)
    {
        lastExecutionMessage = message;
    }

    protected void ClearExecutionMessage()
    {
        lastExecutionMessage = "";
    }

    public bool IsVisible()
    {
        if (visibility == CommandVisibility.Always) return true;
        if(GameController.RunInConsole) return  visibility == CommandVisibility.Console;
        return visibility == CommandVisibility.GUI;
    }

    public bool ExecuteCommand(List<string> args)
    {
        ClearExecutionMessage();
        if (!controller.CanExecuteAction(ActionCost))
        {
            if (string.IsNullOrWhiteSpace(lastExecutionMessage))
            {
                lastExecutionMessage = "Brak punktow akcji";
            }
            return false;
        }
        if (Execute(args))
        {
            controller.UseActionPoints(ActionCost);
            return true;
        }
        if (string.IsNullOrWhiteSpace(lastExecutionMessage))
        {
            lastExecutionMessage = "Blad parametrow!";
        }
        return false;
    }
    

    public virtual List<string> GetAvailableOptions()
    {
        return new List<string>();
    }
}
