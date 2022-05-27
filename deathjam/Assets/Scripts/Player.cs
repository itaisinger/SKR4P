using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject bodyObject;
    [SerializeField] private GameObject spawn;
    [SerializeField] private float _slide;

    [HideInInspector] public GameObject lastBody = null;

    private PlayerController controller_script;
    private PlayerSfx sfx_script;
    private int death_cooldown = 0;
    private BoxCollider2D boxCollider;
    private DeathCounter deathCounter;
    public Tilemap hazards;

    // Start is called before the first frame update
    void Start()
    {
        //get some components
        controller_script   = GetComponent<PlayerController>();
        sfx_script          = GetComponent<PlayerSfx>();
        deathCounter        = GameObject.FindWithTag("deathCounter").GetComponent<DeathCounter>();
        boxCollider         = GetComponent<BoxCollider2D>();

        //spawn at spawn
        transform.position = spawn.transform.position;
        controller_script._currentHorizontalSpeed   = spawn.GetComponent<Spawn>().xMomentum;
        controller_script._currentVerticalSpeed     = spawn.GetComponent<Spawn>().yMomentum * 2f;  
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
            sfx_script.playdeathSfx();
        }

        //play sfx if jumping, might not be the wisest place to put this but whatever
        if(controller_script.JumpingThisFrame)
            sfx_script.playJumpSfx();
            
        if(controller_script.LandingThisFrame)
            sfx_script.playLandSfx();
    }
    
    //kill
    public void kill(float killer)
    {
        if(death_cooldown > 0)
            return;
    
        death_cooldown = 10;

        if(lastBody != null)
            lastBody.GetComponent<Body>().changeSprite();

        //create a body
        GameObject newBody = Instantiate(bodyObject);
        newBody.GetComponent<Body>().setMomentum(controller_script._currentHorizontalSpeed * _slide * Time.deltaTime, controller_script._currentVerticalSpeed * _slide * Time.deltaTime);
        lastBody = newBody;
        newBody.transform.position = transform.position;
        newBody.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;

        //respawn
        transform.position = spawn.transform.position;

        controller_script._currentHorizontalSpeed   = spawn.GetComponent<Spawn>().xMomentum;
        controller_script._currentVerticalSpeed     = spawn.GetComponent<Spawn>().yMomentum;

        //increment death count
        deathCounter.addDeath();

        //play sfx
        sfx_script.playdeathSfx();
    }
}
