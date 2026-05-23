namespace Zoo;

public class LocationHospital(int x, int y) : LocationHabitat(x, y)
{
    public override string Name() => "Szpital";
    public override char Symbol() => '%';
    public override bool CanBeReplaced() => false;
}