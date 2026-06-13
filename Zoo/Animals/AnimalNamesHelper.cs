namespace Zoo.Animals;

public static class AnimalNamesHelper
{
    private static List<string> names = new(){ "Gienio", "Benio", "Zombek", "Willy", "Albert", "Olgierd", "Gaja", "Ziaja", "Bambik" };
    
    public static string RandomName()
    {
        return names[Random.Shared.Next(names.Count)];
    }
    
    private static List<string> playNames = new(){ "Berek", "Chowany", "Zaklepany", "Klasy", "Wyścigi", "Zapasy", "Przeciąganie liny" };
    
    public static string RandomPlayName()
    {
        return playNames[Random.Shared.Next(names.Count)];
    }
}