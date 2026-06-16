using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;
using Zoo.Commands;
using Zoo;

namespace Zoo.GUI;

public static class LeftContextMenu
{
    private const int MenuWidth = 220;
    private const int MenuItemHeight = 30;
    private const int SubMenuWidth = 200;

    private static readonly List<string> QuantityOptions = new() { "1", "2", "3", "5", "10" };

    public static void Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        if (!state.IsLeftMenuOpen) return;

        if (!state.LeftOptionsReady)
        {
            string cmdName = state.LeftSelectedCommand?.ActionCommand() ?? "";
            List<string> subOptions;

            if (cmdName == "kupwode" || cmdName == "kupleki")
            {
                subOptions = new List<string>(QuantityOptions);
            }
            else
            {
                subOptions = state.LeftSelectedCommand?.GetAvailableOptions() ?? new List<string>();
            }

            if (subOptions.Count > 0)
            {
                state.LeftSubOptions = subOptions;
                state.LeftOptionsReady = true;
                state.IsLeftSubMenuOpen = true;
            }
            else
            {
                ExecuteFinalCommand(controller, state.LeftSelectedCommand!, state, new List<string>());
                return;
            }
        }

        if (state.IsLeftSubMenuOpen)
        {
            CalculateSubMenuBounds(screenWidth, screenHeight, state);
            RenderSubMenu(controller, mousePos, isClicked, state);
        }

        HandleOutsideClicks(mousePos, isClicked, state);
    }

    private static void CalculateSubMenuBounds(int screenWidth, int screenHeight, GUIState state)
    {
        int subMenuHeight = state.LeftSubOptions.Count * MenuItemHeight + 10;
        float subMenuX = state.LeftMenuX;
        float subMenuY = state.LeftMenuY;

        if (subMenuX + SubMenuWidth > screenWidth) subMenuX = screenWidth - SubMenuWidth - 5;
        if (subMenuY + subMenuHeight > screenHeight) subMenuY = screenHeight - subMenuHeight - 5;

        state.LeftSubMenuRect = new Rectangle(subMenuX, subMenuY, SubMenuWidth, subMenuHeight);
        state.LeftMenuRect = state.LeftSubMenuRect;
    }

    private static void RenderSubMenu(GameController controller, Vector2 mousePos, bool isClicked, GUIState state)
    {
        Raylib.DrawRectangleRec(state.LeftSubMenuRect, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(state.LeftSubMenuRect, 1, Color.Black);

        for (int i = 0; i < state.LeftSubOptions.Count; i++)
        {
            string option = state.LeftSubOptions[i];
            Rectangle itemRect = new Rectangle(state.LeftSubMenuRect.X, state.LeftSubMenuRect.Y + 5 + (i * MenuItemHeight), SubMenuWidth, MenuItemHeight);

            if (Raylib.CheckCollisionPointRec(mousePos, itemRect))
            {
                Raylib.DrawRectangleRec(itemRect, Color.LightGray);

                if (isClicked && state.LeftSelectedCommand != null)
                {
                    HandleSubMenuClick(controller, option, state);
                    return;
                }
            }

            Raylib.DrawText(option, (int)state.LeftSubMenuRect.X + 10, (int)state.LeftSubMenuRect.Y + 10 + (i * MenuItemHeight), 20, Color.Black);
        }
    }

    private static void HandleSubMenuClick(GameController controller, string option, GUIState state)
    {
        string cmdName = state.LeftSelectedCommand!.ActionCommand();

        if (cmdName == "kupjedzenie" && state.LeftPendingArgs.Count == 0)
        {
            state.LeftPendingArgs.Add(option);
            state.LeftSubOptions = new List<string>(QuantityOptions);
            state.LeftOptionsReady = true;
            state.ClickHandled = true;
            return;
        }

        state.LeftPendingArgs.Add(option);
        ExecuteFinalCommand(controller, state.LeftSelectedCommand!, state, new List<string>(state.LeftPendingArgs));
    }

    private static void HandleOutsideClicks(Vector2 mousePos, bool isClicked, GUIState state)
    {
        if (!state.IsLeftMenuOpen) return;

        bool clickedOutside = !Raylib.CheckCollisionPointRec(mousePos, state.LeftMenuRect);

        if (isClicked && !state.ClickHandled && clickedOutside)
        {
            CloseAllMenus(state);
        }
    }

    private static void ExecuteFinalCommand(GameController controller, Command cmd, GUIState state, List<string> args)
    {
        var argsCopy = new List<string>(args);
        Task.Run(() =>
        {
            bool success = cmd.ExecuteCommand(argsCopy);
            state.StatusMessage = success ? $"Wykonano: {cmd.ActionCommand()}" : $"Blad! Args: {string.Join(", ", argsCopy)}";
        });

        CloseAllMenus(state);
    }

    private static void CloseAllMenus(GUIState state)
    {
        state.IsLeftMenuOpen = false;
        state.IsLeftSubMenuOpen = false;
        state.LeftOptionsReady = false;
        state.LeftSubOptions.Clear();
        state.LeftPendingArgs.Clear();
        state.LeftSelectedCommand = null;
        state.ClickHandled = true;
    }
}