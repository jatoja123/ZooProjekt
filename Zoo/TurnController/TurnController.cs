namespace Zoo;

public class TurnController: Observable
{
    private int turn = 0;
    private int maxTurns = 0;
    private bool isGameRunning = false;

    public void Start(int newMaxTurns)
    {
        turn = 0;
        maxTurns = newMaxTurns;
        isGameRunning = true;
        Notify(new GameStartEvent());
        StartTurn();
    }

    private void StartTurn()
    {
        if(!isGameRunning) return;
        Notify(new TurnEvent(isStartOfTurn: true, turn));
    }
    
    private void EndTurn()
    {
        if(!isGameRunning) return;
        Notify(new TurnEvent(isStartOfTurn: false, turn));
        turn++;
        if (turn == maxTurns)
        {
            turn = 0;
            Notify(new GameEndEvent());
            isGameRunning = false;
        }
    }
}