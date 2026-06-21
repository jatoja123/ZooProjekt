using System.Diagnostics;
using Raylib_cs;
using System.Numerics;
using System.Linq;
using Zoo.Animals;
using Zoo;
using Zoo.Economy;

namespace Zoo.GUI;

public class HabitatRenderer : IRenderer
{
    private readonly AssetLoader assetLoader;

    public HabitatRenderer(AssetLoader assetLoader)
    {
        this.assetLoader = assetLoader;
    }

    public GUIState Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        Raylib.ClearBackground(Color.DarkGreen);
        
        bool isOverUI = false;
        if (state.IsContextMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.ContextMenuRect)) isOverUI = true;
        if (state.IsSubMenuOpen && Raylib.CheckCollisionPointRec(mousePos, state.SubMenuRect)) isOverUI = true;

        Rectangle backButton = new Rectangle(20, 20, 200, 40);
        Raylib.DrawRectangleRec(backButton, Color.LightGray);
        Raylib.DrawText("Powrot do mapy", 35, 30, 20, Color.Black);

        if (state.SelectedHabitat != null)
        {
            string tempText = $"Temperatura: {state.SelectedHabitat.Temperature[0]}C do {state.SelectedHabitat.Temperature[1]}C";
            int textWidth = Raylib.MeasureText(tempText, 24);
            Raylib.DrawText(tempText, screenWidth - textWidth - 20, 30, 24, Color.White);
        }

        if (isClicked && !isOverUI && Raylib.CheckCollisionPointRec(mousePos, backButton))
        {
            GameController.PlayerActions = GameController.MainActions;
            return state.SwitchView(ViewMode.MainMap).SetClickHandled(true);
        }

        if (state.SelectedHabitat == null || state.SelectedHabitat.Animals == null)
        {
            return state;
        }

        int cardWidth = 250;
        int cardHeight = 320;
        int spacingX = 40;
        int spacingY = 40;
        int startX = 250;
        int startY = 100;

        int currentX = startX;
        int currentY = startY;

        foreach (var animal in state.SelectedHabitat.Animals)
        {
            Rectangle cardRect = new Rectangle(currentX, currentY, cardWidth, cardHeight);
            bool isHovered = Raylib.CheckCollisionPointRec(mousePos, cardRect) && !isOverUI;

            if (isHovered)
            {
                Raylib.DrawRectangleRec(cardRect, Color.LightGray);
                Raylib.DrawRectangleLinesEx(cardRect, 3, Color.Gold);
            }
            else
            {
                Raylib.DrawRectangleRec(cardRect, Color.RayWhite);
                Raylib.DrawRectangleLinesEx(cardRect, 2, Color.Black);
            }

            if (isHovered && isClicked && !state.ClickHandled)
            {
                state = state with { SelectedAnimal = animal };
                state = state.OpenContextMenu(mousePos.X, mousePos.Y, screenWidth, screenHeight, GameController.AnimalActions.Count(a => a.IsVisible()));
                state = state.SetClickHandled(true);
            }

            Texture2D? animalTexture = assetLoader.GetAnimalTexture(animal);

            if (animalTexture.HasValue)
            {
                Rectangle imgRect = new Rectangle(currentX + 25, currentY + 20, 200, 200);
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
                Raylib.DrawRectangle(currentX + 25, currentY + 20, 200, 200, Color.DarkGray);
            }

            Raylib.DrawText(animal.Name, currentX + 15, currentY + 230, 24, Color.Black);

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

            string foodTypeStr = "";
            switch(state.SelectedAnimal.foodType)
            {
                case GoodType.FoodMeat: foodTypeStr = "MIESO"; break;
                case GoodType.FoodPlant: foodTypeStr = "ROSLINY"; break;
                case GoodType.FoodMixed: foodTypeStr = "MIESZANE"; break;
            }
            
            Raylib.DrawText($"Statystyki: {state.SelectedAnimal.Name} | Jedzenie: {foodTypeStr} | Temperatura: {state.SelectedAnimal.GetRequiredTemperature()}", statX + 20, statY + 20, 36, Color.Black);

            string needsStr = state.SelectedAnimal.AnimalNeeds.Count > 0 
                ? string.Join(" | ", state.SelectedAnimal.AnimalNeeds.Select(n => $"{n.Type}: {n.Value}/{n.MaxValue}")) 
                : "Brak potrzeb";

            Raylib.DrawText(needsStr, statX + 20, statY + 80, 24, Color.DarkBlue);
        }
        
        return state;
    }
}