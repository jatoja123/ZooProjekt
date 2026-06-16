namespace Zoo.Animals;

public static class AnimalNamesHelper
{
    private static List<string> names = new(){ "Bobert", "Benio", "Zombek", "Wilky", "Albert", "Olgierd", "Gaja", "Amadeusz", "Ziaja", "Bambik", "Topinambur", "Baby Yoda", "Maximus" };
    
    public static string RandomName()
    {
        return names[Random.Shared.Next(names.Count)];
    }
    
    private static List<string> playNames = new(){ "Berek", "Chowany", "Zaklepany", "Klasy", "Wyścigi", "Zapasy", "Przeciąganie liny", "Policjanci i złodzieje", "Warcaby"};
    
    public static string RandomPlayName()
    {
        return playNames[Random.Shared.Next(names.Count)];
    }
}