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

    // Konfiguracja sily eventow
    private const int ReputationLossSentAway = 10;
    private const int ReputationLossChild = 15;
    private const int ChildHappinessGain = 15;
    private const int DiseaseHealthLoss = 4;
    private const int FightHealthLoss = 5;
    private const int DroughtThirstLoss = 4;
    private const int BoredomHappinessLoss = 3;

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
            new()
            {
                Name = "ChorobaNaWybiegu",
                Chance = 0.15f,
                Type = GameEventType.StartOfTurn,
                Priority = EventPriority.Normal,
                Trigger = TriggerDisease
            },
            new()
            {
                Name = "BojkaZwierzat",
                Chance = 0.15f,
                Type = GameEventType.StartOfTurn,
                Priority = EventPriority.Normal,
                Trigger = TriggerFight
            },
            new()
            {
                Name = "UpalnaPogoda",
                Chance = 0.15f,
                Type = GameEventType.StartOfTurn,
                Priority = EventPriority.Normal,
                Trigger = TriggerDrought
            },
            new()
            {
                Name = "SamotneZwierzeSieNudzi",
                Chance = 0.2f,
                Type = GameEventType.StartOfTurn,
                Priority = EventPriority.Low,
                Trigger = TriggerLonelyAnimal
            },
            new()
            {
                Name = "DzieckoWpadloNaWybieg",
                Chance = 0.1f,
                Type = GameEventType.StartOfTurn,
                Priority = EventPriority.High,
                Trigger = TriggerChildInHabitat
            },

        };

        return definitions
            .Select(def => (GameEvent)new ParametrizedGameEvent(def))
            .ToList();
    }

    private static List<LocationHabitat> HabitatsWithAnimals(Func<LocationHabitat, bool>? predicate = null)
    {
        var habitats = GameController.Instance.Map.Locations
            .OfType<LocationHabitat>()
            .Where(h => h.Animals.Count > 0);
 
        if (predicate != null) habitats = habitats.Where(predicate);
 
        return habitats.ToList();
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
                GameController.Instance.GameScore.AdjustReputation(-ReputationLossSentAway);
                GameController.Instance.ConsoleDisplay.DisplayInfo($"Odeslano zwierze: {animal.Name}");
                GameController.Instance.ConsoleDisplay.DisplayInfo($" Zoo traci reputacje ({ReputationLossSentAway} pkt).");
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

    private static void TriggerDisease()
    {
        var habitats = HabitatsWithAnimals();
        if (habitats.Count == 0) return;
 
        var habitat = habitats[rnd.Next(habitats.Count)];
        foreach (var animal in habitat.Animals)
        {
            animal.DecreaseNeed(NeedType.HEALTH, DiseaseHealthLoss);
        }
 
        string message = $"Choroba nawiedzila wybieg ({habitat.X}, {habitat.Y})! Zdrowie zwierzat spadlo.";
        GameController.Instance.ConsoleDisplay.DisplayInfo(message);
        GameController.Instance.TriggerPopupEvent(message);
    }

    private static void TriggerFight()
    {
        var habitats = HabitatsWithAnimals(h => h.Animals.Count >= 2);
        if (habitats.Count == 0) return;
 
        var habitat = habitats[rnd.Next(habitats.Count)];
        var pair = habitat.Animals.OrderBy(_ => rnd.Next()).Take(2).ToList();
 
        foreach (var animal in pair)
        {
            animal.DecreaseNeed(NeedType.HEALTH, FightHealthLoss);
        }
 
        string message = $"Doszlo do bojki na wybiegu ({habitat.X}, {habitat.Y})\n miedzy {pair[0].Name} i {pair[1].Name}!";
        GameController.Instance.ConsoleDisplay.DisplayInfo(message);
        GameController.Instance.TriggerPopupEvent(message);
    }

    private static void TriggerDrought()
    {
        var habitats = HabitatsWithAnimals();
        if (habitats.Count == 0) return;
 
        var habitat = habitats[rnd.Next(habitats.Count)];
        foreach (var animal in habitat.Animals)
        {
            animal.DecreaseNeed(NeedType.THIRST, DroughtThirstLoss);
        }
 
        string message = $"Upalna pogoda wysuszyla zwierzeta na wybiegu ({habitat.X}, {habitat.Y})!";
        GameController.Instance.ConsoleDisplay.DisplayInfo(message);
        GameController.Instance.TriggerPopupEvent(message);
    }
    private static void TriggerLonelyAnimal()
    {
        var habitats = HabitatsWithAnimals(h => h.Animals.Count == 1);
        if (habitats.Count == 0) return;
 
        var habitat = habitats[rnd.Next(habitats.Count)];
        var animal = habitat.Animals[0];
        animal.DecreaseNeed(NeedType.HAPPINESS, BoredomHappinessLoss);
 
        string message = $"{animal.Name} jest samotne na wybiegu ({habitat.X}, {habitat.Y}) i bardzo sie nudzi!";
        GameController.Instance.ConsoleDisplay.DisplayInfo(message);
        GameController.Instance.TriggerPopupEvent(message);
    }

    private static void TriggerChildInHabitat()
    {
        var habitats = HabitatsWithAnimals();
        if (habitats.Count == 0) return;
 
        var habitat = habitats[rnd.Next(habitats.Count)];
 
        var decision = new PendingDecision
        {
            Message = $"Dziecko wpadlo na wybieg ({habitat.X}, {habitat.Y})!\n\nWyciagnac je natychmiast?",
            OptionYesLabel = "Wyciagnij",
            OptionNoLabel = "Zostaw",
            OnYes = () =>
            {
                GameController.Instance.ConsoleDisplay.DisplayInfo("Dziecko zostalo bezpiecznie wyciagniete z wybiegu.");
            },
            OnNo = () =>
            {
                foreach (var animal in habitat.Animals)
                {
                    animal.IncreaseNeed(NeedType.HAPPINESS, ChildHappinessGain);
                }
 
                GameController.Instance.GameScore.AdjustReputation(-ReputationLossChild);
                GameController.Instance.ConsoleDisplay.DisplayInfo($"Dziecko bawilo sie ze zwierzetami... Zoo traci reputacje ({ReputationLossChild} pkt)!");
            }
        };
 
        GameController.Instance.EnqueueDecision(decision);
    }
}