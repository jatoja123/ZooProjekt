using Zoo.Animals;

namespace Zoo;

public abstract class LocationHabitat(int x, int y) : Location(x, y)
{
    public override bool CanBeReplaced() => !isOccupied;
    public Animal? Animal => animal;
    
    private Animal? animal = null;
    private bool isOccupied = false;

    public bool AddAnimal(Animal newAnimal)
    {
        if (isOccupied) return false;
        isOccupied = true;
        animal = newAnimal;
        animal.Habitat = this;
        return true;
    }

    public Animal? RemoveAnimal()
    {
        if (!isOccupied) return null;
        isOccupied = false;
        var currentAnimal = animal;
        currentAnimal.Habitat = null;
        animal = null;
        return currentAnimal;
    }
}