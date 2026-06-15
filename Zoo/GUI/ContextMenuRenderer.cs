using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;
using Zoo.Commands;
using Zoo;

namespace Zoo.GUI;

public static class ContextMenuRenderer
{
    private const int MenuWidth = 220;
    private const int MenuItemHeight = 30;
    private const int SubMenuWidth = 200;

    public static void Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        if (!state.IsContextMenuOpen) return;

        List<Command> contextCommands;

        if (state.CurrentViewMode == ViewMode.HabitatView)
        {
            if (state.SelectedAnimal != null)
            {
                contextCommands = GameController.AnimalActions.ToList();
            }
            else
            {
                return;
            }
        }
        else if (state.SelectedX != -1)
        {
            contextCommands = GameController.MapActions.ToList();
        }
        else
        {
            return;
        }

        if (contextCommands.Count == 0) return;

        CalculateMainMenuBounds(screenWidth, screenHeight, contextCommands, state);
        RenderMainMenu(controller, mousePos, isClicked, contextCommands, state);

        if (state.IsSubMenuOpen)
        {
            CalculateSubMenuBounds(screenWidth, screenHeight, state);
            RenderSubMenu(controller, mousePos, isClicked, state);
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

    private static void RenderMainMenu(GameController controller, Vector2 mousePos, bool isClicked, List<Command> commands, GUIState state)
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
                    HandleMainMenuClick(controller, cmd, state);
                }
            }

            Raylib.DrawText(cmd.ActionCommand(), (int)state.ContextMenuRect.X + 10, (int)state.ContextMenuRect.Y + 10 + (i * MenuItemHeight), 20, Color.Black);
        }
    }

    private static void HandleMainMenuClick(GameController controller, Command cmd, GUIState state)
    {
        var subOptions = cmd.GetAvailableOptions();
        
        if (state.CurrentViewMode == ViewMode.HabitatView && (cmd.ActionCommand() == "nakarm" || cmd.ActionCommand() == "napoj"))
        {
            subOptions = new List<string> { "1", "2", "3", "5", "10" };
        }
        
        if (subOptions.Count > 0)
        {
            state.IsSubMenuOpen = true;
            state.CurrentSubOptions = subOptions;
            state.SelectedCommand = cmd;
            state.ClickHandled = true;
        }
        else
        {
            ExecuteFinalCommand(controller, cmd, state, "");
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

    private static void RenderSubMenu(GameController controller, Vector2 mousePos, bool isClicked, GUIState state)
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
                    if (state.SelectedCommand != null)
                    {
                        ExecuteFinalCommand(controller, state.SelectedCommand, state, option);
                    }
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

    private static void ExecuteFinalCommand(GameController controller, Command cmd, GUIState state, string additionalOption)
    {
        List<string> args = new List<string>();

        if (state.CurrentViewMode == ViewMode.HabitatView && state.SelectedAnimal != null && state.SelectedHabitat != null)
        {
            int animalIndex = state.SelectedHabitat.Animals.ToList().IndexOf(state.SelectedAnimal);
            
            args.Add(state.SelectedHabitat.X.ToString());
            args.Add(state.SelectedHabitat.Y.ToString());
            args.Add(animalIndex.ToString());

            string cmdName = cmd.ActionCommand();
            if ((cmdName == "nakarm" || cmdName == "napoj") && !string.IsNullOrWhiteSpace(additionalOption))
            {
                args.Add(additionalOption.Trim());
            }
        }
        else
        {
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
                    args.Add(additionalOption.Trim());
                }
            }

            if (cmd.ActionCommand() == "sprawdz")
            {
                int targetX = state.SelectedX;
                int targetY = state.SelectedY;

                if (args.Count >= 2 && int.TryParse(args[0], out int x) && int.TryParse(args[1], out int y))
                {
                    targetX = x;
                    targetY = y;
                }
                else if (args.Count == 0 && targetX != -1 && targetY != -1)
                {
                    args.Add(targetX.ToString());
                    args.Add(targetY.ToString());
                }

                var location = controller.Map.GetLocation(targetX, targetY);
                if (location is LocationHabitat habitat)
                {
                    state.SelectedHabitat = habitat;
                    state.CurrentViewMode = ViewMode.HabitatView;
                }
            }
        }

        Task.Run(() =>
        {
            bool success = cmd.Execute(args);
            
            if (success)
            {
                state.StatusMessage = $"Wykonano: {cmd.ActionCommand()}";
            }
            else
            {
                state.StatusMessage = "Blad parametrow!";
            }
        });

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