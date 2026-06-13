using System.Numerics;
using Raylib_cs;

namespace Zoo.GUI;

public static class MapRenderer
{
    public static void Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        bool isOverUI = false;
        
        if (mousePos.X >= screenWidth - 300) isOverUI = true;
        if (state.IsContextMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.ContextMenuRect)) isOverUI = true;
        if (state.IsSubMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.SubMenuRect)) isOverUI = true;

        if (isOverUI)
        {
            isClicked = false;
        }

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

        int mapStartX = ((screenWidth - 300) - mapTotalWidth) / 2;
        int mapStartY = 100 + ((screenHeight - 480 - 100) - mapTotalHeight) / 2;

        if (mapStartX < 20) mapStartX = 20;
        if (mapStartY < 100) mapStartY = 100;

        Raylib.DrawText("Mapa ZOO:", mapStartX, mapStartY - 50, 30, Color.Black);

        for (int y = 0; y < controller.Map.Height; y++)
        {
            for (int x = 0; x < controller.Map.Width; x++)
            {
                DrawMapTile(controller, x, y, mapStartX, mapStartY, spacing, mapFontSize, mousePos, isClicked, isOverUI, state);
            }
        }
    }

    private static void DrawMapTile(GameController controller, int x, int y, int mapStartX, int mapStartY, int spacing, int mapFontSize, Vector2 mousePos, bool isClicked, bool isOverUI, GUIState state)
    {
        var location = controller.Map.GetLocation(x, y);
        int posX = mapStartX + (x * spacing);
        int posY = mapStartY + (y * spacing);

        Rectangle tileRect = new Rectangle(posX, posY, mapFontSize, mapFontSize);

        if (!isOverUI && Raylib.CheckCollisionPointRec(mousePos, tileRect))
        {
            state.HoveredX = x;
            state.HoveredY = y;
            
            if (isClicked)
            {
                state.InputBuffer = $"{x} {y}";
                state.SelectedX = x;
                state.SelectedY = y;
                state.IsContextMenuOpen = true;
                state.ContextMenuX = posX + mapFontSize + 5;
                state.ContextMenuY = posY;
                state.ClickHandled = true;
            }
        }

        if (location == null)
        {
            Raylib.DrawRectangleLinesEx(tileRect, 2, Color.Black);
        }
        else
        {
            string symbol = location.Symbol().ToString();
            
            if (symbol == "@")
            {
                Raylib.DrawRectangleRec(tileRect, Color.Green);
                Raylib.DrawRectangleLinesEx(tileRect, 2, Color.Black);
            }
            else if (symbol == "~")
            {
                Raylib.DrawRectangleRec(tileRect, Color.Blue);
                Raylib.DrawRectangleLinesEx(tileRect, 2, Color.Black);
            }
            else if (symbol == "_")
            {
                Raylib.DrawRectangleLinesEx(tileRect, 2, Color.Black);
            }
            else
            {
                Raylib.DrawText(symbol, posX, posY, mapFontSize, Color.Black);
            }
        }

        if (state.SelectedX == x && state.SelectedY == y)
        {
            Raylib.DrawRectangleLinesEx(tileRect, 4, Color.Red);
        }
    }
}