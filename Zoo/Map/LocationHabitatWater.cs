namespace Zoo;

public class LocationHabitatWater(int x, int y) : LocationHabitat(x, y)
{
    public override string Name() => "Morskie";
    public override char Symbol() => '~';
}