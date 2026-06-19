using System.Collections.Generic;
using Zoo.Commands;

namespace Zoo.GUI;

public interface IMenuStrategy
{
    List<Command> GetCommands(GUIState state);

    List<string> GetSubOptions(Command command, GUIState state);

    List<string> BuildArgs(Command command, GUIState state, string selectedOption);

    bool RequiresNextStep(Command command, GUIState state, string selectedOption);

    List<string> GetNextStepOptions(Command command, GUIState state, string selectedOption);

    bool TryNavigate(GameController controller, Command command, GUIState state, out GUIState newState);
}