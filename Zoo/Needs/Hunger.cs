namespace Zoo.Needs;

public class Hunger : Need
{
    public Hunger(int initialValue, int passiveDecrease) 
        : base(NeedType.HUNGER, initialValue, initialValue, initialValue/2, passiveDecrease){}
}
