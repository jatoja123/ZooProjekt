namespace Zoo;

public class GameDisplay
{
    public void DisplayMap(Map map)
    {
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
}