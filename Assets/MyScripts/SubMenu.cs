using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubMenu : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject OVRFirstPlayer;

    public static bool scenePaused = false;

    private List<GameObject> products1 = new List<GameObject>();
    private List<GameObject> products2 = new List<GameObject>();

    // Initialize products
    void Start()
    {
        GameObject[] myListProducts1 = GameObject.FindGameObjectsWithTag("Product1");
        GameObject[] myListProducts2 = GameObject.FindGameObjectsWithTag("Product2");

        products1 = new List<GameObject>();
        products2 = new List<GameObject>();

        for (int i = 0; i < myListProducts1.Length; i++)
        {
            if (myListProducts1[i] != transform)
                products1.Add(myListProducts1[i]);
        }

        for (int i = 0; i < myListProducts2.Length; i++)
        {
            if (myListProducts2[i] != transform)
                products2.Add(myListProducts2[i]);
            myListProducts2[i].SetActive(false);
        }

        for (int i = 0; i < products1.Count; i++)
        {

            Debug.Log(products1[i].name);
        }

        for (int i = 0; i < products2.Count; i++)
        {

            Debug.Log(products2[i].name);
        }
    }

    private void Update()
    {
        if (scenePaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Distribution1()
    {

        //StartCoroutine(LoadAsync());
        for (int i = 0; i < products1.Count; i++)
        {
            products1[i].SetActive(true);
        }

        for (int i = 0; i < products2.Count; i++)
        {
            products2[i].SetActive(false);
        }

        Resume();

    }

    public void Distribution2()
    {
        //StartCoroutine(LoadAsync());
        for (int i = 0; i < products1.Count; i++)
        {
            products1[i].SetActive(false);
        }

        for (int i = 0; i < products2.Count; i++)
        {
            products2[i].SetActive(true);
        }

        Resume();
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Pause()
    {
        transform.position = new Vector3(OVRFirstPlayer.transform.position.x, 3, OVRFirstPlayer.transform.position.z+5);
        PauseMenu.SetActive(true);
        Time.timeScale = .0f;
    }

    public void BackToMainMenu()
    {
        StartCoroutine(LoadAsync(0));
    }

    IEnumerator LoadAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
