using Zoo.Commands;

namespace Zoo;

public class GameDisplay
{
    private Map? savedMap = null;

    public List<string> Logs { get; private set; } = new();

    public void DisplayMap(Map? map = null)
    {
        if (map == null) map = savedMap;
        if (map == null) return;
        savedMap = map;
        
        string result = "";
        for (int i = 0; i < map.Height; i++)
        {
            for (int j = 0; j < map.Width; j++)
            {
                var location = map.Locations.FirstOrDefault(l => l.X == j && l.Y == i);
                if(location != null) result += location.Symbol();
                else result += " ";
            }

            result += "\n";
        }
        
        Console.WriteLine(result);
    }
    
    public (Command, List<string>) GetPlayerAction(List<Command> commands)
    {
        DisplayMessage("Wybierz akcję (wpisz 'akcje' aby zobaczyć listę dostępnych komend)");
        var args = Console.ReadLine()?.Trim().ToLower().Split(' ');
        var actionStr = args?[0];
        var action = commands.FirstOrDefault(x => x != null && x.ActionCommand() == actionStr, null);
        if (args == null || action == null)
        {
            DisplayWarning("Wybierz poprawną akcję. Sprawdź akcje pisząc 'akcje'");
            return GetPlayerAction(commands);
        }
        
        return (action, args.Skip(1).ToList());
    }

    public string GetPlayerResponse(string message)
    {
        DisplayMessage(message);
        return Console.ReadLine()?.Trim().ToLower() ?? string.Empty;
    }
    
    public void DisplayMessage(string message)
    {
        string formatted = $">> {message}";
        Console.WriteLine(formatted);
        Logs.Add(formatted);
    }
    
    public void DisplayInfo(string message)
    {
        string formatted = $">> [INFO] {message}";
        Console.WriteLine(formatted);
        Logs.Add(formatted);
    }
    
    public void DisplayWarning(string message)
    {
        string formatted = $">> [WARN] {message}";
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(formatted);
        Console.ResetColor();
        Logs.Add(formatted);
    }

    public void DisplayTitle(string title)
    {
        string formatted = $"=== {title} ===";
        Console.WriteLine(formatted);
        Logs.Add(formatted);
    }

    
}