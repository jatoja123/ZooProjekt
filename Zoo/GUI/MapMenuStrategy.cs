using System.Collections.Generic;
using System.Linq;
using Zoo.Commands;

namespace Zoo.GUI;

public sealed class MapMenuStrategy : IMenuStrategy
{
    public List<Command> GetCommands(GUIState state) =>
        GameController.MapActions.ToList();

    public List<string> GetSubOptions(Command command, GUIState state) =>
        command.GetAvailableOptions();

    public List<string> BuildArgs(Command command, GUIState state, string selectedOption)
    {
        var args = new List<string>();
        string cmdName = command.ActionCommand();

        if (cmdName == "wolne" && !string.IsNullOrWhiteSpace(selectedOption))
        {
            string indexStr = selectedOption.Split(':')[0].Trim();
            args.Add(indexStr);
            args.Add(state.SelectedX.ToString());
            args.Add(state.SelectedY.ToString());
            return args;
        }

        if (state.SelectedX != -1)
        {
            args.Add(state.SelectedX.ToString());
            args.Add(state.SelectedY.ToString());
        }

        if (!string.IsNullOrWhiteSpace(state.InputBuffer))
            args.AddRange(state.InputBuffer.Split(' ', System.StringSplitOptions.RemoveEmptyEntries));

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

        if (command.ActionCommand() != "sprawdz")
            return false;

        int targetX = state.SelectedX;
        int targetY = state.SelectedY;

        var location = controller.Map.GetLocation(targetX, targetY);
        if (location is not LocationHabitat habitat)
            return false;

        int capturedX = targetX;
        int capturedY = targetY;
        GameGUI.StateDispatches.Enqueue(s =>
            s.SelectTile(capturedX, capturedY, habitat).SwitchView(ViewMode.HabitatView));

        return true;
    }
}