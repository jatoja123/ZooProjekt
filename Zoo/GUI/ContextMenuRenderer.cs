using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Raylib_cs;
using Zoo.Commands;

namespace Zoo.GUI;

public static class ContextMenuRenderer
{
    private const int MenuWidth = 220;
    private const int MenuItemHeight = 30;
    private const int SubMenuWidth = 200;

    public static void Draw(int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        if (!state.IsContextMenuOpen || state.SelectedX == -1) return;

        var contextCommands = GameController.PlayerActions.Where(c => c.RequiresCoordinates).ToList();

        if (contextCommands.Count == 0) return;

        CalculateMainMenuBounds(screenWidth, screenHeight, contextCommands, state);
        RenderMainMenu(mousePos, isClicked, contextCommands, state);

        if (state.IsSubMenuOpen)
        {
            CalculateSubMenuBounds(screenWidth, screenHeight, state);
            RenderSubMenu(mousePos, isClicked, state);
        }

        HandleOutsideClicks(mousePos, isClicked, state);
    }

    private static void CalculateMainMenuBounds(int screenWidth, int screenHeight, List<Command> commands, GUIState state)
    {
        int menuHeight = commands.Count * MenuItemHeight + 10;
        float menuX = state.ContextMenuX;
        float menuY = state.ContextMenuY;

        if (menuX + MenuWidth > screenWidth) menuX = screenWidth - MenuWidth - 5;
        if (menuY + menuHeight > screenHeight) menuY = screenHeight - menuHeight - 5;

        state.ContextMenuRect = new Rectangle(menuX, menuY, MenuWidth, menuHeight);
    }

    private static void RenderMainMenu(Vector2 mousePos, bool isClicked, List<Command> commands, GUIState state)
    {
        Raylib.DrawRectangleRec(state.ContextMenuRect, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(state.ContextMenuRect, 1, Color.Black);

        for (int i = 0; i < commands.Count; i++)
        {
            var cmd = commands[i];
            Rectangle itemRect = new Rectangle(state.ContextMenuRect.X, state.ContextMenuRect.Y + 5 + (i * MenuItemHeight), MenuWidth, MenuItemHeight);

            if (Raylib.CheckCollisionPointRec(mousePos, itemRect))
            {
                Raylib.DrawRectangleRec(itemRect, Color.LightGray);
                
                if (isClicked)
                {
                    HandleMainMenuClick(cmd, state);
                }
            }

            Raylib.DrawText(cmd.ActionCommand(), (int)state.ContextMenuRect.X + 10, (int)state.ContextMenuRect.Y + 10 + (i * MenuItemHeight), 20, Color.Black);
        }
    }

    private static void HandleMainMenuClick(Command cmd, GUIState state)
    {
        var subOptions = cmd.GetAvailableOptions();
        
        if (subOptions.Count > 0)
        {
            state.IsSubMenuOpen = true;
            state.CurrentSubOptions = subOptions;
            state.SelectedCommand = cmd;
            state.ClickHandled = true;
        }
        else
        {
            ExecuteFinalCommand(cmd, state, "");
        }
    }

    private static void CalculateSubMenuBounds(int screenWidth, int screenHeight, GUIState state)
    {
        int subMenuHeight = state.CurrentSubOptions.Count * MenuItemHeight + 10;
        float subMenuX = state.ContextMenuRect.X + state.ContextMenuRect.Width;
        float subMenuY = state.ContextMenuRect.Y;

        if (subMenuX + SubMenuWidth > screenWidth) subMenuX = state.ContextMenuRect.X - SubMenuWidth;
        if (subMenuY + subMenuHeight > screenHeight) subMenuY = screenHeight - subMenuHeight - 5;

        state.SubMenuRect = new Rectangle(subMenuX, subMenuY, SubMenuWidth, subMenuHeight);
    }

    private static void RenderSubMenu(Vector2 mousePos, bool isClicked, GUIState state)
    {
        Raylib.DrawRectangleRec(state.SubMenuRect, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(state.SubMenuRect, 1, Color.Black);

        for (int i = 0; i < state.CurrentSubOptions.Count; i++)
        {
            string option = state.CurrentSubOptions[i];
            Rectangle itemRect = new Rectangle(state.SubMenuRect.X, state.SubMenuRect.Y + 5 + (i * MenuItemHeight), SubMenuWidth, MenuItemHeight);

            if (Raylib.CheckCollisionPointRec(mousePos, itemRect))
            {
                Raylib.DrawRectangleRec(itemRect, Color.LightGray);

                if (isClicked)
                {
                    ExecuteFinalCommand(state.SelectedCommand, state, option);
                    return;
                }
            }

            Raylib.DrawText(option, (int)state.SubMenuRect.X + 10, (int)state.SubMenuRect.Y + 10 + (i * MenuItemHeight), 20, Color.Black);
        }
    }

    private static void HandleOutsideClicks(Vector2 mousePos, bool isClicked, GUIState state)
    {
        bool clickedOutsideMenu = !Raylib.CheckCollisionPointRec(mousePos, state.ContextMenuRect);
        bool clickedOutsideSubMenu = !state.IsSubMenuOpen || !Raylib.CheckCollisionPointRec(mousePos, state.SubMenuRect);

        if (isClicked && !state.ClickHandled && clickedOutsideMenu && clickedOutsideSubMenu)
        {
            CloseAllMenus(state);
        }
    }

    private static void ExecuteFinalCommand(Command cmd, GUIState state, string additionalOption)
    {
        List<string> args = new List<string>();

        if (cmd.ActionCommand() == "wolne" && !string.IsNullOrWhiteSpace(additionalOption))
        {
            string indexStr = additionalOption.Split(':')[0].Trim();
            args.Add(indexStr);
            args.Add(state.SelectedX.ToString());
            args.Add(state.SelectedY.ToString());
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(state.InputBuffer))
            {
                args = state.InputBuffer.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            if (!string.IsNullOrWhiteSpace(additionalOption))
            {
                args.Add(additionalOption);
            }
        }

        if (cmd.Execute(args))
        {
            state.StatusMessage = $"Wykonano: {cmd.ActionCommand()}";
        }
        else
        {
            state.StatusMessage = "Blad parametrow!";
        }
        
        CloseAllMenus(state);
    }

    private static void CloseAllMenus(GUIState state)
    {
        state.InputBuffer = "";
        state.SelectedX = -1;
        state.SelectedY = -1;
        state.IsContextMenuOpen = false;
        state.IsSubMenuOpen = false;
        state.CurrentSubOptions.Clear();
        state.SelectedCommand = null;
        state.ClickHandled = true;
    }
}