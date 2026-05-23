namespace Zoo.Commands;

public abstract class Command
{
    public virtual int ActionCost => 0;
    public abstract string ActionCommand();
    public abstract string ActionDescription();
    public abstract bool Execute(List<string> args);
}