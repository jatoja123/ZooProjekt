using Raylib_cs;

namespace Zoo.GUI;

public static class TopUIRenderer
{
    public static void Draw(int screenHeight, GUIState state)
    {
        Raylib.DrawText($"Tura: {TurnController.CurrentTurn} / {TurnController.TotalTurns}", 20, 20, 30, Color.DarkGreen);
        Raylib.DrawText($"Wpisz parametry (np. 0 0): {state.InputBuffer}", 20, screenHeight - 60, 30, Color.DarkBlue);
        Raylib.DrawText(state.StatusMessage, 20, screenHeight - 110, 30, Color.Red);
    }
}