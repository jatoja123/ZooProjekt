namespace Zoo;

public class TurnEvent(bool isStartOfTurn, int turn) : NotifyEvent
{
    public bool IsStartOfTurn = isStartOfTurn;
    public int Turn = turn;
}

public class GameStartEvent(int totalTurns) : NotifyEvent
{
    public int TotalTurns = totalTurns;
}
public class GameEndEvent () : NotifyEvent {}