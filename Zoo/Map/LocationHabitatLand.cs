namespace Zoo;

public class LocationHabitatLand(int x, int y) : LocationHabitat(x, y)
{
    public override string Name() => "Lądowe";
    public override char Symbol() => '@';
}