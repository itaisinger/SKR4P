using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject bodyObject;
    [SerializeField] public GameObject spawn;
    [SerializeField] private float _slide;

    [HideInInspector] public GameObject lastBody = null;
    private PlayerController controller_script;
    private int death_cooldown;

    // Start is called before the first frame update
    void Start()
    {
        controller_script = GetComponent<PlayerController>();
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
        float xadd = controller_script._currentHorizontalSpeed  * _slide;
        float yadd = controller_script._currentVerticalSpeed    * _slide;
    
        Debug.Log(xadd + ", " + yadd);

        lastBody = newBody;
        newBody.transform.position = transform.position;
        newBody.transform.position += new Vector3(xadd,yadd,0);

        transform.position = spawn.transform.position;
        //set spawn animation
    }
}
