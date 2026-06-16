using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;
using Zoo.Commands;

namespace Zoo.GUI;

public static class RightPanelRenderer
{
    public static void Draw(int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        Rectangle exitBtnRect = new Rectangle(screenWidth - 300, screenHeight - 80, 280, 50);
        Raylib.DrawRectangleRec(exitBtnRect, Color.DarkGray);
        Raylib.DrawText("Wyjscie", (int)exitBtnRect.X + 90, (int)exitBtnRect.Y + 12, 24, Color.White);

        if (isClicked && Raylib.CheckCollisionPointRec(mousePos, exitBtnRect))
        {
            state.KeepRunning = false;
            state.ClickHandled = true;
        }

        var actionsToDisplay = GameController.PlayerActions.Where(x=> x.IsVisible()).ToList();
        int buttonsCount = actionsToDisplay.Count();
        for (int i = 0; i < buttonsCount; i++)
        {
            var cmd = actionsToDisplay[i];
            float btnY = exitBtnRect.Y - 20 - ((buttonsCount - i) * 60);
            Rectangle btnRect = new Rectangle(screenWidth - 300, btnY, 280, 50);
            Raylib.DrawRectangleRec(btnRect, Color.LightGray);
            Raylib.DrawText(cmd.ActionCommand(), (int)btnRect.X + 15, (int)btnRect.Y + 12, 24, Color.Black);

            if (isClicked && Raylib.CheckCollisionPointRec(mousePos, btnRect))
            {
                if (GameController.PlayerActions == GameController.ShopActions)
                {
                    state.IsLeftMenuOpen = true;
                    state.IsLeftSubMenuOpen = false;
                    state.LeftSubOptions.Clear();
                    state.LeftPendingArgs.Clear();
                    state.LeftMenuX = screenWidth - 300 - 220;
                    state.LeftMenuY = btnY;
                    state.LeftSelectedCommand = cmd;
                    state.ClickHandled = true;
                }
                else
                {
                    ExecuteCommand(cmd, state);
                    state.ClickHandled = true;
                }
            }
        }
    }

    private static void ExecuteCommand(Command cmd, GUIState state)
    {
        var args = string.IsNullOrWhiteSpace(state.InputBuffer)
            ? new List<string>()
            : state.InputBuffer.Split(' ').ToList();

        Task.Run(() =>
        {
            if (cmd.ExecuteCommand(args))
            {
                state.StatusMessage = $"Wykonano: {cmd.ActionCommand()}";
                state.InputBuffer = "";
                state.SelectedX = -1;
                state.SelectedY = -1;
            }
            else
            {
                state.StatusMessage = "Blad parametrow!";
            }
        });
    }
}