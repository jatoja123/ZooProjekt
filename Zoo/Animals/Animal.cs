namespace Zoo.Animals;

public abstract class Animal
{
    public bool IsInHabitat => Habitat != null;
    public LocationHabitat? Habitat = null;
}
