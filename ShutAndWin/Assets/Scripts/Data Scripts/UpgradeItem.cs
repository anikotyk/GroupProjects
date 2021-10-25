using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UpgradeItem : MonoBehaviour
{
    [Header("Upgrade Menu Objects")]
    public string statName;
    public string itemName;
    public Text itemNameText, buyText;
    public Slider itemLevelBar;
    public Button buyButton;

    [Header("Item Prices Setup:")]
    public int[] pricesLevel;

    private StatsUpgradeInfo stat;
    private bool isUpgrading;

    // Use this for initialization
    void Start ()
    {
        stat = StatsManager.instance.GetStats(statName);

        itemNameText.text = itemName;

        if (stat.level == pricesLevel.Length)
        {
            buyText.text = "MAX";
        }
        else
        {
            buyText.text = pricesLevel[stat.level].ToString();
        }

        itemLevelBar.value = stat.level;

        buyButton.onClick.AddListener(BuyUpgrade);

        UpdateItemDisplay();
    }

    public void BuyUpgrade()
    {
        if (isUpgrading)
        {
            DialogManager.instance.ShowMessage(statName + " is currently upgrading");
            return;
        }

        if (StatsManager.instance.money >= pricesLevel[stat.level])
        {
            DialogManager.instance.ShowDialog("Do you really want to upgrade " + statName, () =>
            {
                //do the upgrade
                StatsManager.instance.AddMoney(-pricesLevel[stat.level]);

                StatsManager.instance.statsTimer.Add(statName, DateTime.Now.AddMinutes(StatsManager.instance.GetUpgradeTime(statName)[stat.level]));

                //start the coroutine
                StartCoroutine(DoUpgrade());
            });
            //Debug.Log("Upgrading " + statName);
        }
        else
        {
            //show message not enough money
            //Debug.Log("Not Enough Money");
            DialogManager.instance.ShowMessage("You don't have enough money to upgrade " + statName);
        }
    }

    public void UpdateItemDisplay()
    {
        UpdateMoney.instance.UpdateMoneyDisplay();

        stat = StatsManager.instance.GetStats(statName);

        itemLevelBar.value = stat.level;

        if (stat.level == pricesLevel.Length)
        {
            buyText.text = "MAX";
            return;
        }

        buyText.text = pricesLevel[stat.level].ToString();

        CheckForUpgradeStatus();
    }

    public void CheckForUpgradeStatus()
    {
        if (StatsManager.instance.statsTimer.ContainsKey(statName))
        {
            if (DateTime.Now < StatsManager.instance.statsTimer[statName])
            {
                StartCoroutine(DoUpgrade());
            }
            else
            {
                IncreaseStat();
            }
        }
    }

    IEnumerator DoUpgrade()
    {
        isUpgrading = true;

        TimeSpan timeRemaining = StatsManager.instance.statsTimer[statName] - DateTime.Now;

        while (timeRemaining.TotalSeconds > 0f)
        {
            timeRemaining = StatsManager.instance.statsTimer[statName] - DateTime.Now;
            buyText.text = string.Format("{0:00}:{1:00}", timeRemaining.Minutes, timeRemaining.Seconds);
            yield return null;
        }

        //do the upgrade

        isUpgrading = false;

        IncreaseStat();
    }

    void IncreaseStat()
    {
        stat.level++;

        if (isUpgrading)
        {
            StopAllCoroutines();
            isUpgrading = false;
        }

        buyText.text = pricesLevel[stat.level].ToString();
        itemLevelBar.value = stat.level;

        StatsManager.instance.statsTimer.Remove(statName);

        DialogManager.instance.ShowMessage("Finish upgrading " + statName);
    }
}
