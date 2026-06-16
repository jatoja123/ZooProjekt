using System.Collections.Generic;
using Zoo.Animals;
using Zoo.Commands;
using Zoo.Commands.Animals;
using Zoo.GameEvents;
using Zoo.Economy;

namespace Zoo;

public class GameController : IObserver
{
    public static GameController Instance { get; private set; } = null!;
    public static List<Command> PlayerActions = new();

    public static List<Command> MainActions = new();
    public static List<Command> ShopActions = new();
    public static List<Command> AnimalActions = new();
    public static List<Command> MapActions = new();
    
    private static int maxActionCost = 10;

    private TurnController turnController = null!;
    private GameEventsController gameEventsController = null!;

    private ConsoleDisplay consoleDisplay = null!;
    public ConsoleDisplay ConsoleDisplay => consoleDisplay;

    private Map map = null!;
    public Map Map => map;

    private AnimalsController animalsController = null!;
    public AnimalsController AnimalsController => animalsController;

    private MoneyController moneyController = null!;
    public MoneyController MoneyController => moneyController;

    private Storage storage = null!;
    public Storage Storage => storage;

    private GameGUI gameGui = null!;

    private bool skippingTurn = false;

    public int ActionCostUsed { get; private set; } = 0;
    public int MaxActionCost => maxActionCost;
    public int ActionsLeft => MaxActionCost - ActionCostUsed;

    public void StartGame()
    {
        Instance = this;

        MainActions = new List<Command>()
        {
            new CommandActionList(this),
            new CommandFreeAnimals(this),
            new CommandShowWarehouse(this),
            new CommandSkipTurn(this),
            new CommandOpenShop(this)
        };

        ShopActions = new List<Command>()
        {
            new CommandActionList(this),
            new CommandBuyFood(this),
            new CommandBuyWater(this),
            new CommandBuyMedicine(this),
            new CommandBuyAnimal(this),
            new CommandExpandStorage(this),
            new CommandExitShop(this)
        };

        AnimalActions = new List<Command>()
        {
            new CommandAnimalFeed(this),
            new CommandAnimalDrink(this),
            new CommandAnimalPlay(this),
        };
        
        MapActions = new List<Command>()
        {
            new CommandBuildHabitat(this),
            new CommandShowHabitat(this),
            new CommandRemoveAnimal(this),
            new CommandFreeAnimals(this),
        };

        PlayerActions = MainActions;

        map = new Map();
        map.Start();
        consoleDisplay = new ConsoleDisplay();

        gameGui = new GameGUI(this);
        gameGui.Start();

        gameEventsController = new GameEventsController();
        gameEventsController.Start();

        animalsController = new AnimalsController();

        moneyController = new MoneyController();

        storage = new Storage();

        turnController = new TurnController();
        turnController.Subscribe(this);
        turnController.Subscribe(gameEventsController);
        turnController.Subscribe(gameGui);
        turnController.Start();
    }

    public void ReceiveEvent(NotifyEvent notifyEvent)
    {
        if (notifyEvent is TurnEvent turnEvent)
        {
            if (turnEvent.IsStartOfTurn)
            {
                consoleDisplay.DisplayTitle($"Tura {turnEvent.Turn}");
                consoleDisplay.DisplayMap(map);

                moneyController.CalculateIncome(animalsController);
                consoleDisplay.DisplayInfo($"Stan konta: {moneyController.Money}$");
            }
            else
            {
                foreach (var animal in animalsController.Animals)
                {
                    animal.Update(notifyEvent);
                }
            }
        }
        else if (notifyEvent is GameStartEvent)
        {
            RunConsoleLoop();
        }
        else if (notifyEvent is GameEndEvent)
        {
            HandleGameEnd();
        }
    }

    public bool CanExecuteAction(int actionCost)
    {
        if (ActionCostUsed + actionCost > maxActionCost)
        {
            consoleDisplay.DisplayMessage($"Akcja jest za droga ({actionCost}). Zostało Ci {ActionsLeft}/{maxActionCost} akcji");
            return false;
        }
        
        return true;
    }

    public void UseActionPoints(int actionCost)
    {
        ActionCostUsed += actionCost;
    }
    
    private void HandleGameEnd()
    {
        consoleDisplay.DisplayTitle("Koniec gry");
        gameGui.AddPopup("Koniec Gry!");
    }

    public void TriggerPopupEvent(string message)
    {
        gameGui.AddPopup(message);
    }

    public void SkipTurn()
    {
        turnController.EndTurn();
    }
    
    private void RunConsoleLoop()
    {
        while (turnController.IsGameRunning)
        {
            var (command, args) = consoleDisplay.GetPlayerAction(PlayerActions);
            command.ExecuteCommand(args);
        }
    }
}