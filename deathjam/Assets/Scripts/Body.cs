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

    [SerializeField] private Sprite nextSprite; //normal body sprite
    //[SerializeField] private Tilemap hazardsTilemap;

    [HideInInspector] public Vector2 velocity;

    //public List<SpikeTileData> spikeTilesData;
    //private SpikeTileData spike;

    // Start is called before the first frame update
    void Start()
    {
        ground_layer = LayerMask.NameToLayer("Ground");
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();

        roundRemain = rounds;

        //find tile
        ///hazardsTilemap = GameObject.FindWithTag("Hazard").GetComponent<Tilemap>();                                                  //get the tilemap component
        //Vector3Int spikeTileCoords = hazardsTilemap.WorldToCell(transform.position + new Vector3(velocity.x, velocity.y, 0f));      //get the tile coords on the tiles grid
        //spikesTile = hazardsTilemap.GetTile(spikeTileCoords);                                                                       //get the spikes type
        //Debug.Log(spikesTile);

        //initialize the tiledata list
        //spikeTilesData = Resources.LoadAll<TileData>("TileData").ToList();
        //spike = GetTileData(transform.position);
        //Debug.Log(spike.dir);
    }

    // move into the spikes nicely
    void Update()
    {   
        //if remain slide frames, go sliding.
        if(moving && roundRemain > 0)
        {
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

    public void setMomentum(float xadd,float yadd)
    {
        velocity = new Vector2(xadd,yadd);
    }

    public void changeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = nextSprite;
    }

    // public TileData GetTileData(Vector3 position)
    // {
    //     //position in grid
    //     Vector3Int pos = hazardsTilemap.WorldToCell(transform.position);   

    //     //getting the tile in the grid 
    //     Tile t = hazardsTilemap.GetTile<Tile>(pos);

    //     TileData tile = null;

    //     foreach (TileData til in spikeTilesData)
    //     {
    //         for(int i = 0; i < til.spikeTilesData.Count; i++)
    //         {
    //             if(til.spikeTilesData[i] == t)
    //             {
    //                 tile = til;
    //                 break;
    //             }
    //         }
    //     }

    //     if (tile != null)
    //         return tile;
    //     else
    //         return null;
    // }
}