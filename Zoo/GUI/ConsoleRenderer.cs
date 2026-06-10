using System.Linq;
using Raylib_cs;

namespace Zoo.GUI;

public static class ConsoleRenderer
{
    public static void Draw(GameController controller, int screenWidth, int screenHeight)
    {
        Raylib.DrawText("Konsola gry (ostatnie komunikaty):", 20, screenHeight - 480, 30, Color.DarkGray);
        Raylib.DrawRectangle(20, screenHeight - 440, screenWidth - 350, 300, Color.Black);

        if (controller.GameDisplay != null && controller.GameDisplay.Logs != null)
        {
            var recentLogs = controller.GameDisplay.Logs.TakeLast(8).ToList();
            for (int i = 0; i < recentLogs.Count; i++)
            {
                Raylib.DrawText(recentLogs[i], 35, (screenHeight - 425) + (i * 35), 24, Color.Green);
            }
        }
    }
}