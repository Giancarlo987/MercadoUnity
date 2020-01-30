using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaticDistribution : MonoBehaviour
{
    public TextMeshProUGUI text;

    public static bool dist1 = false;
    public static bool dist2 = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableDist1 (bool check)
    {
        dist1 = check;
    }

    public void EnableDist2(bool check)
    {
        dist2 = check;
    }
}
