namespace Zoo.Commands;

public interface ICommand
{
    int ActionCost();
    string ActionCommand();
    bool Execute();
}