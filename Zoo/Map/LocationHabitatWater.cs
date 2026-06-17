using Zoo.Environment;

namespace Zoo;

public class LocationHabitatWater(int x, int y) : LocationHabitat(x, y)
{
    public override CageTypeEnum HabitatType => CageTypeEnum.Water;
    public override string Name() => "Morskie";
    public override char Symbol() => '~';
}