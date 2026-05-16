using Xunit;
using DiceGame.Logic.Models;
using DiceGame.Logic.Scoring;

namespace DiceGame.Tests;

public class ScoringTests
{
    private Die[] CreateDice(params int[] values) => 
        values.Select(v => new Die(v)).ToArray();

    [Theory]
    [InlineData(new[] { 1, 1, 2, 3, 4 }, ScoreCategory.Ones, 2)]
    [InlineData(new[] { 2, 2, 2, 1, 3 }, ScoreCategory.Twos, 6)]
    [InlineData(new[] { 3, 3, 1, 2, 4 }, ScoreCategory.Threes, 6)]
    [InlineData(new[] { 4, 4, 4, 4, 1 }, ScoreCategory.Fours, 16)]
    [InlineData(new[] { 1, 5, 5, 5, 5 }, ScoreCategory.Fives, 20)]
    [InlineData(new[] { 1, 2, 3, 4, 6 }, ScoreCategory.Sixes, 6)]
    // Examples from Wikipedia
    [InlineData(new[] { 1, 1, 1, 3, 5 }, ScoreCategory.Ones, 3)]
    [InlineData(new[] { 2, 2, 2, 5, 6 }, ScoreCategory.Twos, 6)]
    [InlineData(new[] { 3, 3, 3, 3, 4 }, ScoreCategory.Threes, 12)]
    [InlineData(new[] { 4, 4, 5, 5, 5 }, ScoreCategory.Fours, 8)]
    [InlineData(new[] { 1, 1, 2, 2, 5 }, ScoreCategory.Fives, 5)]
    [InlineData(new[] { 2, 3, 6, 6, 6 }, ScoreCategory.Sixes, 18)]
    public void Calculate_BasicCategories_ReturnsCorrectSum(int[] values, int category, int expected)
    {
        var dice = CreateDice(values);
        int result = ScoreCalculator.Calculate(category, dice);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Calculate_ThreeOfAKind_ValidHand_ReturnsSum()
    {
        var dice = CreateDice(3, 3, 3, 1, 2);
        int result = ScoreCalculator.Calculate(ScoreCategory.ThreeOfAKind, dice);
        Assert.Equal(12, result);
    }

    [Fact]
    public void Calculate_ThreeOfAKind_InvalidHand_ReturnsZero()
    {
        var dice = CreateDice(3, 3, 1, 2, 4);
        int result = ScoreCalculator.Calculate(ScoreCategory.ThreeOfAKind, dice);
        Assert.Equal(0, result);
    }

    // Example from Wikipedia
    [Fact]
    public void Calculate_ThreeOfAKind_WikipediaExample_Returns17()
    {
        var dice = CreateDice(2, 3, 4, 4, 4);
        int result = ScoreCalculator.Calculate(ScoreCategory.ThreeOfAKind, dice);
        Assert.Equal(17, result);
    }

    [Fact]
    public void Calculate_FourOfAKind_ValidHand_ReturnsSum()
    {
        var dice = CreateDice(4, 4, 4, 4, 2);
        int result = ScoreCalculator.Calculate(ScoreCategory.FourOfAKind, dice);
        Assert.Equal(18, result);
    }

    [Fact]
    public void Calculate_FourOfAKind_InvalidHand_ReturnsZero()
    {
        var dice = CreateDice(4, 4, 4, 1, 2);
        int result = ScoreCalculator.Calculate(ScoreCategory.FourOfAKind, dice);
        Assert.Equal(0, result);
    }

    // Example from Wikipedia
    [Fact]
    public void Calculate_FourOfAKind_WikipediaExample_Returns24()
    {
        var dice = CreateDice(4, 5, 5, 5, 5);
        int result = ScoreCalculator.Calculate(ScoreCategory.FourOfAKind, dice);
        Assert.Equal(24, result);
    }

    [Theory]
    [InlineData(new[] { 3, 2, 3, 2, 3 }, 25)]
    [InlineData(new[] { 1, 6, 6, 1, 6 }, 25)]
    public void Calculate_FullHouse_ValidHand_Returns25(int[] values, int expected)
    {
        var dice = CreateDice(values);
        int result = ScoreCalculator.Calculate(ScoreCategory.FullHouse, dice);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Calculate_FullHouse_InvalidHand_ReturnsZero()
    {
        var dice = CreateDice(2, 2, 3, 3, 4);
        int result = ScoreCalculator.Calculate(ScoreCategory.FullHouse, dice);
        Assert.Equal(0, result);
    }

    // Example from Wikipedia
    [Fact]
    public void Calculate_FullHouse_WikipediaExample_Returns25()
    {
        var dice = CreateDice(2, 2, 5, 5, 5);
        int result = ScoreCalculator.Calculate(ScoreCategory.FullHouse, dice);
        Assert.Equal(25, result);
    }

    [Theory]
    [InlineData(new[] { 4, 1, 3, 2, 6 }, 30)]
    [InlineData(new[] { 3, 5, 4, 6, 1 }, 30)]
    public void Calculate_SmallStraight_ValidHand_Returns30(int[] values, int expected)
    {
        var dice = CreateDice(values);
        int result = ScoreCalculator.Calculate(ScoreCategory.SmallStraight, dice);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Calculate_SmallStraight_InvalidHand_ReturnsZero()
    {
        var dice = CreateDice(1, 2, 3, 5, 6);
        int result = ScoreCalculator.Calculate(ScoreCategory.SmallStraight, dice);
        Assert.Equal(0, result);
    }

    // Example from Wikipedia
    [Fact]
    public void Calculate_SmallStraight_WikipediaExample_Returns30()
    {
        var dice = CreateDice(1, 3, 4, 5, 6);
        int result = ScoreCalculator.Calculate(ScoreCategory.SmallStraight, dice);
        Assert.Equal(30, result);
    }

    [Theory]
    [InlineData(new[] { 5, 2, 4, 3, 6 }, 40)]
    [InlineData(new[] { 1, 2, 4, 3, 5 }, 40)]
    public void Calculate_LargeStraight_ValidHand_Returns40(int[] values, int expected)
    {
        var dice = CreateDice(values);
        int result = ScoreCalculator.Calculate(ScoreCategory.LargeStraight, dice);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Calculate_LargeStraight_InvalidHand_ReturnsZero()
    {
        var dice = CreateDice(1, 2, 3, 4, 6);
        int result = ScoreCalculator.Calculate(ScoreCategory.LargeStraight, dice);
        Assert.Equal(0, result);
    }

    // Example from Wikipedia
    [Fact]
    public void Calculate_LargeStraight_WikipediaExample_Returns40()
    {
        var dice = CreateDice(1, 2, 3, 4, 5);
        int result = ScoreCalculator.Calculate(ScoreCategory.LargeStraight, dice);
        Assert.Equal(40, result);
    }

    [Fact]
    public void Calculate_King_ValidHand_Returns50()
    {
        var dice = CreateDice(6, 6, 6, 6, 6);
        int result = ScoreCalculator.Calculate(ScoreCategory.King, dice);
        Assert.Equal(50, result);
    }

    [Fact]
    public void Calculate_King_InvalidHand_ReturnsZero()
    {
        var dice = CreateDice(6, 6, 6, 6, 5);
        int result = ScoreCalculator.Calculate(ScoreCategory.King, dice);
        Assert.Equal(0, result);
    }

    // Example from Wikipedia
    [Fact]
    public void Calculate_King_WikipediaExample_Returns50()
    {
        var dice = CreateDice(1, 1, 1, 1, 1);
        int result = ScoreCalculator.Calculate(ScoreCategory.King, dice);
        Assert.Equal(50, result);
    }

    [Fact]
    public void Calculate_Chance_ReturnsSum()
    {
        var dice = CreateDice(1, 2, 3, 4, 6);
        int result = ScoreCalculator.Calculate(ScoreCategory.Chance, dice);
        Assert.Equal(16, result);
    }

    // Example from Wikipedia
    [Fact]
    public void Calculate_Chance_WikipediaExample_Returns13()
    {
        var dice = CreateDice(1, 1, 3, 3, 5);
        int result = ScoreCalculator.Calculate(ScoreCategory.Chance, dice);
        Assert.Equal(13, result);
    }

    [Theory]
    [InlineData(62, 0)]
    [InlineData(63, 35)]
    [InlineData(100, 35)]
    public void CalculateBonus_CorrectThresholds_ReturnsBonus(int upperSum, int expectedBonus)
    {
        int result = ScoreCalculator.CalculateBonus(upperSum);
        Assert.Equal(expectedBonus, result);
    }

    [Fact]
    public void CalculateUpperSum_CorrectSum_ReturnsTotal()
    {
        int?[] scores = new int?[18];
        scores[ScoreCategory.Ones] = 3;
        scores[ScoreCategory.Twos] = 6;
        scores[ScoreCategory.Sixes] = 12;

        int result = ScoreCalculator.CalculateUpperSum(scores);
        Assert.Equal(21, result);
    }

    [Fact]
    public void CalculateLowerSum_CorrectSum_ReturnsTotal()
    {
        int?[] scores = new int?[18];
        scores[ScoreCategory.ThreeOfAKind] = 15;
        scores[ScoreCategory.FullHouse] = 25;
        scores[ScoreCategory.King] = 50;

        int result = ScoreCalculator.CalculateLowerSum(scores);
        Assert.Equal(90, result);
    }
}
