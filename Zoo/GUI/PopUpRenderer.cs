using System.Numerics;
using Raylib_cs;
using Zoo;

namespace Zoo.GUI;

public class PopupRenderer
{
    public GUIState Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, new Color(0, 0, 0, 150));

        int popupWidth = 600;
        int popupHeight = 300;
        int popupX = (screenWidth - popupWidth) / 2;
        int popupY = (screenHeight - popupHeight) / 2;

        Rectangle popupRect = new Rectangle(popupX, popupY, popupWidth, popupHeight);
        Raylib.DrawRectangleRec(popupRect, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(popupRect, 4, Color.Black);

        Raylib.DrawText("Nowe Zdarzenie!", popupX + 20, popupY + 20, 30, Color.DarkBlue);
        Raylib.DrawText(state.CurrentPopupMessage, popupX + 20, popupY + 80, 24, Color.Black);

        int btnWidth = 150;
        int btnHeight = 50;
        Rectangle closeBtnRect = new Rectangle(popupX + (popupWidth - btnWidth) / 2, popupY + popupHeight - 70, btnWidth, btnHeight);

        Raylib.DrawRectangleRec(closeBtnRect, Color.LightGray);
        Raylib.DrawRectangleLinesEx(closeBtnRect, 2, Color.Black);
        Raylib.DrawText("Zamknij", (int)closeBtnRect.X + 35, (int)closeBtnRect.Y + 15, 20, Color.Black);

        if (isClicked && Raylib.CheckCollisionPointRec(mousePos, closeBtnRect))
        {
            return state.ClosePopup().SetClickHandled(true);
        }
        
        return state;
    }
}