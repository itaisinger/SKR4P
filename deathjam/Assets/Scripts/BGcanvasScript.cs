using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGcanvasScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private static BGcanvasScript instance = null;
    public static BGcanvasScript Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}