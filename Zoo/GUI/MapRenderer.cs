using System.Numerics;
using System.Linq;
using Raylib_cs;
using Zoo.Animals;
using Zoo;

namespace Zoo.GUI;

public class MapRenderer : IRenderer
{
    private readonly AssetLoader assetLoader;

    public MapRenderer(AssetLoader assetLoader)
    {
        this.assetLoader = assetLoader;
    }

    public GUIState Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        bool isOverUI = IsMouseOverUI(screenWidth, mousePos, state);
        if (isOverUI) isClicked = false;

        state = state.UpdateHoveredTile(-1, -1);

        if (controller.Map == null)
        {
            Raylib.DrawText("Mapa ZOO:", 20, 20, 30, Color.Black);
            return state;
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
                state = DrawTile(controller, x, y, mapStartX, mapStartY, spacing, mapFontSize, mousePos, isClicked, isOverUI, state);
            }
        }

        DrawHoverInfo(controller, mapStartX, mapStartY, spacing, mapFontSize, state);
        return state;
    }

    private bool IsMouseOverUI(int screenWidth, Vector2 mousePos, GUIState state)
    {
        if (mousePos.X > screenWidth - 300) return true;
        if (state.IsContextMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.ContextMenuRect)) return true;
        if (state.IsSubMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.SubMenuRect)) return true;
        if (state.IsLeftMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.LeftMenuRect)) return true;
        if (state.IsLeftSubMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.LeftSubMenuRect)) return true;
        return false;
    }

    private GUIState DrawTile(GameController controller, int x, int y, int mapStartX, int mapStartY, int spacing, int mapFontSize, Vector2 mousePos, bool isClicked, bool isOverUI, GUIState state)
    {
        var location = controller.Map.GetLocation(x, y);
        int posX = mapStartX + (x * spacing);
        int posY = mapStartY + (y * spacing);

        Rectangle tileRect = new Rectangle(posX, posY, mapFontSize, mapFontSize);

        if (!isOverUI && Raylib.CheckCollisionPointRec(mousePos, tileRect))
        {
            state = state.UpdateHoveredTile(x, y);
            
            bool isStorage = location != null && location.Symbol() == 'M';

            if (isClicked && !isStorage && !state.ClickHandled)
            {
                state = state.SelectTile(x, y)
                              .OpenContextMenu(posX + mapFontSize + 5, posY, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), GameController.MapActions.Count(a => a.IsVisible()))
                              .SetClickHandled(true);
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
        
        return state;
    }

    private void DrawHoverInfo(GameController controller, int mapStartX, int mapStartY, int spacing, int mapFontSize, GUIState state)
    {
        if (state.HoveredX == -1 || state.HoveredY == -1) return;

        var hoveredLoc = controller.Map.GetLocation(state.HoveredX, state.HoveredY);
        if (hoveredLoc is LocationHabitat hoveredHabitat && hoveredHabitat.Animals.Count > 0)
        {
            int posX = mapStartX + (state.HoveredX * spacing);
            int posY = mapStartY + (state.HoveredY * spacing);
            
            var animal = hoveredHabitat.Animals[0];
            Texture2D? animalTexture = assetLoader.GetAnimalTexture(animal);
            
            int cardW = 160;
            int cardH = 160;
            int cardX = posX - cardW - 15;
            int cardY = posY + (mapFontSize / 2) - (cardH / 2);
            
            Raylib.DrawRectangle(cardX, cardY, cardW, cardH, Color.RayWhite);
            Raylib.DrawRectangleLinesEx(new Rectangle(cardX, cardY, cardW, cardH), 2, Color.Black);
            
            if (animalTexture.HasValue)
            {
                Rectangle imgRect = new Rectangle(cardX + 10, cardY + 10, cardW - 20, cardH - 20);
                Raylib.DrawTexturePro(
                    animalTexture.Value,
                    new Rectangle(0, 0, animalTexture.Value.Width, animalTexture.Value.Height),
                    imgRect,
                    new Vector2(0, 0),
                    0f,
                    Color.White
                );
            }
            else
            {
                Raylib.DrawRectangle(cardX + 10, cardY + 10, cardW - 20, cardH - 20, Color.DarkGray);
            }
            
            Raylib.DrawText(animal.Name, cardX + 20, cardY + cardH / 2 - 10, 20, Color.Black);
        }
    }
}