using Zoo.Animals;
using Zoo.Commands;
using Zoo.GameEvents;

namespace Zoo;

public class GameController : IObserver
{
    public static GameController Instance { get; private set; }
    public static List<Command> PlayerActions = new();
    private static int MaxActionCost = 10;
    
    private TurnController turnController;
    private GameEventsController gameEventsController;
    
    private GameDisplay gameDisplay;
    public GameDisplay GameDisplay => gameDisplay;
    
    private Map map;
    public Map Map => map;
    
    private AnimalsController animalsController;
    public AnimalsController AnimalsController => animalsController;

    private bool skippingTurn = false;
    
    public void StartGame()
    {
        Instance = this;
        
        PlayerActions = new List<Command>()
        {
            new CommandDisplayMap(this),
            new CommandChangeEnvironment(this),
            new CommandActionList(this),
            new CommandFreeAnimals(this),
            new CommandRemoveAnimal(this),
            new CommandSkipTurn(this),
        };
        
        map = new Map();
        map.Start();
        gameDisplay = new GameDisplay();
        
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
            if (turnEvent.IsStartOfTurn) HandleTurn(turnEvent.Turn);
        }
        else if (notifyEvent is GameEndEvent)
        {
            HandleGameEnd();
        }
    }

    private void HandleTurn(int turn)
    {
        skippingTurn = false;
        gameDisplay.DisplayTitle($"Tura {turn+1}");
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
        skippingTurn = true;
    }
}