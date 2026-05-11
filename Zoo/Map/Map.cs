namespace Zoo;

public class Map
{
    private static int width = 10;
    private static int height = 10;
    
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
}