using System;
using Zoo.Animals;

namespace Zoo.GameEvents;

public class EventNewAnimalArrives : GameEvent
{
    public override float EventChance() => 1f;
    public override GameEventType EventType => GameEventType.StartOfTurn;
    public override EventPriority Priority => EventPriority.Normal;
    
    private AnimalsController animalsController => GameController.Instance.AnimalsController;
    
    public override void Trigger()
    {
        var rnd = new Random();
        var animalType = AnimalsController.AnimalTypes[rnd.Next(AnimalsController.AnimalTypes.Count)];
        
        var animal = (Animal?)Activator.CreateInstance(animalType, animalType.Name);
        
        if (animal == null) return;
        
        animalsController.AddAnimal(animal);
        
        string message = $"Nowe zwierze w ZOO: {animal.GetType().Name}";
        GameController.Instance.GameDisplay.DisplayInfo(message);
        GameController.Instance.TriggerPopupEvent(message);
    }
}