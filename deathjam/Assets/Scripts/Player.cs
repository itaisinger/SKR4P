using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject bodyObject;
    [SerializeField] public GameObject spawn;
    [SerializeField] private float _slide;

    [HideInInspector] public GameObject lastBody = null;
    private PlayerController controller_script;
    private int death_cooldown;
    private BoxCollider2D boxCollider;

    public Tilemap hazards;

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

        //spikes detection
        Vector3Int hazardsMap = hazards.WorldToCell(transform.position);

        if(hazards.GetTile(hazardsMap) != null)
        {
            kill(0f);
        }

        //tp back if stuck
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = spawn.transform.position;
        }
    }
    
    //kill
    public void kill(float killer)
    {
        if(death_cooldown > 0)
            return;
    
        death_cooldown = 10;

        if(lastBody != null)
            lastBody.GetComponent<Body>().changeSprite();

        //create a corpse and respawn
        GameObject newBody = Instantiate(bodyObject);
        newBody.GetComponent<Body>().setMomentum(controller_script._currentHorizontalSpeed * _slide * Time.deltaTime, controller_script._currentVerticalSpeed * _slide * Time.deltaTime);
        lastBody = newBody;
        newBody.transform.position = transform.position;
        newBody.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;

        transform.position = spawn.transform.position;
        //set spawn animation
    }
}
