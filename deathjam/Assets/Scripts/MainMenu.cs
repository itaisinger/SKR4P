using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nextScene;
    public string creditsScene;
    private string targetScene;
    private Transition trans;
    private bool ending = false;

    // Start is called before the first frame update
    void Start()
    {
        trans = GameObject.FindWithTag("Transition").GetComponent<Transition>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("quit");
            Application.Quit();
        }

        //enter first level
        if(Input.GetKeyDown(KeyCode.Space))
        {  
            ending = true;
            targetScene = nextScene;
            trans.TransitionOut();
        }
        //enter credits
        if(Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("c");
            ending = true;
            targetScene = creditsScene;
            trans.TransitionOut();
        }

        

        if(ending && trans.IsDone())
            SceneManager.LoadScene(targetScene);
    }
}