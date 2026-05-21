namespace Zoo;

public abstract class Location(int x, int y)
{
    public int X = x;
    public int Y = y;
    public abstract string Name();
    public abstract char Symbol();
    public virtual bool CanBeReplaced() => true;
}