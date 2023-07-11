using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ScoreData : MonoBehaviour //this script should be called GameData
{
    [SerializeField] private List<HiScore> scoreboardList;
    
    public ScoreData()
    {
        //this.isRaining = false;
        //this.hp = 100;
        //this.coins = 0;
        //this.score = 0;
        //timeAlive = 0;
        //enemiesKilled = 0;
    }

    /*public int GetScore(int index)
    {
        return scoreboardList[index].finalScore;
    }*/
    public HiScore GetScoreboardData(int index)
    {
        return scoreboardList[index];
    }
    public List<HiScore> GetFullScoreboardData()
    {
        return scoreboardList;
    }
    public bool ScoreIsNewRecord(HiScore contender)
    {
        if (scoreboardList[10] != null)
        {
            if (scoreboardList[10].finalScore < contender.finalScore)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
    public int SetNewHiScore(HiScore newRecord) //returns int to display required elements such as name entry on the corresponding leaderboard spot
    {
        for (int i = 0; i < scoreboardList.Count; i++)
        {
            if (newRecord.finalScore > scoreboardList[i].finalScore)
            {
                if (scoreboardList[10] != null)
                {
                    scoreboardList.RemoveAt(10);
                }

                for (int j = scoreboardList.Count; j > i; j--)
                {
                    scoreboardList.Insert(j, scoreboardList[j-1]);
                }
                scoreboardList.Insert(i, newRecord);
                return i;
            }
        }
        print("hiscoreset returned 0");
        return 0; //this is an error case
    }

    /*public int GetCoins()
    {
        return coins;
        //return this.isRaining;
    }

    public int GetHP()
    {
        return hp;
    }
    public void SetHP(int num)
    {
        hp = num;
    }
    public void SetCoins(int num)
    {
        coins = num;
    }*/
    /*public void SetRainState(bool rain)
    {
        //this.isRaining = rain;
    }*/
}

[System.Serializable]
public class HiScore
{
    public int finalScore = 0;
    public int timeSurvived = 0;
    public int totalKilled = 0;
    public int wavesCompleted = 0;
    public string playerName = "AAA";
}
