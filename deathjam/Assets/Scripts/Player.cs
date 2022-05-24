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
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        controller_script = GetComponent<PlayerController>();
        transform.position = spawn.transform.position;
        death_cooldown = 0;
        lastBody = null;

        boxCollider = GetComponent<BoxCollider2D>();
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
        newBody.GetComponent<Body>().setMomentum(controller_script._currentHorizontalSpeed * _slide * Time.deltaTime, controller_script._currentVerticalSpeed * _slide * Time.deltaTime);
        lastBody = newBody;
        newBody.transform.position = transform.position;
        //newBody.transform.position += new Vector3(xadd,yadd,0);

        transform.position = spawn.transform.position;
        //set spawn animation
    }
}
