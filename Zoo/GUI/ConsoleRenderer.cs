using System.Linq;
using Raylib_cs;

namespace Zoo.GUI;

public static class ConsoleRenderer
{
    public static void Draw(GameController controller, int x, int y, int width, int height, bool compactMode)
    {
        int titleY = compactMode ? y - 25 : y - 40;
        int titleFontSize = compactMode ? 20 : 30;
        Color titleColor = compactMode ? Color.RayWhite : Color.DarkGray;
        
        int logFontSize = compactMode ? 16 : 24;
        int logSpacing = compactMode ? 22 : 35;
        
        string titleText = compactMode ? "Konsola:" : "Konsola gry (ostatnie komunikaty):";
        
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
    }
}