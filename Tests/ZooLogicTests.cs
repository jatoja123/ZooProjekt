using System.Collections.Generic;
using System.Linq;
using Zoo;
using Zoo.Animals;
using Zoo.Economy;
using Zoo.Needs;
using Xunit;

namespace Zoo.Tests;

public class ZooLogicTests
{
    private class CustomNeed : Need
    {
        public CustomNeed(int value, int maxValue, int threshold)
            : base(NeedType.HEALTH, value, maxValue, threshold, 0) { }
    }

    [Fact(DisplayName = "TC-01 Need.Decrease()")]
    public void Need_Decrease_ReducesValueByAmount()
    {
        var need = new Hunger(10, 0);

        need.Decrease(3);

        Assert.Equal(7, need.Value);
    }

    [Fact(DisplayName = "TC-02 Need.Decrease() below zero")]
    public void Need_Decrease_DoesNotDropBelowZero()
    {
        var need = new Hunger(2, 0);

        need.Decrease(5);

        Assert.Equal(0, need.Value);
    }

    [Fact(DisplayName = "TC-03 Need.IsCritical()")]
    public void Need_IsCritical_WhenValueBelowOrEqualThreshold()
    {
        var need = new CustomNeed(value: 4, maxValue: 10, threshold: 5);

        Assert.True(need.IsCritical());
    }

    [Fact(DisplayName = "TC-04 Storage.Add() - capacity limit")]
    public void Storage_Add_ReturnsFalse_WhenLimitExceeded()
    {
        var storage = new Storage();

        Assert.True(storage.Add(GoodType.FoodMeat, 5));
        Assert.False(storage.Add(GoodType.FoodMeat, 7));
    }

    [Fact(DisplayName = "TC-05 Storage.Use() - insufficient resources")]
    public void Storage_Use_ReturnsZero_WhenNotEnoughResources()
    {
        var storage = new Storage();

        var used = storage.Use(GoodType.FoodMeat, 3);

        Assert.Equal(0, used);
    }

    [Fact(DisplayName = "TC-06 LocationHabitat.AddAnimal() - different species on one habitat")]
    public void LocationHabitat_AddAnimal_ReturnsFalseForDifferentSpecies()
    {
        var habitat = new LocationHabitatLand(0, 0);
        habitat.Temperature = new List<int> { 20, 40 };

        Assert.True(habitat.AddAnimal(new Lion("Leo")));

        bool added = habitat.AddAnimal(new Monkey("Mikki"), out string failMessage);

        Assert.False(added);
        Assert.Equal("Niekompatybilny gatunek zwierzęcia do pozostałych na wybiegu", failMessage);
    }

    [Fact(DisplayName = "TC-07 LocationHabitat.AddAnimal() - max 4 animals")]
    public void LocationHabitat_AddAnimal_ReturnsFalseOnFifthAnimal()
    {
        var habitat = new LocationHabitatLand(0, 0);
        habitat.Temperature = new List<int> { 20, 40 };

        Assert.True(habitat.AddAnimal(new Lion("Leo")));
        Assert.True(habitat.AddAnimal(new Lion("Nala")));
        Assert.True(habitat.AddAnimal(new Lion("Max")));
        Assert.True(habitat.AddAnimal(new Lion("Rex")));
        Assert.False(habitat.AddAnimal(new Lion("Kari")));
    }

    [Fact(DisplayName = "TC-08 Animal.Feed() - never overfeeds")]
    public void Animal_Feed_DoesNotOverfeed()
    {
        var animal = new Lion("Simba");
        animal.DecreaseNeed(NeedType.HUNGER, 3);

        int fedAmount = animal.Feed(5);
        int hungerValue = animal.AnimalNeeds.First(n => n.Type == NeedType.HUNGER).Value;

        Assert.Equal(3, fedAmount);
        Assert.Equal(10, hungerValue);
    }
}
