using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Body : MonoBehaviour
{
    private float xadd, yadd;
    private BoxCollider2D boxCollider;
    private int ground_layer;
    private bool moving = true;
    private Rigidbody2D body;
    private int roundRemain = 0;                        //frames remain to slide into spikes
    
    [SerializeField] private int rounds = 10;           //total amount of frames to slide into spikes
    [SerializeField] private float velocity_mult = 10f; //slide velocity multiplier
    [SerializeField] private float OppositePushMult = 0f; //slide velocity multiplier

    [SerializeField] private Sprite nextSprite; //normal body sprite

    [HideInInspector] public Vector2 velocity;
    private string HOR_TAG = "SpikesHor";
    private string VER_TAG = "SpikesVer";
    private bool collidedWithSpikeField = false;

    // Start is called before the first frame update
    void Start()
    {
        ground_layer = LayerMask.NameToLayer("Ground");
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();

        roundRemain = rounds;
    }

    // move into the spikes nicely
    void FixedUpdate()
    {   
        //if remain slide frames, go sliding.
        if(moving && roundRemain > 0)
        {
            //body.velocity = new Vector3(0f,0f,0f);
            body.AddForce(velocity * velocity_mult,ForceMode2D.Impulse);

            //in polish might be night to do velocity_mult *= 0.9; to ease in.
            roundRemain--;

        }
        else if(roundRemain < -rounds)
        {
            body.velocity = new Vector3(0f,0f,0f);
            roundRemain--;
        }
        else
        {
            body.bodyType = RigidbodyType2D.Static;
        }
    }

    //collision with spikes fields
    void OnTriggerEnter2D(Collider2D spikeField)
    {   
        //here, check what kind of spikes im colliding with by checking for trigger collide with spikes hor or ver tag.
        //then, nulify the velocity in the appropriate direction.

        //no need to repeat this
        if(collidedWithSpikeField) return;

        collidedWithSpikeField = true;
        
        body.velocity = new Vector2(0f,0f);

        //spikes are vertical
        if(spikeField.CompareTag(VER_TAG))
        {
            velocity.x *= OppositePushMult;
        }
        //spikes are horizontal
        if(spikeField.CompareTag(HOR_TAG))
        {
            velocity.y *= OppositePushMult;
        }
    }

    public void setMomentum(float xadd,float yadd)
    {
        velocity = new Vector2(xadd,yadd);
    }

    public void changeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = nextSprite;
    }
}