using System.Numerics;
using Zoo;

namespace Zoo.GUI;

public interface IRenderer
{
    GUIState Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state);
}