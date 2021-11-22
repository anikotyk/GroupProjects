using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateMoney : MonoBehaviour
{
    public static UpdateMoney instance;

    public Text moneyDisplay, scoreDisplay;

	// Use this for initialization
	void Awake ()
    {
        instance = this;	
	}

    private void Start()
    {
        DisplayMoney(StatsManager.instance.money);
        UpdateMoney.instance.DisplayScore(StatsManager.instance.totalscore);
    }

    public void DisplayMoney(int value)
    {
        if (moneyDisplay)
            moneyDisplay.text = "$ " + value.ToString();
    }

    public void UpdateMoneyDisplay()
    {
        DisplayMoney(StatsManager.instance.money);
    }

    public void DisplayScore(int value)
    {
        string text = value.ToString("00000000");
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            text = "score: " + value;
        }
        if (scoreDisplay)
            scoreDisplay.text = text;
    }
    
}
