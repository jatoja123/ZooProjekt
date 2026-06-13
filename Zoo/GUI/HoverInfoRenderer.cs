using System.Numerics;
using Raylib_cs;

namespace Zoo.GUI;

public static class HoverInfoRenderer
{
    public static void Draw(GameController controller, int mapStartX, int mapStartY, int spacing, int mapFontSize, GUIState state)
    {
        if (state.HoveredX == -1 || state.HoveredY == -1) return;

        var hoveredLoc = controller.Map.GetLocation(state.HoveredX, state.HoveredY);
        if (hoveredLoc is LocationHabitat hoveredHabitat && hoveredHabitat.Animals.Count > 0)
        {
            int posX = mapStartX + (state.HoveredX * spacing);
            int posY = mapStartY + (state.HoveredY * spacing);
            
            string animalName = hoveredHabitat.Animals[0].GetType().Name;
            Texture2D? animalTexture = AssetLoader.GetAnimalTexture(animalName);
            
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
                Raylib.DrawText(animalName, cardX + 20, cardY + cardH / 2 - 10, 20, Color.Black);
            }
        }
    }
}