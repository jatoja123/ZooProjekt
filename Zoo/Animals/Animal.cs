namespace Zoo.Animals;

public abstract class Animal
{
    public bool IsInHabitat => Habitat != null;
    public LocationHabitat? Habitat = null;
    private string _name;
    //private List <Need> _animalNeeds;
    //private List <EnviromentalNeed> _requiredEnviroment;
    
    public Animal(string name)
    {
        _name = name;
    }
    
    public void Update(NotifyEvent gameEvent)
    {
        // if(event == NotifyEvent.TURN_ENDED)
        // {
        //     foreach(Need need in _animalNeeds)
        //     {
        //         need.Decrease(10);
        //     }
        // }
    }
        
    public void Feed()
    {
        //Need hunger = _animalNeeds.FirstOrDefault(n => n.Type == NeedType.HUNGER);
        // if (hunger != null)
        // {
        //     hunger.Increase(30);
        // }
    }
    public void GiveWater()
    {
        // Need thirst = _animalNeeds.FirstOrDefault(n => n.Type == NeedType.THIRST);
        //
        // if (thirst != null)
        // {
        //     thirst.Increase(25);
        // }
    }
    public void Play()
    {
        // Need happiness = _animalNeeds.FirstOrDefault(n => n.Type == NeedType.HAPPINESS);
        //
        // if (happiness != null)
        // {
        //     happiness.Increase(35);
        // }
    }
}
