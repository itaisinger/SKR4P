using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{   
    private Text textComp;
    private string base_string = "";//"Death Count: ";
    //private int deathCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        textComp = GetComponent<Text>();
        textComp.text = base_string + Data.Deaths;
    }

    public void addDeath()
    {
        Debug.Log("death added");
        Data.Deaths++;
        textComp.text = base_string + Data.Deaths;
    }
}