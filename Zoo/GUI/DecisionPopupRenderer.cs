// Zoo/GUI/DecisionPopupRenderer.cs
using System.Numerics;
using Raylib_cs;

namespace Zoo.GUI;

public class DecisionPopupRenderer
{
    public GUIState Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        if (!state.IsDecisionOpen || state.CurrentDecision == null) return state;

        Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, new Color(0, 0, 0, 150));

        int popupWidth = 600;
        int popupHeight = 300;
        int popupX = (screenWidth - popupWidth) / 2;
        int popupY = (screenHeight - popupHeight) / 2;

        Rectangle popupRect = new Rectangle(popupX, popupY, popupWidth, popupHeight);
        Raylib.DrawRectangleRec(popupRect, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(popupRect, 4, Color.Black);

        Raylib.DrawText("Decyzja", popupX + 20, popupY + 20, 30, Color.DarkBlue);
        Raylib.DrawText(state.CurrentDecision.Message, popupX + 20, popupY + 80, 22, Color.Black);

        int btnWidth = 220;
        int btnHeight = 50;

        Rectangle yesRect = new Rectangle(popupX + 30, popupY + popupHeight - 70, btnWidth, btnHeight);
        Rectangle noRect = new Rectangle(popupX + popupWidth - btnWidth - 30, popupY + popupHeight - 70, btnWidth, btnHeight);

        Raylib.DrawRectangleRec(yesRect, Color.Green);
        Raylib.DrawText(state.CurrentDecision.OptionYesLabel, (int)yesRect.X + 20, (int)yesRect.Y + 15, 20, Color.White);

        Raylib.DrawRectangleRec(noRect, Color.Red);
        Raylib.DrawText(state.CurrentDecision.OptionNoLabel, (int)noRect.X + 20, (int)noRect.Y + 15, 20, Color.White);

        if (isClicked && Raylib.CheckCollisionPointRec(mousePos, yesRect))
            return state.ResolveDecision(true).SetClickHandled(true);

        if (isClicked && Raylib.CheckCollisionPointRec(mousePos, noRect))
            return state.ResolveDecision(false).SetClickHandled(true);

        return state;
    }
}