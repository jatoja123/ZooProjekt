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
            
            bool isOverUI = false;
            if (state.IsContextMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.ContextMenuRect)) isOverUI = true;
            if (state.IsSubMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.SubMenuRect)) isOverUI = true;

            Rectangle backButton = new Rectangle(20, 20, 200, 40);
            Raylib.DrawRectangleRec(backButton, Color.LightGray);
            Raylib.DrawText("Powrot do mapy", 35, 30, 20, Color.Black);

            if (isClicked && !isOverUI && Raylib.CheckCollisionPointRec(mousePos, backButton))
            {
                state.CurrentViewMode = ViewMode.MainMap;
                state.SelectedAnimal = null;
                GameController.PlayerActions = GameController.MainActions;
                state.IsContextMenuOpen = false;
                state.ClickHandled = true;
                return;
            }

            if (state.SelectedHabitat == null || state.SelectedHabitat.Animals == null)
            {
                return;
            }

            int cardWidth = 250;
            int cardHeight = 320;
            int spacingX = 20;
            int spacingY = 20;
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

                if (isHovered && isClicked && !isOverUI)
                {
                    state.SelectedAnimal = animal;
                    GameController.PlayerActions = GameController.AnimalActions;
                    state.IsContextMenuOpen = true;
                    state.ContextMenuX = mousePos.X + 15;
                    state.ContextMenuY = mousePos.Y - 30;
                    state.ClickHandled = true;
                }

                Rectangle imgRect = new Rectangle(currentX + 15, currentY + 15, cardWidth - 30, 200);
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

                Raylib.DrawText(animalName, currentX + 15, currentY + 230, 24, Color.Black);

                currentX += cardWidth + spacingX;

                if (currentX + cardWidth > screenWidth - 30)
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
                int statY = screenHeight - 550;

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