namespace Zoo.Needs;

public class Health : Need
{
    public Health(int initialValue) 
        : base(NeedType.HEALTH, initialValue, initialValue, initialValue/5){}
}
