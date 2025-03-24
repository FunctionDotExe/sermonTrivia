using UnityEngine;

public class MultiplayerPlayerData
{
    public int playerNumber;
    public float timeMultiplier;
    public float remainingTime;
    public int score;
    public int currentRound;

    public MultiplayerPlayerData(int number)
    {
        playerNumber = number;
        timeMultiplier = Random.Range(1, 5) * 20f; // Random 20-80 seconds
        remainingTime = timeMultiplier;
        score = 0;
        currentRound = 1;
    }
} 