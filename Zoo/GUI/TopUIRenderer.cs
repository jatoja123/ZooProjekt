using Raylib_cs;

namespace Zoo.GUI;

public static class TopUIRenderer
{
    public static void Draw(GameController controller, int screenHeight, GUIState state)
    {
        Raylib.DrawText($"Tura: {TurnController.CurrentTurn} / {TurnController.TotalTurns}", 20, 20, 30, Color.DarkGreen);
        Raylib.DrawText($"Pozostale akcje: {controller.ActionsLeft} / {controller.MaxAction}", 20, 60, 30, Color.DarkGreen);
        Raylib.DrawText($"Wpisz parametry (np. 0 0): {state.InputBuffer}", 20, screenHeight - 60, 30, Color.DarkBlue);
        Raylib.DrawText(state.StatusMessage, 20, screenHeight - 110, 30, Color.Red);
    }
}