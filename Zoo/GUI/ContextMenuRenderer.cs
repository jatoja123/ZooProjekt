using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;
using Zoo.Commands;
using Zoo;

namespace Zoo.GUI;

public class ContextMenuRenderer : IRenderer
{
    private const int ItemHeight = 30;
    private const int MenuWidth = 220;
    private const int SubMenuWidth = 200;

    private readonly IMenuStrategy strategy;

    public ContextMenuRenderer(IMenuStrategy strategy)
    {
        this.strategy = strategy;
    }

    public GUIState Draw(
        GameController controller,
        int screenWidth, int screenHeight,
        Vector2 mousePos, bool isClicked,
        GUIState state)
    {
        if (!IsMenuOpen(state)) return state;

        if (strategy is ShopMenuStrategy)
            return DrawShopMenu(controller, screenWidth, screenHeight, mousePos, isClicked, state);

        return DrawContextMenu(controller, screenWidth, screenHeight, mousePos, isClicked, state);
    }

    private GUIState DrawContextMenu(
        GameController controller,
        int screenWidth, int screenHeight,
        Vector2 mousePos, bool isClicked,
        GUIState state)
    {
        if (!state.IsContextMenuOpen) return state;

        var commands = strategy.GetCommands(state);
        if (commands.Count == 0) return state;

        state = RenderMainMenu(controller, mousePos, isClicked, commands, screenWidth, screenHeight, state);

        if (state.IsSubMenuOpen)
            state = RenderSubMenu(controller, mousePos, isClicked, state);

        return HandleOutsideClick(mousePos, isClicked, state);
    }

