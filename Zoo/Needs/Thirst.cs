namespace Zoo.Needs;

public class Thirst : Need
{
    public Thirst(int initialValue) 
        : base(NeedType.THIRST, initialValue, 100, 40){}
}
