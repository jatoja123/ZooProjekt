using System.Collections.Generic;
using System.Linq;
using Zoo.Animals;
using Zoo.Environment;
namespace Zoo;

public abstract class LocationHabitat(int x, int y) : Location(x, y)
{
    public const int MaxAnimals = 4;
    public abstract CageTypeEnum HabitatType { get; }
    public List<int> Temperature { get; set; } = new List<int> { 5, 20 };
    public override bool CanBeReplaced() => animals.Count == 0;
    public IReadOnlyList<Animal> Animals => animals;
    
    private List<Animal> animals = new();

    public bool AddAnimal(Animal newAnimal, out string failMessage)
    {
        if (animals.Count >= MaxAnimals)
        {
            failMessage = $"Zbyt duzo zwierzat na wybiegu (max {MaxAnimals})";
            return false;
        }
       
        if (animals.Contains(newAnimal))
        {
            failMessage = "To zwierze już jest na wybiegu";
            return false;
        }
        
        if (animals.Count > 0 && animals[0].GetType().Name != newAnimal.GetType().Name)
        {
            failMessage = "Niekompatybilny gatunek zwierzecia do pozostałych na wybiegu";
            return false;
        }
        
        foreach (var need in newAnimal.EnvironmentalNeeds)
        {
            if (!need.IsTemperatureSatisfied(this))
            {
                failMessage = "Nieodpowiednia temperatura wybiegu dla tego zwierzecia";
                return false;
            }
            if (!need.IsEnviromentSatisfied(this))
            {
                failMessage = "Niekompatybilny typ wybiegu dla tego zwierzecia";
                return false;
            }
        }
        
        failMessage = "";

        animals.Add(newAnimal);
        newAnimal.Habitat = this;
        return true;
    }

    public bool AddAnimal(Animal newAnimal)
    {
        return AddAnimal(newAnimal, out _);
    }

    public bool RemoveAnimal(Animal animalToRemove)
    {
        if (!animals.Contains(animalToRemove)) return false;
        
        animals.Remove(animalToRemove);
        animalToRemove.Habitat = null;
        return true;
    }

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

        if (animals.Count >= MaxAnimals || animals.Count < 2) return false;

        var candidates = animals.Where(a => a.CanReproduce()).ToList();
        if (candidates.Count < 2) return false;

        if (Random.Shared.NextDouble() > 0.30) return false;

        var type = animals[0].GetType();
        offspring = (Animal)Activator.CreateInstance(type, AnimalNamesHelper.RandomName())!;

        return true;
    }
}
