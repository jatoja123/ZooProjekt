using System.Numerics;
using Raylib_cs;

namespace Zoo.GUI;

public static class MapRenderer
{
    public static void Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        bool isOverUI = IsMouseOverUI(screenWidth, mousePos, state);
        if (isOverUI) isClicked = false;

        state.HoveredX = -1;
        state.HoveredY = -1;

        if (controller.Map == null)
        {
            Raylib.DrawText("Mapa ZOO:", 20, 20, 30, Color.Black);
            return;
        }

        int mapFontSize = 85;
        int spacing = 95;
        int mapTotalWidth = controller.Map.Width * spacing;
        int mapTotalHeight = controller.Map.Height * spacing;

        int mapStartX = System.Math.Max(20, ((screenWidth - 300) - mapTotalWidth) / 2);
        int mapStartY = System.Math.Max(100, 100 + ((screenHeight - 580) - mapTotalHeight) / 2);

        Raylib.DrawText("Mapa ZOO:", mapStartX, mapStartY - 50, 30, Color.Black);

        for (int y = 0; y < controller.Map.Height; y++)
        {
            for (int x = 0; x < controller.Map.Width; x++)
            {
                MapTileRenderer.DrawTile(controller, x, y, mapStartX, mapStartY, spacing, mapFontSize, mousePos, isClicked, isOverUI, state);
            }
        }

        HoverInfoRenderer.Draw(controller, mapStartX, mapStartY, spacing, mapFontSize, state);
    }

    private static bool IsMouseOverUI(int screenWidth, Vector2 mousePos, GUIState state)
    {
        if (mousePos.X >= screenWidth - 300) return true;
        if (state.IsContextMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.ContextMenuRect)) return true;
        if (state.IsSubMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.SubMenuRect)) return true;
        return false;
    }
}