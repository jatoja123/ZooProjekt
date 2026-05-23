using Zoo.Animals;

namespace Zoo.GameEvents;

public class EventAnimalsNotInHabitatDie : GameEvent
{
    public override float EventChance() => 1;
    public override GameEventType EventType => GameEventType.EndOfTurn;
    
    private AnimalsController animalsController => GameController.Instance.AnimalsController;
    
    public override void Trigger()
    {
        var animalsToDie = animalsController.Animals.Where(x => !x.IsInHabitat).ToList();
        foreach (var animal in animalsToDie)
        {
            GameController.Instance.GameDisplay.DisplayInfo($"Zwierze umiera (nie było na żadnym wybiegu): {animal.GetType().Name}");
            animalsController.RemoveAnimal(animal);
        }
    }
}