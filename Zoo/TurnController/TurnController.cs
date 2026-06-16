namespace Zoo;

public class TurnController: Observable
{
    private static int totalTurns = 5;
    private int turn = 0;
    private bool isGameRunning = false;

    public bool IsGameRunning => isGameRunning;
    public int CurrentTurn => turn;
    public int TotalTurns => totalTurns;

    public void Start()
    {
        turn = 1;
        isGameRunning = true;
        Notify(new GameStartEvent(totalTurns));
        StartTurn();
    }

    private void StartTurn()
    {
        if(!isGameRunning) return;
        Notify(new TurnEvent(isStartOfTurn: true, turn));
    }
    
    public void EndTurn()
    {
        if(!isGameRunning) return;
        Notify(new TurnEvent(isStartOfTurn: false, turn));
        turn++;
        
        if (turn >= totalTurns)
        {
            turn = 0;
            Notify(new GameEndEvent());
            isGameRunning = false;
        } else StartTurn();
    }
}