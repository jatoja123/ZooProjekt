namespace Zoo.Animals;

public class AnimalsController
{
    public static List<Type> AnimalTypes = new List<Type>()
    {
        typeof(Lion),
        typeof(Elephant),
        typeof(Emu),
        typeof(Turtle),
        typeof(Snake),
        typeof(Dolphin),
        typeof(Monkey),
        typeof(Giraffe),
        typeof(Penguin)
    };
    
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

    public Animal? GetAnimal(int x, int y, int index)
    {
        var habitat = animals.First(a => a.IsInHabitat && a.Habitat?.X == x && a.Habitat?.Y == y).Habitat;
        return habitat?.Animals[index];
    }

}
