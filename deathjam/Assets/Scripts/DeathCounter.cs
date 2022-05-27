using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{   
    private Text textComp;
    private int deathCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        textComp = GetComponent<Text>();
    }

    public void addDeath()
    {
        deathCount++;
        textComp.text = "Death Count: " + deathCount;
    }
}
