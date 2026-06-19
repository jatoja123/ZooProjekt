using System.Linq;
using System.Numerics;
using Raylib_cs;
using Zoo;

namespace Zoo.GUI;

public class ConsoleRenderer : IRenderer
{
    private readonly bool isHabitatMode;

    public ConsoleRenderer(bool isHabitatMode)
    {
        this.isHabitatMode = isHabitatMode;
    }

    public GUIState Draw(GameController controller, int screenWidth, int screenHeight, Vector2 mousePos, bool isClicked, GUIState state)
    {
        int x = 20;
        int y = isHabitatMode ? screenHeight - 340 : screenHeight - 440;
        int width = isHabitatMode ? screenWidth - 40 : screenWidth - 350;
        int height = isHabitatMode ? 320 : 300;

        int titleY = isHabitatMode ? y - 25 : y - 40;
        int titleFontSize = 30;
        Color titleColor = isHabitatMode ? Color.RayWhite : Color.DarkGray;
        
        int logFontSize = 24;
        int logSpacing = 35;
        
        string titleText = isHabitatMode ? "Konsola:" : "Konsola gry (ostatnie komunikaty):";
        
        Raylib.DrawText(titleText, x, titleY, titleFontSize, titleColor);
        Raylib.DrawRectangle(x, y, width, height, Color.Black);
        Raylib.DrawRectangleLinesEx(new Rectangle(x, y, width, height), 2, Color.DarkGray);

        if (controller.ConsoleDisplay != null && controller.ConsoleDisplay.Logs != null)
        {
            var recentLogs = controller.ConsoleDisplay.Logs.TakeLast(8).ToList();
            for (int i = 0; i < recentLogs.Count; i++)
            {
                Raylib.DrawText(recentLogs[i], x + 15, y + 15 + (i * logSpacing), logFontSize, Color.Green);
            }
        }
        
        return state;
    }
}