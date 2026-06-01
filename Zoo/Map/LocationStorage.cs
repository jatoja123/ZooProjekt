namespace Zoo;

public class LocationStorage(int x, int y) : Location(x, y)
{
    public override string Name() => "Magazyn";
    public override char Symbol() => 'M';
}