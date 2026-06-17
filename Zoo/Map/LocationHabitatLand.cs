using Zoo.Environment;

namespace Zoo;

public class LocationHabitatLand(int x, int y) : LocationHabitat(x, y)
{
    public override CageTypeEnum HabitatType => CageTypeEnum.Land;
    public override string Name() => "Lądowe";
    public override char Symbol() => '@';
}