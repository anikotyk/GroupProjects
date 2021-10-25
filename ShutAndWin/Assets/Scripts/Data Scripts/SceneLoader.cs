using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public static SceneLoader instance;

    public GameObject panel;
    public Transform progressBar;

    private Vector3 barScale = Vector3.one;

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

        DisablePanel();
    }

    void DisablePanel()
    {
        panel.SetActive(false);
    }

    void UpdateBar(float value)
    {
        barScale.x = value;
        progressBar.localScale = barScale;
    }

    IEnumerator LoadScene(string sceneName)
    {
        panel.SetActive(true);

        AsyncOperation asyncLoading = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoading.isDone)
        {
            UpdateBar(Mathf.Clamp01(asyncLoading.progress/0.9f));

            yield return null;
        }

        DisablePanel();
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }
}
