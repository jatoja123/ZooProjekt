namespace Zoo.Needs;

public class Happiness : Need
{
   public Happiness(int initialValue) 
        : base(NeedType.HAPPINESS, initialValue, initialValue, initialValue/5){}

}
