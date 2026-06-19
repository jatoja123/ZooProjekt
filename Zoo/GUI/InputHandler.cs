using Raylib_cs;

namespace Zoo.GUI;

public class InputHandler
{
    public GUIState HandleKeyboard(GUIState state)
    {
        int key = Raylib.GetCharPressed();
        if (key >= 32 && key <= 125) state = state.AppendInputChar((char)key);
        if (Raylib.IsKeyPressed(KeyboardKey.Backspace)) state = state.BackspaceInput();
        
        return state;
    }
}