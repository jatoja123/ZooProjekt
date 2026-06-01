namespace Zoo.Animals;
using Needs;
public abstract class Animal
{
    public bool IsInHabitat => Habitat != null;
    public LocationHabitat? Habitat = null;
    private string _name;
    public List<Need> AnimalNeeds { get; private set; }
    //private List <EnviromentalNeed> _requiredEnviroment;
    
    public Animal(string name)
    {
        _name = name;
        AnimalNeeds = [new Hunger(100), new Thirst(100), new Happiness(100)];
    }
    
    public void Update(NotifyEvent gameEvent)
    {
        // if(event == NotifyEvent.TURN_ENDED)
        // {
        //     foreach(Need need in AnimalNeeds)
        //     {
        //         need.Decrease(10);
        //     }
        // }
    }
        
    public void Feed()
    {
        Need? hunger = AnimalNeeds.FirstOrDefault(n => n.Type == NeedType.HUNGER);
        if (hunger != null)
        {
            hunger.Increase(30);
        }
    }
    public void GiveWater()
    {
        Need? thirst = AnimalNeeds.FirstOrDefault(n => n.Type == NeedType.THIRST);
        
        if (thirst != null)
        {
            thirst.Increase(25);
        }
    }
    public void Play()
    {
        Need? happiness = AnimalNeeds.FirstOrDefault(n => n.Type == NeedType.HAPPINESS);
        
        if (happiness != null)
        {
            happiness.Increase(35);
        }
    }
}
