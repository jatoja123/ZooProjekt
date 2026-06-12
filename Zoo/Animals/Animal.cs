using System.Collections.Generic;
using System.Linq;
using Zoo.Needs;

namespace Zoo.Animals;

public abstract class Animal
{
    public char foodType {get; private set;} // M - meat, P - plant, B - both 
    public uint age {get; private set;}
    public bool IsInHabitat => Habitat != null;
    public LocationHabitat? Habitat = null;
    private string _name;
    public List<Need> AnimalNeeds { get; private set; }
    
    public Animal(string name, char food)
    {
        _name = name;
        foodType = food;
        AnimalNeeds = [new Hunger(100), new Thirst(100), new Happiness(100), new Health(100)];
        age = 1;
    }
    
    public void Update(NotifyEvent gameEvent)
    {
        if (gameEvent is TurnEvent turnEvent && !turnEvent.IsStartOfTurn)
        {
            foreach(Need need in AnimalNeeds)
            {
                need.Decrease(10);
            }
        }
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

    public void Heal()
    {
        Need? health = AnimalNeeds.FirstOrDefault(n => n.Type == NeedType.HEALTH);
        if (health != null)
        {
            health.Increase(35);
        }
    }
}