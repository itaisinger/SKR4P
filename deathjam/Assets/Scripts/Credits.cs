using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public string MenuScene;
    public bool ending = false;
    public Transition trans;

    void Start()
    {
        trans = GameObject.FindWithTag("Transition").GetComponent<Transition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            ending = true;
            trans.TransitionOut();
        }

        if(ending && trans.IsDone())
            SceneManager.LoadScene(MenuScene);
    }
}