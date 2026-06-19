using System.Text.Json;

namespace Zoo.Score;

public class GameScore : IObserver
{
    private List<IScoreable> scoreables;
    private int maxScore = 0;

    public int CurrentScore { get; private set; } = 0;
    public int MaxScore => maxScore;
    public bool IsNewBestScore => CurrentScore > maxScore;

    public GameScore(List<IScoreable> scoreables)
    {
        this.scoreables = scoreables;
        LoadMaxScore();
    }
    
    private void CalculateScore()
    {
        int score = 0;
        foreach (IScoreable scoreable in scoreables) score += scoreable.CalculateScore();
        CurrentScore = score;
    }

    private void LoadMaxScore(string filePath = "score.json")
    {
        if (!File.Exists(filePath))
        {
            maxScore = 0;
            return;
        }

        string json = File.ReadAllText(filePath);
        ScoreData? data = JsonSerializer.Deserialize<ScoreData>(json);

        maxScore = data?.MaxScore ?? 0;
    }
    
    private void SaveMaxScore(string filePath = "score.json")
    {
        maxScore = Math.Max(maxScore, CurrentScore);
        ScoreData data = new ScoreData
        {
            MaxScore = CurrentScore
        };

        string json = JsonSerializer.Serialize(data);
        File.WriteAllText(filePath, json);
    }

    private class ScoreData
    {
        public int MaxScore { get; set; }
    }

    public void ReceiveEvent(NotifyEvent notifyEvent)
    {
        if(notifyEvent is GameEndEvent) SaveMaxScore();
        else if(notifyEvent is TurnEvent { IsStartOfTurn: false })
        {
            CalculateScore();
        }
    }
}