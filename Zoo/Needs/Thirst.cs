namespace Zoo.Needs;

public class Thirst : Need
{
    public Thirst(int initialValue, int passiveDecrease) 
        : base(NeedType.THIRST, initialValue, initialValue, initialValue/4, passiveDecrease){}
}
