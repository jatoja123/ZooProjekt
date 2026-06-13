namespace Zoo.Animals;

public static class AnimalNamesHelper
{
    private static List<string> names = new(){ "Gienio", "Benio", "Zombek", "Willy", "Albert", "Olgierd", "Gaja", "Ziaja", "Bambik" };
    
    public static string RandomName()
    {
        return names[Random.Shared.Next(names.Count)];
    }
}