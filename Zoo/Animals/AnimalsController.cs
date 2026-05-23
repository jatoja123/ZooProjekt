namespace Zoo.Animals;

public class AnimalsController
{
    public static List<Type> AnimalTypes = new List<Type>() { typeof(Lion), typeof(Elephant) };
    
    private List<Animal> animals = new();
    public List<Animal> Animals => animals;
    public List<Animal> FreeAnimals => animals.Where(x => !x.IsInHabitat).ToList();

    public void AddAnimal(Animal animal)
    {
        animals.Add(animal);
    }
    
    public void RemoveAnimal(Animal animal)
    {
        if (!animals.Contains(animal)) return;
        animals.Remove(animal);
    }
}