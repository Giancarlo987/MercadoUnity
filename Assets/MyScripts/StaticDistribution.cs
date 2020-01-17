using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaticDistribution : MonoBehaviour
{
    public TextMeshProUGUI text;
    static int distribution = 0;

    public static bool dist1 = false;
    public static bool dist2 = true;

    // Start is called before the first frame update
    void Start()
    {
        distribution += 1;
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText(distribution.ToString());

        //if (dist1)
            text.SetText("Dist 1 " + dist1.ToString() + 
                "\n " + "Dist 2 " + dist2.ToString());
        //else if (dist2)
            //text.SetText("Dist 2 " + dist2.ToString());
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
