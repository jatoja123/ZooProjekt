using Raylib_cs;
using System.Numerics;
using System.Linq;
using Zoo.Animals;

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

            if (isClicked && Raylib.CheckCollisionPointRec(mousePos, backButton))
            {
                state.CurrentViewMode = ViewMode.MainMap;
                state.SelectedAnimal = null;
                GameController.PlayerActions = GameController.MainActions;
                state.ClickHandled = true;
                return;
            }

            if (state.SelectedHabitat == null || state.SelectedHabitat.Animals == null)
            {
                return;
            }

            int cardWidth = 400;
            int cardHeight = 500;
            int spacingX = 30;
            int spacingY = 30;
            int startX = 30;
            int startY = 90;

            int currentX = startX;
            int currentY = startY;

            foreach (var animal in state.SelectedHabitat.Animals)
            {
                string animalName = animal.Name;
                bool isSelected = state.SelectedAnimal == animal;

                Rectangle cardRect = new Rectangle(currentX, currentY, cardWidth, cardHeight);
                bool isHovered = Raylib.CheckCollisionPointRec(mousePos, cardRect);

                Raylib.DrawRectangleRec(cardRect, isHovered || isSelected ? Color.LightGray : Color.RayWhite);
                Raylib.DrawRectangleLinesEx(cardRect, isSelected ? 4 : 2, isSelected ? Color.Blue : (isHovered ? Color.Red : Color.Black));

                if (isHovered && isClicked)
                {
                    state.SelectedAnimal = animal;
                    GameController.PlayerActions = GameController.AnimalActions;
                    state.IsContextMenuOpen = true;
                    state.ContextMenuX = mousePos.X;
                    state.ContextMenuY = mousePos.Y;
                    state.ClickHandled = true;
                }

                Rectangle imgRect = new Rectangle(currentX + 20, currentY + 20, cardWidth - 40, 330);
                Texture2D? animalTexture = AssetLoader.GetAnimalTexture(animal);

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

                Raylib.DrawText(animalName, currentX + 20, currentY + 380, 32, Color.Black);

                currentX += cardWidth + spacingX;

                if (currentX + cardWidth > screenWidth)
                {
                    currentX = startX;
                    currentY += cardHeight + spacingY;
                }
            }

            if (state.SelectedAnimal != null)
            {
                int statHeight = 150;
                int statWidth = screenWidth - 60;
                int statX = 30;
                int statY = screenHeight - statHeight - 30;

                Raylib.DrawRectangle(statX, statY, statWidth, statHeight, Color.RayWhite);
                Raylib.DrawRectangleLinesEx(new Rectangle(statX, statY, statWidth, statHeight), 3, Color.Black);

                string selName = state.SelectedAnimal.Name;
                Raylib.DrawText($"Statystyki: {selName}", statX + 20, statY + 20, 36, Color.Black);

                string needsStr = state.SelectedAnimal.AnimalNeeds.Count > 0 
                    ? string.Join(" | ", state.SelectedAnimal.AnimalNeeds.Select(n => $"{n.Type}: {n.Value}/{n.MaxValue}")) 
                    : "Brak potrzeb";

                Raylib.DrawText(needsStr, statX + 20, statY + 80, 24, Color.DarkBlue);
                
                string foodTypeStr = $"Dieta: {state.SelectedAnimal.foodType}";
                Raylib.DrawText(foodTypeStr, statX + 20, statY + 110, 24, Color.DarkGreen);
            }
        }
    }
}