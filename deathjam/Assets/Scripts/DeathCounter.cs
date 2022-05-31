using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{   
    private Text textComp;
    private string base_string = "";
    private BGSoundScript data;

    // Start is called before the first frame update
    void Start()
    {
        textComp = GetComponent<Text>();
        data = GameObject.FindWithTag("Music").GetComponent<BGSoundScript>();
        textComp.text = base_string + data.Deaths;
    }

    void Update()
    {
        textComp.text = base_string + data.Deaths;
    }

    public void addDeath()
    {
        Debug.Log("death added through deathCounter");
        data.Increment();
    }
}