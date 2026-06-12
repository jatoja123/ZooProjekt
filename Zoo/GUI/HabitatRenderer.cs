using Raylib_cs;
using System.Numerics;

namespace Zoo.GUI
{
    public static class HabitatRenderer
    {
        public static void Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
        {
            Raylib.ClearBackground(Color.DarkGreen);

            Rectangle backButton = new Rectangle(20, 20, 200, 40);
            Raylib.DrawRectangleRec(backButton, Color.LightGray);
            Raylib.DrawText("Powrot do mapy", 35, 30, 20, Color.Black);

            if (isClicked)
            {
                if (Raylib.CheckCollisionPointRec(mousePos, backButton))
                {
                    state.CurrentViewMode = ViewMode.MainMap;
                    state.SelectedHabitat = null;
                    return;
                }
            }

            if (state.SelectedHabitat == null || state.SelectedHabitat.Animals == null)
            {
                return;
            }

            int cardWidth = 200;
            int cardHeight = 260;
            int spacingX = 20;
            int spacingY = 20;
            int startX = 20;
            int startY = 80;

            int currentX = startX;
            int currentY = startY;

            foreach (var animal in state.SelectedHabitat.Animals)
            {
                string animalName = animal.GetType().Name;
                Console.WriteLine($"Szukam tekstury dla: {animalName}");
                Rectangle cardRect = new Rectangle(currentX, currentY, cardWidth, cardHeight);
                Raylib.DrawRectangleRec(cardRect, Color.RayWhite);
                Raylib.DrawRectangleLinesEx(cardRect, 2, Color.Black);

                Rectangle imgRect = new Rectangle(currentX + 10, currentY + 10, cardWidth - 20, 150);

                Texture2D? animalTexture = AssetLoader.GetAnimalTexture(animalName);
                Console.WriteLine(animalTexture.HasValue ? "Załadowano!" : "Brak tekstury!");

                if (animalTexture.HasValue)
                {
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
                    Raylib.DrawRectangleRec(imgRect, Color.DarkGray);
                }

                Raylib.DrawText(animalName, currentX + 10, currentY + 170, 20, Color.Black);

                currentX += cardWidth + spacingX;
                if (currentX + cardWidth > screenWidth)
                {
                    currentX = startX;
                    currentY += cardHeight + spacingY;
                }
            }
        }
    }
}