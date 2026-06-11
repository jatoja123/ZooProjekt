namespace Zoo;

public class TurnController: Observable
{
    private static int MaxGameTurns = 10;
    private int turn = 0;
    private int maxTurns = 0;
    private bool isGameRunning = false;

    public static int CurrentTurn { get; private set; } = 0;
    public static int TotalTurns { get; private set; } = 10;

    public void Start()
    {
        turn = 0;
        maxTurns = MaxGameTurns;
        CurrentTurn = turn;
        TotalTurns = maxTurns;
        isGameRunning = true;
        Notify(new GameStartEvent());
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
        CurrentTurn = turn;
        if (turn == maxTurns)
        {
            turn = 0;
            CurrentTurn = turn;
            Notify(new GameEndEvent());
            isGameRunning = false;
        } else StartTurn();
    }
}