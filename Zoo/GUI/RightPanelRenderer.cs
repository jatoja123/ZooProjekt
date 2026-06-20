using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;
using Zoo.Commands;
using Zoo;

namespace Zoo.GUI;

public class RightPanelRenderer : IRenderer
{
    public GUIState Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        Rectangle exitBtnRect = new Rectangle(screenWidth - 300, screenHeight - 80, 280, 50);
        Raylib.DrawRectangleRec(exitBtnRect, Color.DarkGray);
        Raylib.DrawText("Wyjscie", (int)exitBtnRect.X + 90, (int)exitBtnRect.Y + 12, 24, Color.White);

        if (isClicked && Raylib.CheckCollisionPointRec(mousePos, exitBtnRect))
        {
            return state.ShutdownGame().SetClickHandled(true);
        }

        var actionsToDisplay = GameController.PlayerActions.Where(x => x.IsVisible()).ToList();
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
                    return state.OpenLeftMenu(screenWidth - 300 - 220, btnY, cmd).SetClickHandled(true);
                }
                else
                {
                    ExecuteCommand(cmd, state);
                    return state.ClearInputBuffer().SelectTile(-1, -1).SetClickHandled(true);
                }
            }
        }
        
        return state;
    }

    private static GUIState ExecuteCommand(Command cmd, GUIState state)
{
    var args = string.IsNullOrWhiteSpace(state.InputBuffer)
        ? new List<string>()
        : state.InputBuffer.Split(' ').ToList();

    Task.Run(() =>
    {
        bool success = cmd.ExecuteCommand(args);
        if (!success)
        {
            string msg = cmd.LastExecutionMessage;
            GameGUI.StateDispatches.Enqueue(s => s.SetStatus(msg));
            GameGUI.EnqueuePopup(msg);
        }
    });

    return state.ClearInputBuffer().SelectTile(-1, -1);
}
}
