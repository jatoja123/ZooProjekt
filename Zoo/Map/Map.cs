namespace Zoo;

public class Map
{
    private static int width = 10;
    private static int height = 10;
    private static Dictionary<string, Type> locationTypes = new()
    {
        { "puste", typeof(LocationEmpty) },
        { "ladowe", typeof(LocationHabitatLand) }
    };
    
    public int Width => width;
    public int Height => height;
    public List<Location> Locations = new ();
    
    public void Initialize()
    {
        for (int h = 0; h < height * width; h++)
        {
            Location location = new LocationEmpty(h % width, h / width);
            Locations.Add(location);
        }
    }

    public bool ChangeEnvironment(int x, int y, string newType)
    {
        var location = GetLocation(x, y);
        if (location == null || !location.CanBeReplaced()) return false;
        
        if(!locationTypes.TryGetValue(newType, out var locationType)) return false;
        var newLocation = (Location?)Activator.CreateInstance(locationType, x, y);
        if (newLocation == null) return false;
        
        Locations.Remove(location);
        Locations.Add(newLocation);
        return true;
    }

    private Location? GetLocation(int x, int y) => Locations.FirstOrDefault(l => l?.X == x && l?.Y == y, null);
}