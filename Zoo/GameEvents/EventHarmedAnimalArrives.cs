// using System;
// using Zoo.Animals;
// using Zoo.Needs;

// namespace Zoo.GameEvents;

// public class EventHarmedAnimalArrives : GameEvent
// {
//     public override float EventChance() => 0.2f;
//     public override GameEventType EventType => GameEventType.StartOfTurn;
//     public override EventPriority Priority => EventPriority.Normal;
    
//     private AnimalsController animalsController => GameController.Instance.AnimalsController;
    
//     public override void Trigger()
//     {
//         var rnd = new Random();
//         var animalType = AnimalsController.AnimalTypes[rnd.Next(AnimalsController.AnimalTypes.Count)];
//         var animal = (Animal?)Activator.CreateInstance(animalType, AnimalNamesHelper.RandomName());
        
//         if (animal == null) return;

//         animal.SetAge(rnd.Next(2, 10));
//         var needs = new List<NeedType>() { NeedType.HAPPINESS, NeedType.HEALTH, NeedType.HUNGER, NeedType.THIRST };
//         foreach (var need in needs)
//         {
//             int value = rnd.Next(0, 6);
//             animal.DecreaseNeed(need, value);
//         }

//         animalsController.AddAnimal(animal);
        
//         string message = $"Znaleziono zranione zwierzę: {animal.Name}";
//         GameController.Instance.ConsoleDisplay.DisplayInfo(message);
//         GameController.Instance.TriggerPopupEvent(message);
//     }
// }