using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject textObj;
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(active)
            {
                Application.Quit();
            }
            else active = true;
        }
        else if(Input.anyKeyDown && active)
        {
            active = false;
        }

        textObj.SetActive(active);
    }
}