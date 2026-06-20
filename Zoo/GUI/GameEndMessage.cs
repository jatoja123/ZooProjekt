using System;
using System.Numerics;
using Raylib_cs;
using Zoo;

namespace Zoo.GUI;

public class GameEndMessage
{
    public GUIState Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, new Color(0, 0, 0, 200));

        int popupWidth = 800;
        int popupHeight = 500;
        int popupX = (screenWidth - popupWidth) / 2;
        int popupY = (screenHeight - popupHeight) / 2;

        Rectangle popupRect = new Rectangle(popupX, popupY, popupWidth, popupHeight);
        Raylib.DrawRectangleRec(popupRect, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(popupRect, 6, Color.Black);

        string title = "KONIEC GRY";
        int titleFontSize = 50;
        int titleWidth = Raylib.MeasureText(title, titleFontSize);
        Raylib.DrawText(title, popupX + (popupWidth - titleWidth) / 2, popupY + 50, titleFontSize, Color.DarkBlue);

        string scoreMsg = $"Twoj wynik: {controller.GameScore.CurrentScore}";
        string maxScoreMsg = $"Najlepszy wynik: {controller.GameScore.MaxScore}";
        int scoreFontSize = 40;
        
        int scoreWidth = Raylib.MeasureText(scoreMsg, scoreFontSize);
        int maxScoreWidth = Raylib.MeasureText(maxScoreMsg, scoreFontSize);

        Raylib.DrawText(scoreMsg, popupX + (popupWidth - scoreWidth) / 2, popupY + 180, scoreFontSize, Color.Black);
        Raylib.DrawText(maxScoreMsg, popupX + (popupWidth - maxScoreWidth) / 2, popupY + 260, scoreFontSize, Color.Black);

        int btnWidth = 200;
        int btnHeight = 60;
        Rectangle exitBtnRect = new Rectangle(popupX + (popupWidth - btnWidth) / 2, popupY + popupHeight - 100, btnWidth, btnHeight);

        Raylib.DrawRectangleRec(exitBtnRect, Color.LightGray);
        Raylib.DrawRectangleLinesEx(exitBtnRect, 2, Color.Black);
        
        string btnText = "Zakoncz";
        int btnTextFontSize = 30;
        int btnTextWidth = Raylib.MeasureText(btnText, btnTextFontSize);
        Raylib.DrawText(btnText, (int)exitBtnRect.X + (btnWidth - btnTextWidth) / 2, (int)exitBtnRect.Y + 15, btnTextFontSize, Color.Black);

        if (isClicked && Raylib.CheckCollisionPointRec(mousePos, exitBtnRect))
        {
            System.Environment.Exit(0);
        }
        
        return state;
    }
}