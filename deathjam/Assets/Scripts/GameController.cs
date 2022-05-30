using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    void Start()
    {

    }
    
    //public Scene nextScene;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
    }

    public void nextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);    
    }
    public void prevScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);    
    }
}
