namespace Zoo;

public class LocationEmpty(int x, int y) : Location(x, y)
{
    public override string Name() => "Puste";
    public override char Symbol() => '_';
}