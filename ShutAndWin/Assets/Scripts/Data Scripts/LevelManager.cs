using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Medals medals = new Medals();
    public int totalEnemy, enemyKilled, totalRescue, humanRescued, score;

    public UnityEvent onGameEnd;

    public GameObject OnGameover;
    public GameObject onGameWin;
    private string levelName;

    private void Awake()
    {
        instance = this;
        medals.untouched = true;

        levelName = SceneManager.GetActiveScene().name;
        
    }
    private void Start()
    {
        UpdateMoney.instance.DisplayScore(0);
    }

    public void RegisterEnemy()
    {
        totalEnemy++;
    }

    public void RegisterRescue()
    {
        totalRescue++;
    }

    public void AddEnemyKill(int scoreValue)
    {
        enemyKilled++;
        score += scoreValue;
        UpdateMoney.instance.DisplayScore(score);
    }

    public void AddRescue()
    {
        humanRescued++;
        score += 3;
        UpdateMoney.instance.DisplayScore(score);
    }

    public void PlayerHit()
    {
        medals.untouched = false;
    }

    public void GameEnd()
    {
        StartCoroutine(CountDelay());
    }

    public void GameOver()
    {
        //StartCoroutine(CountDelay());
        OnGameover.SetActive(true);
        onGameWin.SetActive(false);
        onGameEnd.Invoke();
    }

    IEnumerator CountDelay()
    {
        yield return new WaitForSeconds(0.25f);

        if (enemyKilled >= totalEnemy)
            medals.kill = true;

        if (humanRescued >= totalRescue)
            medals.rescue = true;

        StatsManager.instance.AddMedals(levelName, medals);
        StatsManager.instance.totalscore += score;
        GooglePlayServicesManager.instance.PostScoreToLeaderboard(StatsManager.instance.totalscore);
        AchivmentCheck();
        onGameEnd.Invoke();
    }

    public void AchivmentCheck()
    {
        if (StatsManager.instance.totalscore >= 1000000)
        {
            GooglePlayServicesManager.instance.AchievementCompleted(GooglePlayServicesManager.instance.achivment5);
        }else if (StatsManager.instance.totalscore >= 100000)
        {
            GooglePlayServicesManager.instance.AchievementCompleted(GooglePlayServicesManager.instance.achivment4);
        }
        else if (StatsManager.instance.totalscore >= 10000)
        {
            GooglePlayServicesManager.instance.AchievementCompleted(GooglePlayServicesManager.instance.achivment3);
        }
        else if (StatsManager.instance.totalscore >= 1000)
        {
            GooglePlayServicesManager.instance.AchievementCompleted(GooglePlayServicesManager.instance.achivment2);
        }
        else if (StatsManager.instance.totalscore >= 100)
        {
            GooglePlayServicesManager.instance.AchievementCompleted(GooglePlayServicesManager.instance.achivment1);
        }
        
    }
}

[System.Serializable]
public class Medals
{
    public bool rescue, kill, untouched;
}
