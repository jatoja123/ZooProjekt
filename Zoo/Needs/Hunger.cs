namespace Zoo.Needs;

public class Hunger : Need
{
    public Hunger(int initialValue) 
        : base(NeedType.HUNGER, initialValue, 100, 20){}
}
