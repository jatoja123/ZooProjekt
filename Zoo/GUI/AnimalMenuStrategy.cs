using System.Collections.Generic;
using System.Linq;
using Zoo.Commands;

namespace Zoo.GUI;

public sealed class AnimalMenuStrategy : IMenuStrategy
{
    private static readonly List<string> QuantityOptions = new() { "1", "2", "3", "5", "10" };
    private static readonly HashSet<string> QuantityCommands = new() { "nakarm", "napoj", "lecz" };

    public List<Command> GetCommands(GUIState state) =>
        GameController.AnimalActions.ToList();

    public List<string> GetSubOptions(Command command, GUIState state)
    {
        string name = command.ActionCommand();
        return QuantityCommands.Contains(name) ? new List<string>(QuantityOptions) : new List<string>();
    }

    public List<string> BuildArgs(Command command, GUIState state, string selectedOption)
    {
        var args = new List<string>();

        if (state.SelectedHabitat == null || state.SelectedAnimal == null)
            return args;

        int animalIndex = state.SelectedHabitat.Animals.ToList().IndexOf(state.SelectedAnimal);
        args.Add(state.SelectedHabitat.X.ToString());
        args.Add(state.SelectedHabitat.Y.ToString());
        args.Add(animalIndex.ToString());

        if (!string.IsNullOrWhiteSpace(selectedOption))
            args.Add(selectedOption.Trim());

        return args;
    }

    public bool RequiresNextStep(Command command, GUIState state, string selectedOption) => false;

    public List<string> GetNextStepOptions(Command command, GUIState state, string selectedOption) =>
        new List<string>();

    public bool TryNavigate(GameController controller, Command command, GUIState state, out GUIState newState)
    {
        newState = state;
        return false;
    }
}