using Raylib_cs;

namespace Zoo.GUI;

public static class InputHandler
{
    public static void HandleKeyboard(GUIState state)
    {
        int key = Raylib.GetCharPressed();
        if (key >= 32 && key <= 125) state.InputBuffer += (char)key;
        if (Raylib.IsKeyPressed(KeyboardKey.Backspace) && state.InputBuffer.Length > 0) 
            state.InputBuffer = state.InputBuffer.Remove(state.InputBuffer.Length - 1);
    }
}