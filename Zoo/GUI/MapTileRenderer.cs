using System.Numerics;
using Raylib_cs;

namespace Zoo.GUI;

public static class MapTileRenderer
{
    public static void DrawTile(GameController controller, int x, int y, int mapStartX, int mapStartY, int spacing, int mapFontSize, Vector2 mousePos, bool isClicked, bool isOverUI, GUIState state)
    {
        var location = controller.Map.GetLocation(x, y);
        int posX = mapStartX + (x * spacing);
        int posY = mapStartY + (y * spacing);

        Rectangle tileRect = new Rectangle(posX, posY, mapFontSize, mapFontSize);

        if (!isOverUI && Raylib.CheckCollisionPointRec(mousePos, tileRect))
        {
            state.HoveredX = x;
            state.HoveredY = y;
            
            bool isStorage = location != null && location.Symbol() == 'M';

            if (isClicked && !isStorage)
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
            if (location is LocationHabitat habitat)
            {
                float fullness = habitat.Animals.Count / 4.0f;
                Color habitatColor = habitat is LocationHabitatLand 
                    ? new Color(0, (int)(100 + fullness * 155), 0, 255) 
                    : new Color(0, 0, (int)(100 + fullness * 155), 255);
                    
                Raylib.DrawRectangleRec(tileRect, habitatColor);
                Raylib.DrawRectangleLinesEx(tileRect, 2, Color.Black);
            }
            else if (location.Symbol().ToString() == "_")
            {
                Raylib.DrawRectangleLinesEx(tileRect, 2, Color.Black);
            }
            else
            {
                Raylib.DrawText(location.Symbol().ToString(), posX, posY, mapFontSize, Color.Black);
            }
        }

        if (state.SelectedX == x && state.SelectedY == y)
        {
            Raylib.DrawRectangleLinesEx(tileRect, 4, Color.Red);
        }
    }
}