namespace Zoo.GameEvents;

public class PendingDecision
{
    public required string Message { get; init; }
    public required string OptionYesLabel { get; init; }
    public required string OptionNoLabel { get; init; }
    public required Action OnYes { get; init; }
    public required Action OnNo { get; init; }
}