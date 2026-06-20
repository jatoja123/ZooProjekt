using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.Animals;
using Zoo.Needs;

namespace Zoo.GameEvents;

public static class GameEventCatalog
{
    private static AnimalsController animalsController => GameController.Instance.AnimalsController;
    private static Random rnd => Random.Shared;

    public static List<GameEvent> CreateAll()
    {
        var definitions = new List<GameEventDefinition>
        {
            new()
            {
                Name = "ZranioneZwierzePrzybywa",
                Chance = 0.2f,
                Type = GameEventType.StartOfTurn,
                Priority = EventPriority.Normal,
                DecisionFactory = CreateHarmedAnimalDecision
            },
            new()
            {
                Name = "ZwierzetaBezWybieguUmieraja",
                Chance = 1f,
                Type = GameEventType.EndOfTurn,
                Priority = EventPriority.Normal,
                Trigger = TriggerAnimalsNotInHabitatDie
            },

        };

        return definitions
            .Select(def => (GameEvent)new ParametrizedGameEvent(def))
            .ToList();
    }

    private static PendingDecision CreateHarmedAnimalDecision()
    {
        var animalType = AnimalsController.AnimalTypes[rnd.Next(AnimalsController.AnimalTypes.Count)];
        var animal = (Animal)Activator.CreateInstance(animalType, AnimalNamesHelper.RandomName())!;

        animal.SetAge(rnd.Next(2, 10));
        var needs = new List<NeedType> { NeedType.HAPPINESS, NeedType.HEALTH, NeedType.HUNGER, NeedType.THIRST };
        foreach (var need in needs)
        {
            animal.DecreaseNeed(need, rnd.Next(0, 6));
        }

        return new PendingDecision
        {
            Message = $"Znaleziono zranione zwierze: {animal.Name}.\n\n Przyjac je do zoo?",
            OptionYesLabel = "Przyjmij",
            OptionNoLabel = "Odeslij",
            OnYes = () =>
            {
                animalsController.AddAnimal(animal);
                GameController.Instance.ConsoleDisplay.DisplayInfo($"Przyjeto zwierze: {animal.Name}");
            },
            OnNo = () =>
            {
                GameController.Instance.ConsoleDisplay.DisplayInfo($"Odeslano zwierze: {animal.Name}");
            }
        };
    }

    private static void TriggerAnimalsNotInHabitatDie()
    {
        var animalsToDie = animalsController.FreeAnimals.ToList();
        foreach (var animal in animalsToDie)
        {
            string message = $"Zwierze umiera,\n nie bylo na wybiegu: {animal.Name}";
            GameController.Instance.ConsoleDisplay.DisplayInfo(message);
            GameController.Instance.TriggerPopupEvent(message);
            animalsController.RemoveAnimal(animal);
        }
    }
}