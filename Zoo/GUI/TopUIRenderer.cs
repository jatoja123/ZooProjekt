using Raylib_cs;

namespace Zoo.GUI;

public static class TopUIRenderer
{
    public static void Draw(GameController controller, int screenHeight, GUIState state, int turn, int totalTurns)
    {
        Raylib.DrawText($"Tura: {turn} / {totalTurns}", 20, 20, 30, Color.DarkGreen);
        Raylib.DrawText($"Pozostale akcje: {controller.ActionsLeft} / {controller.MaxActionCost}", 20, 60, 30, Color.DarkGreen);
        Raylib.DrawText($"Wpisz parametry (np. 0 0): {state.InputBuffer}", 20, screenHeight - 60, 30, Color.DarkBlue);
        Raylib.DrawText(state.StatusMessage, 20, screenHeight - 110, 30, Color.Red);
    }
}