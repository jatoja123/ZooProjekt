using System.Collections.Generic;
using Zoo.Animals;
using Zoo.Commands;
using Zoo.Commands.Animals;
using Zoo.GameEvents;
using Zoo.Economy;
using Zoo.Score;

namespace Zoo;

public class GameController : IObserver
{
    public static GameController Instance { get; private set; } = null!;
    public static bool RunInConsole = false;
    public static List<Command> PlayerActions = new(); // obecne akcje dostępne dla gracza

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
    
    private GameScore gameScore = null!;
    public GameScore GameScore => gameScore;

    private Storage storage = null!;
    public Storage Storage => storage;

    private GameGUI gameGui = null!;

    public int ActionCostUsed { get; private set; } = 0;
    public int MaxActionCost => maxActionCost;
    public int ActionsLeft => MaxActionCost - ActionCostUsed;

    public async Task StartGame()
    {
        Instance = this;

        MainActions = new List<Command>()
        {
            new CommandActionList(this),
            new CommandFreeAnimals(this),
            new CommandShowWarehouse(this),
            new CommandSkipTurn(this),
            new CommandOpenShop(this),
            new CommandDisplayMap(this, CommandVisibility.Console),
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
            new CommandAnimalHeal(this),
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

        gameEventsController = new GameEventsController();
        gameEventsController.Start();

        animalsController = new AnimalsController();

        moneyController = new MoneyController();

        storage = new Storage();

        gameScore = new GameScore(new() { map, storage, moneyController });

        turnController = new TurnController();
        turnController.Subscribe(gameScore);
        turnController.Subscribe(gameGui);
        turnController.Subscribe(this);
        turnController.Subscribe(gameEventsController);
        turnController.Start();
        
        if(RunInConsole) await Task.Run(RunConsoleLoop);
        else await gameGui.Start();
    }

    public void ReceiveEvent(NotifyEvent notifyEvent)
    {
        if (notifyEvent is TurnEvent turnEvent)
        {
            if (turnEvent.IsStartOfTurn)
            {
                consoleDisplay.DisplayTitle($"Tura {turnEvent.Turn}");
                consoleDisplay.DisplayMap(map);

                ActionCostUsed = 0;

                moneyController.CalculateIncome(animalsController);
                consoleDisplay.DisplayMessage($"Stan konta: {moneyController.Money}$");
                consoleDisplay.DisplayMessage($"Reputacja: {gameScore.CurrentScore} pkt | Najlepsza reputacja: {gameScore.MaxScore} pkt");
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
            if (RunInConsole)
            {
                MainActions.AddRange(MapActions);
                MainActions.AddRange(AnimalActions);
                PlayerActions = MainActions;
            }
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