using System.Collections.Generic;
using System.Linq;
using Zoo.Economy;
using Zoo.Needs;

namespace Zoo.Animals;

public abstract class Animal
{
    public GoodType foodType {get; private set;} // M - meat, P - plant, B - both 
    public uint age {get; private set;}
    public bool IsInHabitat => Habitat != null;
    public LocationHabitat? Habitat = null;
    
    private string _name;
    public string Name => $"{_name} - {GetType().Name} ({age})";
    
    public List<Need> AnimalNeeds { get; private set; }
    
    public Animal(string name, GoodType food)
    {
        _name = name;
        foodType = food;
        AnimalNeeds = [new Hunger(10, 1), new Thirst(10, 1), new Happiness(10), new Health(10)];
        age = 1;
    }
    
    public void Update(NotifyEvent gameEvent)
    {
        if (gameEvent is TurnEvent turnEvent && !turnEvent.IsStartOfTurn)
        {
            foreach(Need need in AnimalNeeds)
            {
                need.Decrease(need.PassiveDecrease);
            }
        }
    }
    
    /// <summary>
    /// Zwraca ile REALNIE nakarmiono (żeby nie przekarmiać)
    /// </summary>
    /// <param name="foodCount"></param>
    /// <returns></returns>
    public int Feed(int foodCount)
    {
        Need? hunger = AnimalNeeds.FirstOrDefault(n => n.Type == NeedType.HUNGER);
        if (hunger != null)
        {
            foodCount = Math.Min(foodCount, hunger.Missing);
            hunger.Increase(foodCount);
            return foodCount;
        }

        return 0;
    }

    /// <summary>
    /// Zwraca ile REALNIE dano wody (żeby nie utopić)
    /// </summary>
    /// <param name="waterAmount"></param>
    /// <returns></returns>
    public int GiveWater(int waterAmount)
    {
        Need? thirst = AnimalNeeds.FirstOrDefault(n => n.Type == NeedType.THIRST);
        if (thirst != null)
        {
            waterAmount = Math.Min(waterAmount, thirst.Missing);
            thirst.Increase(waterAmount);
            return waterAmount;
        }

        return 0;
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
    public int GetCondition(Animal animal)
    {
        return animal.AnimalNeeds.Min(n => n.Value);
    }
}