using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public string sceneTarget;

    public Button playButton;
    public Text killText, rescueText, untouchedText;

    public Color disabledColor, enabledColor;

    private Medals sceneMedal;

	// Use this for initialization
	void Start ()
    {
        //if (StatsManager.instance.achievementList.ContainsKey(sceneTarget))
        //    sceneMedal = StatsManager.instance.achievementList[sceneTarget];

        UpdateMenu();

        playButton.onClick.AddListener(GoToLevel);
	}

    public void UpdateMenu()
    {
        if (StatsManager.instance.achievementList.ContainsKey(sceneTarget))
            sceneMedal = StatsManager.instance.achievementList[sceneTarget];

        if (sceneMedal != null)
        {
            killText.color = sceneMedal.kill ? enabledColor : disabledColor;
            rescueText.color = sceneMedal.rescue ? enabledColor : disabledColor;
            untouchedText.color = sceneMedal.untouched ? enabledColor : disabledColor;
        }
        else
        {
            killText.color = disabledColor;
            rescueText.color = disabledColor;
            untouchedText.color = disabledColor;
        }
    }

    void GoToLevel()
    {
        SceneLoader.instance.ChangeScene(sceneTarget);
    }
}
