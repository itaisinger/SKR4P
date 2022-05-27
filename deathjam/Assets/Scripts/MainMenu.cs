using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nextScene;
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            trans.TransitionOut();
            ending = true;
        }

        if(ending && trans.IsDone())
            SceneManager.LoadScene(nextScene);
    }
}