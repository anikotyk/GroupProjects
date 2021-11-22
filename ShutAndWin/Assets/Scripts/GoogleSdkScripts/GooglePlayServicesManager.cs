using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.UI;
using System;

public class GooglePlayServicesManager : MonoBehaviour
{
    public string leaderbord = "CgkIoPSb59IbEAIQBg";

    public string achivment1 = "CgkIoPSb59IbEAIQAQ";
    public string achivment2 = "CgkIoPSb59IbEAIQAg";
    public string achivment3 = "CgkIoPSb59IbEAIQAw";
    public string achivment4 = "CgkIoPSb59IbEAIQBA";
    public string achivment5 = "CgkIoPSb59IbEAIQBQ";

    public static GooglePlayServicesManager instance;
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

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false).
            EnableSavedGames().
            Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        SignInUserPlayGames();
    }

    void SignInUserPlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (success) =>
        {
            switch (success)
            {
                case SignInStatus.Success:
                    Debug.Log("signined in player using play games successfully");
                    OpenCloudToSaveOrLoad(false);
                    break;
                default:
                    Debug.Log("Signin not successfull");
                    break;
            }
        });
    }

    public void PostScoreToLeaderboard(int score)
    {
        Social.ReportScore(score, leaderbord, (bool success) =>
        {
            if (success)
            {
                Debug.Log("successfully add score to leaderboard");
            }
            else
            {
                Debug.Log("not successfull");
            }
        });
    }

    public void ShowLeaderBoard()
    {
        Social.ShowLeaderboardUI();
    }

    public void AchievementCompleted(string achivmentid)
    {
        Social.ReportProgress(achivmentid, 100.0f, (bool success) =>
        {
            if (success)
            {
                Debug.Log("successfully unlocked achievements");
            }
            else
            {
                Debug.Log("not successfull");
            }
        });
    }


    //cloud saving data

    private bool issaving = false;
    private string SAVE_NAME = "userdata";

    public void OpenCloudToSaveOrLoad(bool saving)
    {
        if (Social.localUser.authenticated)
        {
            issaving = saving;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution
                (SAVE_NAME, GooglePlayGames.BasicApi.DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, SaveOrLoad);
        }
    }

    private void SaveOrLoad(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (issaving)
            {
                byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(GameObject.FindObjectOfType<StatsManager>().CreateJsonData());
                SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
                ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(meta, update, data, SaveUpdate);
            }
            else
            {
                ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, ReadDataFromCloud);
            }
        }
    }

    private void ReadDataFromCloud(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string savedata = System.Text.ASCIIEncoding.ASCII.GetString(data);
            if (savedata != "")
            {
                SaveData saveData = SaveSystem.Load<SaveData>(false, savedata);
                if (saveData.totalscore > StatsManager.instance.totalscore)
                {
                    StatsManager.instance.LoadProgress(saveData);
                    Debug.Log("LOADING DATA FROM SERVER");
                }
                else
                {
                    Debug.Log("SERVER DOESNT HAVE BETTER DATA");
                }
            }
        }
    }

    private void SaveUpdate(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        Debug.Log("SAVED DATA TO SERVER");
    }
}
