using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    private static bool initialized;

    public static bool GetInitialized()
    {
        return initialized;
    }

    private static GameData gameData;
    private static ScoreData scoreData;

    public static GameData GetGameData()
    {
        return gameData;
    }
    public static ScoreData GetScoreData()
    {
        return scoreData;
    }

    public static void Save()
    {
        SaveManager.Save(gameData, "saveData");
    }
    public static void SaveScore()
    {
        SaveManager.Save(scoreData, "scoreboardData");
    }

    public static void Load()
    {
        if (initialized) return;

        gameData = SaveManager.Load<GameData>("saveData");

        initialized = true;
    }
    public static void LoadScoreboard()
    {
        if (initialized) return;

        scoreData = SaveManager.Load<ScoreData>("scoreboardData");

        initialized = true;
    }
}
