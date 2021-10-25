using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    public GameObject panel;
    public Text dialogMessage, yesText, noText;
    public Button yesButton, noButton;

    private void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    public void ShowDialog(string message, UnityAction yesAction, UnityAction noAction = null, 
        string YesText = "Yes", string NoText = "No")
    {
        noButton.gameObject.SetActive(true);

        dialogMessage.text = message;

        yesText.text = YesText;
        noText.text = NoText;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        if (noAction != null)
        {
            noButton.onClick.AddListener(noAction);
        }

        noButton.onClick.AddListener(DisablePanel);

        yesButton.onClick.AddListener(yesAction);
        yesButton.onClick.AddListener(DisablePanel);

        panel.SetActive(true);
    }

    public void ShowMessage(string message)
    {
        noButton.gameObject.SetActive(false);

        dialogMessage.text = message;

        yesText.text = "Ok";

        yesButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(DisablePanel);

        panel.SetActive(true);
    }

    void DisablePanel()
    {
        panel.SetActive(false);
    }
}
