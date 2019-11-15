using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Slider sliderMarket1;
    public TextMeshProUGUI progressText;

    public void LoadLevelMarket1(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    public void LoadLevelMarket2(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    public void LoadLevelMarket3(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            sliderMarket1.value = progress;

            progressText.text = Mathf.Round(progress * 100.0f).ToString() + '%';

            yield return null;
        }
    }
}
