using System;
using System.Linq;
using System.Collections.Generic;
using DiceGame.Logic.Scoring;

namespace DiceGame.Logic.Models;

public class GameSession
{
    public PlayerState[] Players { get; private set; }
    public GameHand Hand { get; private set; }
    public int ActivePlayerIndex { get; private set; }
    public int PlayerCount => Players.Length;

    public event Action? OnTurnAdvanced;
    public event Action? OnGameOver;

    public GameSession(int playerCount)
    {
        Players = new PlayerState[playerCount];
        for (int i = 0; i < playerCount; i++)
            Players[i] = new PlayerState();

        Hand = new GameHand();
        ActivePlayerIndex = 0;
    }

    public void LockScore(int categoryIndex)
    {
        int score = ScoreCalculator.Calculate(categoryIndex, Hand.Dice);
        Players[ActivePlayerIndex].LockScore(categoryIndex, score);
        
        Hand.Reset();
        AdvanceTurn();
    }

    private void AdvanceTurn()
    {
        ActivePlayerIndex++;
        if (ActivePlayerIndex >= PlayerCount)
        {
            ActivePlayerIndex = 0;
        }

        OnTurnAdvanced?.Invoke();

        if (Players.All(p => !p.HasEmptyCategories()))
        {
            OnGameOver?.Invoke();
        }
    }

    public List<int> GetWinners(out int winningScore)
    {
        winningScore = -1;
        var winners = new List<int>();

        for (int i = 0; i < Players.Length; i++)
        {
            int score = Players[i].Scores[ScoreCategory.GrandTotal].GetValueOrDefault();
            if (score > winningScore)
            {
                winningScore = score;
                winners.Clear();
                winners.Add(i);
            }
            else if (score == winningScore)
            {
                winners.Add(i);
            }
        }

        return winners;
    }
}
