using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.Economy;
using Zoo.Environment;
using Zoo.Needs;

namespace Zoo.Animals;

public abstract class Animal
{
    public GoodType foodType {get; private set;} // M - meat, P - plant, B - both 
    public int age {get; private set;}
    public AgeRatio AgeRatio { get; private set; } 
    public int maxAge = 10;
    public bool IsDead { get; private set; } = false;
    public bool IsInHabitat => Habitat != null;
    public LocationHabitat? Habitat = null;
    private string _name;
    public string Name => $"{_name} - {GetType().Name} ({age})";
    
    public List<Need> AnimalNeeds { get; private set; }
    public List<EnvironmentalNeed> EnvironmentalNeeds { get; protected set; } = new();
    
    public Animal(string name, GoodType food, AgeRatio ageRatio = AgeRatio.NORMAL)
    {
        _name = name;
        foodType = food;
        AgeRatio = ageRatio;
        AnimalNeeds = [new Hunger(10, 1), new Thirst(10, 1), new Happiness(10), new Health(10)];
        age = 1;
    }
    
    public void Update(NotifyEvent gameEvent)
    {
        if (IsDead) return;

        if (gameEvent is TurnEvent turnEvent && !turnEvent.IsStartOfTurn)
        {
            foreach(Need need in AnimalNeeds)
            {
                int decreaseAmount = need.PassiveDecrease;
                if (this is IWaterAnimal)
                {
                    if (need.Type == NeedType.THIRST)
                    {
                        decreaseAmount = 0;
                    }
                    else
                    {
                        decreaseAmount += 1;
                    }
                }
                need.Decrease(decreaseAmount);
            }
            
            age += AnimalAgeRatio.AgingFactor[AgeRatio];
            
            if (CzyUmiera())
            {
                IsDead = true;
                return;
            }
        }
    }

    public bool CzyUmiera()
    {
        if (age >= maxAge)
        {
            return true;
        }

        int progStarosci = maxAge / 2;
        if (age < progStarosci)
        {
            return false;
        }

        double postepStarosci = (double)(age - progStarosci) / (maxAge - progStarosci);
        double szansaNaSmierc = postepStarosci * 0.40; 

        return Random.Shared.NextDouble() < szansaNaSmierc;
    }
    
    /// <summary>
    /// Zwraca ile REALNIE nakarmiono (żeby nie przekarmiać)
    /// </summary>
    /// <param name="foodCount"></param>
    /// <returns></returns>
    public int Feed(int foodCount)
    {
        if (IsDead) return 0;
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
        if (IsDead) return 0;
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
        if (IsDead) return;
        Need? happiness = AnimalNeeds.FirstOrDefault(n => n.Type == NeedType.HAPPINESS);
        if (happiness != null)
        {
            happiness.Increase(35);
        }
    }

    public void Heal()
    {
        if (IsDead) return;
        Need? health = AnimalNeeds.FirstOrDefault(n => n.Type == NeedType.HEALTH);
        if (health != null)
        {
            health.Increase(35);
        }
    }

    public int GetCondition()
    {
        if (IsDead) return 0;
        return AnimalNeeds.Min(n => n.Value);
    }

        /// <summary>
    /// Sprawdza, czy zwierzę spełnia warunki do rozmnażania.
    /// Domyślnie: wiek powyżej 2, zdrowie i szczęście powyżej 7.
    /// </summary>
    public virtual bool CanReproduce()
    {
        if (IsDead) return false;

        // Podstawowe warunki wiekowe
        if (age < 2 || age >= maxAge) return false;

        // Rozmnażanie wymaga dobrego stanu zdrowia i zadowolenia (powyżej 7)
        return (AnimalNeeds[2].Value > 7) && (AnimalNeeds[3].Value > 7);
    }
}