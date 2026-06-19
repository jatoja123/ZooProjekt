using System.Collections.Generic;
using Zoo.Commands;

namespace Zoo.GUI;

public sealed class ShopMenuStrategy : IMenuStrategy
{
    private static readonly List<string> QuantityOptions = new() { "1", "2", "3", "5", "10" };
    private static readonly HashSet<string> DirectQuantityCommands = new() { "kupwode", "kupleki" };
    private const string FoodCommand = "kupjedzenie";

    public List<Command> GetCommands(GUIState state) =>
        new List<Command>();

    public List<string> GetSubOptions(Command command, GUIState state)
    {
        string name = command.ActionCommand();

        if (DirectQuantityCommands.Contains(name))
            return new List<string>(QuantityOptions);

        return command.GetAvailableOptions();
    }

    public bool RequiresNextStep(Command command, GUIState state, string selectedOption)
    {
        return command.ActionCommand() == FoodCommand && state.LeftPendingArgs.Count == 0;
    }

    public List<string> GetNextStepOptions(Command command, GUIState state, string selectedOption) =>
        new List<string>(QuantityOptions);

    public List<string> BuildArgs(Command command, GUIState state, string selectedOption)
    {
        var args = new List<string>(state.LeftPendingArgs);

        if (!string.IsNullOrWhiteSpace(selectedOption))
            args.Add(selectedOption.Trim());

        return args;
    }

    public bool TryNavigate(GameController controller, Command command, GUIState state, out GUIState newState)
    {
        newState = state;
        return false;
    }
}