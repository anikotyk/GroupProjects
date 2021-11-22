using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    public int lives = 5;
    public int money;
    public int totalscore;

    [Header("Upgrade Options")]
    public List<ShootProfile> blasterUpgradeList = new List<ShootProfile>();
    public List<ShootProfile> missileUpgradeList = new List<ShootProfile>();
    public List<float> healthUpgradeList = new List<float>();
    public List<MegaBombData> megaBombUpgList = new List<MegaBombData>();
    public List<ShieldData> shieldUpgList = new List<ShieldData>();
    public List<LaserData> laserUpgList = new List<LaserData>();

    public Dictionary<string, Medals> achievementList = new Dictionary<string, Medals>();
    public Dictionary<string, DateTime> statsTimer = new Dictionary<string, DateTime>();

    [Header("Upgrade Timer Data")]
    public List<StatsUpgradeInfo> stats = new List<StatsUpgradeInfo>();

    //singleton pattern initialization
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        if (File.Exists(Application.persistentDataPath + "/savegame.dat"))
        {
            LoadProgress(SaveSystem.Load<SaveData>());
        }
        else
        {
            money = 100;
            totalscore = 0;
        }
        UpdateMoney.instance.DisplayScore(totalscore);
    }

    private void OnDestroy()
    {
        SaveProgress();

    }

    public void AddMoney(int value)
    {
        money += value;

        //TO-DO: ui update system
        UpdateMoney.instance.DisplayMoney(money);
    }

    //this is for getting the stats value, such as weapon or powerups.
    public T GetStatsValue<T>(string statName, List<T> statsList)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].name == statName)
            {
                return statsList[stats[i].level - 1];
            }
        }

        return default(T);
    }

    //method to getting the upgrade time
    public float[] GetUpgradeTime(string statName)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].name == statName)
            {
                return stats[i].upgradeTime;
            }
        }

        return null;
    }

    public StatsUpgradeInfo GetStats(string statName)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].name == statName)
            {
                return stats[i];
            }
        }
        return null;
    }

    public void SaveProgress()
    {
        SaveSystem.Save(CreateSaveDataObject());
        GameObject.FindObjectOfType<GooglePlayServicesManager>().OpenCloudToSaveOrLoad(true);
    }

    public string CreateJsonData()
    {
        return JsonUtility.ToJson(CreateSaveDataObject());
    }

    public SaveData CreateSaveDataObject()
    {
        SaveData saveData = new SaveData();

        saveData.lives = lives;
        saveData.money = money;
        saveData.totalscore = totalscore;

        saveData.achievementList = achievementList;
        saveData.statsTimer = statsTimer;

        saveData.stats = stats;
        return saveData;
    }

    public void LoadProgress(SaveData loadData)
    {
        lives = loadData.lives;
        money = loadData.money;
        totalscore = loadData.totalscore;

        achievementList = loadData.achievementList;
        statsTimer = loadData.statsTimer;

        stats = loadData.stats;

        UpdateItemDisplay();
    }

    void UpdateItemDisplay()
    {
        UpgradeItem[] items = FindObjectsOfType<UpgradeItem>();
        LevelMenu[] levelMenus = FindObjectsOfType<LevelMenu>();

        for (int i = 0; i < items.Length; i++)
        {
            items[i].UpdateItemDisplay();
        }

        for (int i = 0; i < levelMenus.Length; i++)
        {
            levelMenus[i].UpdateMenu();
        }

        UpdateMoney.instance.UpdateMoneyDisplay();
        UpdateMoney.instance.DisplayScore(StatsManager.instance.totalscore);
    }

    public void AddMedals(string levelName, Medals medal)
    {
        Medals newMedal = new Medals();

        if (achievementList.ContainsKey(levelName))
        {
            newMedal.kill = medal.kill ? true : achievementList[levelName].kill;
            newMedal.rescue = medal.rescue ? true : achievementList[levelName].rescue;
            newMedal.untouched = medal.untouched ? true : achievementList[levelName].untouched;

            achievementList[levelName] = newMedal;
        }
        else
        {
            achievementList.Add(levelName, medal);
        }
    }
}

[System.Serializable]
public class StatsUpgradeInfo
{
    public string name;
    public int level;
    public float[] upgradeTime;
}

[System.Serializable]
public class MegaBombData
{
    public float radius;
    public float damage;
}

[System.Serializable]
public class ShieldData
{
    public float shieldDuration;
}

[System.Serializable]
public class LaserData
{
    public float laserDuration;
}

[System.Serializable]
public class SaveData
{
    public int lives;
    public int money;
    public int totalscore;
    public Dictionary<string, Medals> achievementList = new Dictionary<string, Medals>();
    public Dictionary<string, DateTime> statsTimer = new Dictionary<string, DateTime>();
    public List<StatsUpgradeInfo> stats = new List<StatsUpgradeInfo>();
}
