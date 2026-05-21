using Zoo.Commands;

namespace Zoo;

public class GameDisplay
{
    private Map? savedMap = null;
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
    
    public ICommand GetPlayerAction(List<ICommand> commands)
    {
        DisplayMessage("Wybierz akcje");
        var actionStr = Console.ReadLine()?.Trim().ToLower();
        var action = commands.FirstOrDefault(x => x.ActionCommand() == actionStr, null);
        if (action == null)
        {
            DisplayWarning("Wybierz poprawną akcję. Sprawdź akcje pisząc 'akcje'");
            return GetPlayerAction(commands);
        }
        
        return action;
    }

    public string GetPlayerResponse(string message)
    {
        DisplayMessage(message);
        return Console.ReadLine()?.Trim().ToLower();
    }
    
    public void DisplayMessage(string message)
    {
        Console.WriteLine($"[ {message} ]");
    }
    
    public void DisplayInfo(string message)
    {
        Console.WriteLine($"> {message}");
    }
    
    public void DisplayWarning(string message)
    {
        Console.WriteLine($"! {message} !");
    }
    
    public void DisplayTitle(string message)
    {
        message = message.ToUpper();
        Console.WriteLine($"==== {message} ====");
    }
}