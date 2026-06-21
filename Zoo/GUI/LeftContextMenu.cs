using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;
using Zoo.Commands;
using Zoo;

namespace Zoo.GUI;

public class LeftContextMenu : IRenderer
{
    private const int MenuItemHeight = 30;
    private const int SubMenuWidth = 200;
    private readonly List<string> QuantityOptions = new() { "1", "2", "3", "5", "10" };

    public GUIState Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        if (!state.IsLeftMenuOpen) return state;

        if (!state.LeftOptionsReady)
        {
            string cmdName = state.LeftSelectedCommand?.ActionCommand() ?? "";
            List<string> subOptions;

            if (cmdName == "kupwode" || cmdName == "kupleki")
                subOptions = new List<string>(QuantityOptions);
            else
                subOptions = state.LeftSelectedCommand?.GetAvailableOptions() ?? new List<string>();

            if (subOptions.Count > 0)
                return state.SetupLeftSubMenu(subOptions, screenWidth, screenHeight);
            
            ExecuteFinalCommand(controller, state.LeftSelectedCommand!, state, new List<string>());
            return state.CloseAllMenus();
        }

        if (state.IsLeftSubMenuOpen)
        {
            state = RenderLeftSubMenu(controller, mousePos, isClicked, state);
        }

        return HandleOutsideClicks(mousePos, isClicked, state);
    }

    private GUIState RenderLeftSubMenu(GameController controller, Vector2 mousePos, bool isClicked, GUIState state)
    {
        Raylib.DrawRectangleRec(state.LeftSubMenuRect, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(state.LeftSubMenuRect, 1, Color.Black);

        var options = state.LeftSubOptions;
        for (int i = 0; i < options.Count; i++)
        {
            string option = options[i];
            Rectangle itemRect = new Rectangle(state.LeftSubMenuRect.X, state.LeftSubMenuRect.Y + 5 + (i * MenuItemHeight), SubMenuWidth, MenuItemHeight);

            if (Raylib.CheckCollisionPointRec(mousePos, itemRect))
            {
                Raylib.DrawRectangleRec(itemRect, Color.LightGray);
                if (isClicked && !state.ClickHandled)
                {
                    return HandleOptionClick(controller, option, state).SetClickHandled(true);
                }
            }
            Raylib.DrawText(option, (int)itemRect.X + 10, (int)itemRect.Y + 10, 20, Color.Black);
        }
        return state;
    }

    private GUIState HandleOptionClick(GameController controller, string option, GUIState state)
    {
        string cmdName = state.LeftSelectedCommand?.ActionCommand() ?? "";

        if (cmdName == "kupjedzenie" && state.LeftPendingArgs.Count == 0)
        {
            return state.AddLeftPendingArgument(option).UpdateLeftSubOptions(new List<string>(QuantityOptions));
        }

        var newState = state.AddLeftPendingArgument(option);
        ExecuteFinalCommand(controller, newState.LeftSelectedCommand!, newState, new List<string>(newState.LeftPendingArgs));
        return newState.CloseAllMenus();
    }

    private GUIState HandleOutsideClicks(Vector2 mousePos, bool isClicked, GUIState state)
    {
        if (!state.IsLeftMenuOpen) return state;

        bool clickedOutside = !Raylib.CheckCollisionPointRec(mousePos, state.LeftMenuRect);
        if (isClicked && !state.ClickHandled && clickedOutside)
            return state.CloseAllMenus();
        
        return state;
    }

    private void ExecuteFinalCommand(GameController controller, Command cmd, GUIState state, List<string> args)
    {
        var argsCopy = new List<string>(args);
        Task.Run(() =>
        {
            bool success = cmd.ExecuteCommand(argsCopy);
            if (!success)
            {
                string msg = cmd.LastExecutionMessage;
                GameGUI.StateDispatches.Enqueue(s => s.SetStatus(msg));
                controller.ConsoleDisplay.DisplayWarning(msg);
            }
        });
    }
}
