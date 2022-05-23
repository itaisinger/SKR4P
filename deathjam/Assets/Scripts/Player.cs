using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject bodyObject;
    [SerializeField] public GameObject spawn;

    [HideInInspector] public GameObject lastBody = null;
    private int death_cooldown;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = spawn.transform.position;
        death_cooldown = 0;
        lastBody = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(death_cooldown > 0) death_cooldown--;
    }

    
    //kill
    public void kill(float killer)
    {
        if(death_cooldown > 0)
            return;
        
        death_cooldown = 10;

        //create a corpse and respawn
        GameObject newBody = Instantiate(bodyObject);
        
        lastBody = newBody;
        newBody.transform.position = transform.position;

        transform.position = spawn.transform.position;
        //set spawn animation
    }
}
