using System.Numerics;
using Raylib_cs;
using Zoo;

namespace Zoo.GUI;

public class TopUIRenderer : IRenderer
{
    public GUIState Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        Raylib.DrawText($"Tura: {state.CurrentTurn} / {state.TotalTurns}", 20, 20, 30, Color.DarkGreen);
        Raylib.DrawText($"Pozostale akcje: {controller.ActionsLeft} / {controller.MaxActionCost}", 20, 60, 30, Color.DarkGreen);
        Raylib.DrawText($"Wpisz parametry (np. 0 0): {state.InputBuffer}", 20, screenHeight - 60, 30, Color.DarkBlue);
        Raylib.DrawText(state.StatusMessage, 20, screenHeight - 110, 30, Color.Red);
        
        return state;
    }
}