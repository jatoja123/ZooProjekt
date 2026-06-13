using System.Linq;
using Zoo.Animals;

namespace Zoo.GameEvents;

public class EventAnimalsNotInHabitatDie : GameEvent
{
    public override float EventChance() => 1;
    public override GameEventType EventType => GameEventType.EndOfTurn;
    
    private AnimalsController animalsController => GameController.Instance.AnimalsController;
    
    public override void Trigger()
    {
        var animalsToDie = animalsController.FreeAnimals.ToList();
        foreach (var animal in animalsToDie)
        {
            string message = $"Zwierze umiera, nie bylo na wybiegu: {animal.Name}";
            GameController.Instance.GameDisplay.DisplayInfo(message);
            GameController.Instance.TriggerPopupEvent(message);
            animalsController.RemoveAnimal(animal);
        }
    }
}