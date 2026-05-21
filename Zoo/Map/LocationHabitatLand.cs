namespace Zoo;

public class LocationHabitatLand(int x, int y) : Location(x, y)
{
    public override string Name() => "Lądowe";
    public override char Symbol() => '@';
}