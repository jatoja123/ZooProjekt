using System.Collections.Generic;
using Zoo.Animals;
using Zoo.Commands;
using Zoo.GameEvents;

namespace Zoo;

public class GameController : IObserver
{
    public static GameController Instance { get; private set; } = null!;
    public static List<Command> PlayerActions = new();
    private static int MaxActionCost = 10;
    
    private TurnController turnController = null!;
    private GameEventsController gameEventsController = null!;
    
    private GameDisplay gameDisplay = null!;
    public GameDisplay GameDisplay => gameDisplay;
    
    private Map map = null!;
    public Map Map => map;
    
    private AnimalsController animalsController = null!;
    public AnimalsController AnimalsController => animalsController;

    public List<string> Warehouse { get; private set; } = new();

    private GameGUI gameGui = null!;

    private bool skippingTurn = false;

    public int CurrentTurn { get; private set; } = 1;
    
    public void StartGame()
    {
        Instance = this;
        
        PlayerActions = new List<Command>()
        {
            new CommandBuildHabitat(this),
            new CommandChangeEnvironment(this),
            new CommandActionList(this),
            new CommandFreeAnimals(this),
            new CommandRemoveAnimal(this),
            new CommandShowHabitat(this),
            new CommandShowWarehouse(this),
            new CommandSkipTurn(this),
        };
        
        map = new Map();
        map.Start();
        gameDisplay = new GameDisplay();
        
        gameGui = new GameGUI(this);
        gameGui.Start();
        
        gameEventsController = new GameEventsController();
        gameEventsController.Start();
        
        animalsController = new AnimalsController();    
        
        turnController = new TurnController();
        turnController.Subscribe(gameEventsController);
        turnController.Subscribe(this);
        turnController.Start();
    }
    
    public void ReceiveEvent(NotifyEvent notifyEvent)
    {
        if (notifyEvent is TurnEvent turnEvent)
        {
            if (turnEvent.IsStartOfTurn) 
            {
                HandleTurn(turnEvent.Turn);
            }
            else
            {
                foreach (var animal in animalsController.Animals)
                {
                    animal.Update(notifyEvent);
                }
            }
        }
        else if (notifyEvent is GameEndEvent)
        {
            HandleGameEnd();
        }
    }

    private void HandleTurn(int turn)
    {
        skippingTurn = false;
        CurrentTurn = turn + 1;
        gameDisplay.DisplayTitle($"Tura {CurrentTurn}");
        int actionCostUsed = 0;
        gameDisplay.DisplayMap(map);
        
        while (actionCostUsed < MaxActionCost && !skippingTurn)
        {
            var (action, args) = gameDisplay.GetPlayerAction(PlayerActions);
            var cost = action.ActionCost;
            if (actionCostUsed + cost > MaxActionCost)
            {
                gameDisplay.DisplayMessage($"Akcja jest za droga ({cost}). Zostało Ci {MaxActionCost-actionCostUsed}/{MaxActionCost} akcji");
                continue;
            }

            if(!action.Execute(args)) continue;
            actionCostUsed += cost;
        }
        
        turnController.EndTurn();
    }

    private void HandleGameEnd()
    {
        gameDisplay.DisplayTitle("Koniec gry");
    }

    public void SkipTurn()
    {
        if (turnController != null)
        {
            turnController.EndTurn();
        }
    }
}