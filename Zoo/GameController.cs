namespace Zoo;

public class GameController : IObserver
{
    private static int MaxTurns = 10;
    
    private TurnController turnController;
    private GameDisplay gameDisplay;
    private Map map;
    
    public void StartGame()
    {
        map = new Map();
        map.Initialize();
        gameDisplay = new GameDisplay();
        
        turnController = new TurnController();
        turnController.Subscribe(this);
        turnController.Start(MaxTurns);
    }
    
    public void ReceiveEvent(NotifyEvent notifyEvent)
    {
        if (notifyEvent is TurnEvent turnEvent)
        {
            if (turnEvent.IsStartOfTurn)
            {
                GetPlayerActions();
                gameDisplay.DisplayMap(map);
            }
        }
    }

    private void GetPlayerActions()
    {
        Console.WriteLine("Wybierz akcje");
    }
}