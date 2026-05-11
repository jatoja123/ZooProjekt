namespace Zoo;

public class TurnEvent(bool isStartOfTurn, int turn) : NotifyEvent
{
    public bool IsStartOfTurn = isStartOfTurn;
    public int Turn = turn;
}
public class GameStartEvent () : NotifyEvent {}
public class GameEndEvent () : NotifyEvent {}