    private GUIState RenderMainMenu(
        GameController controller,
        Vector2 mousePos, bool isClicked,
        List<Command> commands,
        int screenWidth, int screenHeight,
        GUIState state)
    {
        Raylib.DrawRectangleRec(state.ContextMenuRect, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(state.ContextMenuRect, 1, Color.Black);

        for (int i = 0; i < commands.Count; i++)
        {
            var cmd = commands[i];
            var itemRect = new Rectangle(
                state.ContextMenuRect.X,
                state.ContextMenuRect.Y + 5 + i * ItemHeight,
                MenuWidth, ItemHeight);

            if (Raylib.CheckCollisionPointRec(mousePos, itemRect))
            {
                Raylib.DrawRectangleRec(itemRect, Color.LightGray);

                if (isClicked && !state.ClickHandled)
                    return OnMainMenuClick(controller, cmd, screenWidth, screenHeight, state)
                           .SetClickHandled(true);
            }

            Raylib.DrawText(cmd.ActionCommand(), (int)itemRect.X + 10, (int)itemRect.Y + 3, 20, Color.Black);
        }

        return state;
    }

    private GUIState OnMainMenuClick(
        GameController controller,
        Command cmd,
        int screenWidth, int screenHeight,
        GUIState state)
    {
        var subOptions = strategy.GetSubOptions(cmd, state);

        if (subOptions.Count > 0)
            return state.OpenSubMenu(subOptions, cmd, screenWidth, screenHeight);

        return ExecuteAndClose(controller, cmd, state, "");
    }

    private GUIState RenderSubMenu(
        GameController controller,
        Vector2 mousePos, bool isClicked,
        GUIState state)
    {
        Raylib.DrawRectangleRec(state.SubMenuRect, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(state.SubMenuRect, 1, Color.Black);

        var options = state.CurrentSubOptions;
        for (int i = 0; i < options.Count; i++)
        {
            string option = options[i];
            var itemRect = new Rectangle(
                state.SubMenuRect.X,
                state.SubMenuRect.Y + 5 + i * ItemHeight,
                SubMenuWidth, ItemHeight);

            if (Raylib.CheckCollisionPointRec(mousePos, itemRect))
            {
                Raylib.DrawRectangleRec(itemRect, Color.LightGray);

                if (isClicked && !state.ClickHandled && state.SelectedCommand != null)
                    return ExecuteAndClose(controller, state.SelectedCommand, state, option)
                           .SetClickHandled(true);
            }

            Raylib.DrawText(option, (int)state.SubMenuRect.X + 10, (int)state.SubMenuRect.Y + 3 + i * ItemHeight, 20, Color.Black);
        }

        return state;
    }

    private GUIState HandleOutsideClick(Vector2 mousePos, bool isClicked, GUIState state)
    {
        bool outsideMain = !Raylib.CheckCollisionPointRec(mousePos, state.ContextMenuRect);
        bool outsideSub = !state.IsSubMenuOpen || !Raylib.CheckCollisionPointRec(mousePos, state.SubMenuRect);

        if (isClicked && !state.ClickHandled && outsideMain && outsideSub)
            return state.CloseAllMenus();

        return state;
    }

    private GUIState DrawShopMenu(
        GameController controller,
        int screenWidth, int screenHeight,
        Vector2 mousePos, bool isClicked,
        GUIState state)
    {
        if (!state.IsLeftMenuOpen) return state;

        if (!state.LeftOptionsReady)
        {
            if (state.LeftSelectedCommand == null)
                return state.CloseAllMenus();

            var options = strategy.GetSubOptions(state.LeftSelectedCommand, state);

            if (options.Count > 0)
                return state.SetupLeftSubMenu(options, screenWidth, screenHeight);

            DispatchCommand(controller, state.LeftSelectedCommand, state, "");
            return state.CloseAllMenus();
        }

        if (state.IsLeftSubMenuOpen)
            state = RenderShopSubMenu(controller, mousePos, isClicked, state);

        return HandleShopOutsideClick(mousePos, isClicked, state);
    }

    private GUIState RenderShopSubMenu(
        GameController controller,
        Vector2 mousePos, bool isClicked,
        GUIState state)
    {
        Raylib.DrawRectangleRec(state.LeftSubMenuRect, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(state.LeftSubMenuRect, 1, Color.Black);

        var options = state.LeftSubOptions;
        for (int i = 0; i < options.Count; i++)
        {
            string option = options[i];
            var itemRect = new Rectangle(
                state.LeftSubMenuRect.X,
                state.LeftSubMenuRect.Y + 5 + i * ItemHeight,
                SubMenuWidth, ItemHeight);

            if (Raylib.CheckCollisionPointRec(mousePos, itemRect))
            {
                Raylib.DrawRectangleRec(itemRect, Color.LightGray);

                if (isClicked && !state.ClickHandled)
                    return OnShopOptionClick(controller, option, state).SetClickHandled(true);
            }

            Raylib.DrawText(option, (int)itemRect.X + 10, (int)itemRect.Y + 10, 20, Color.Black);
        }

        return state;
    }

    private GUIState OnShopOptionClick(GameController controller, string option, GUIState state)
    {
        if (state.LeftSelectedCommand == null)
            return state.CloseAllMenus();

        if (strategy.RequiresNextStep(state.LeftSelectedCommand, state, option))
        {
            var nextOptions = strategy.GetNextStepOptions(state.LeftSelectedCommand, state, option);
            return state.AddLeftPendingArgument(option).UpdateLeftSubOptions(nextOptions);
        }

        DispatchCommand(controller, state.LeftSelectedCommand, state.AddLeftPendingArgument(option), option);
        return state.AddLeftPendingArgument(option).CloseAllMenus();
    }

    private GUIState HandleShopOutsideClick(Vector2 mousePos, bool isClicked, GUIState state)
    {
        if (!state.IsLeftMenuOpen) return state;

        bool outside = !Raylib.CheckCollisionPointRec(mousePos, state.LeftMenuRect);
        if (isClicked && !state.ClickHandled && outside)
            return state.CloseAllMenus();

        return state;
    }

    private GUIState ExecuteAndClose(
        GameController controller,
        Command cmd,
        GUIState state,
        string selectedOption)
    {
        if (strategy.TryNavigate(controller, cmd, state, out var navigatedState))
            return navigatedState.CloseAllMenus();

        DispatchCommand(controller, cmd, state, selectedOption);
        return state.CloseAllMenus();
    }

    private void DispatchCommand(
        GameController controller,
        Command cmd,
        GUIState state,
        string selectedOption)
    {
        var args = strategy.BuildArgs(cmd, state, selectedOption);
        var argsCopy = new List<string>(args);

        Task.Run(() =>
        {
            bool success = cmd.ExecuteCommand(argsCopy);
            if (!success)
            {
                string msg = cmd.LastExecutionMessage;
                GameGUI.StateDispatches.Enqueue(s => s.SetStatus(msg));
                GameGUI.EnqueuePopup(msg);
            }
        });
    }

    private bool IsMenuOpen(GUIState state) =>
        state.IsContextMenuOpen || state.IsLeftMenuOpen;
}
