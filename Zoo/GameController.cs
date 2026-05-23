using Zoo.Commands;

namespace Zoo;

public class GameController : IObserver
{
    public static GameController Instance { get; private set; }
    public static List<Command> PlayerActions = new();
    private static int MaxActionCost = 10;
    
    private TurnController turnController;
    private GameDisplay gameDisplay;
    public GameDisplay GameDisplay => gameDisplay;
    private Map map;
    public Map Map => map;
    
    public void StartGame()
    {
        Instance = this;
        
        PlayerActions = new List<Command>()
        {
            new DisplayMapCommand(this),
            new ChangeEnvironmentCommand(this),
            new CommandListCommand(this),
        };
        
        map = new Map();
        map.Initialize();
        gameDisplay = new GameDisplay();
        
        turnController = new TurnController();
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
        gameDisplay.DisplayTitle($"Tura {turn+1}");
        int actionCostUsed = 0;
        gameDisplay.DisplayMap(map);
        
        while (actionCostUsed < MaxActionCost)
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
}