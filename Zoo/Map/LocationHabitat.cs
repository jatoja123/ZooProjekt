using System.Collections.Generic;
using System.Linq;
using Zoo.Animals;
using Zoo.Environment;
namespace Zoo;

public abstract class LocationHabitat(int x, int y) : Location(x, y)
{
    public abstract CageTypeEnum HabitatType { get; }
    public List<int> Temperature { get; set; } = new List<int> { -20, 20 };
    public override bool CanBeReplaced() => animals.Count == 0;
    public IReadOnlyList<Animal> Animals => animals;
    
    private List<Animal> animals = new();

    public bool AddAnimal(Animal newAnimal)
    {
        if (animals.Count >= 4) return false;
        if (animals.Contains(newAnimal)) return false;
        
        if (animals.Count > 0 && animals[0].GetType().Name != newAnimal.GetType().Name)
        {
            return false;
        }
        
        foreach (var need in newAnimal.EnvironmentalNeeds)
        {
            if (!need.IsSatisfied(this))
            {
                return false;
            }
        }

        animals.Add(newAnimal);
        newAnimal.Habitat = this;
        return true;
    }

    public bool RemoveAnimal(Animal animalToRemove)
    {
        if (!animals.Contains(animalToRemove)) return false;
        
        animals.Remove(animalToRemove);
        animalToRemove.Habitat = null;
        return true;
    }

    /// <summary>
    /// Stan lokacji - liczba [0,1]
    /// </summary>
    /// <returns>liczba [0,1]</returns>
    public float LocationCondition()
    {
        if (animals.Count == 0) return 1;
        
        float animalConditions = 0;
        foreach (var animal in animals)
        {
            animalConditions += animal.GetCondition();
        }
        
        return animalConditions / animals.Count;
    }

     public bool TryReproduce(out Animal? offspring)
    {
        offspring = null;

        if (animals.Count >= 4 || animals.Count < 2) return false;

        var candidates = animals.Where(a => a.CanReproduce()).ToList();
        if (candidates.Count < 2) return false;

        if (Random.Shared.NextDouble() > 0.30) return false;

        var type = animals[0].GetType();
        offspring = (Animal)Activator.CreateInstance(type, $"Baby_{Random.Shared.Next(1000, 9999)}")!;

        return true;
    }
}
