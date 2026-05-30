namespace Zoo;

public class Map
{
    private static int width = 5;
    private static int height = 5;
    private static Dictionary<string, Type> locationTypes = new()
    {
        { "ladowe", typeof(LocationHabitatLand) },
        { "wodne", typeof(LocationHabitatWater) },
    };
    
    public int Width => width;
    public int Height => height;
    public List<Location> Locations { get; private set; } = new ();
    
    public void Start()
    {
        for (int h = 0; h < height * width; h++)
        {
            Location location = new LocationEmpty(h % width, h / width);
            if(h == height * width / 2)
            {
                location = new LocationStorage(h % width, h / width);
            }
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

    public Location? GetLocation(int x, int y) => Locations.FirstOrDefault(l => l?.X == x && l?.Y == y, null);
}