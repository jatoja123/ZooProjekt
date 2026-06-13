using System.Collections.Generic;
using System.Linq;
using Zoo.Animals;

namespace Zoo;

public abstract class LocationHabitat(int x, int y) : Location(x, y)
{
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
}