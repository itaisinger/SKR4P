using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float scale_max = 1.3f;
    [SerializeField] private float scale_min = 0.7f;
    [SerializeField] private float squash_spd = 0.5f;

    private float scale_w;
    private float scale_h;
    private float dir;

    private PlayerController controller_script;
    private Animator anim;
    private SpriteRenderer sr;
    private BoxCollider2D box;

    public float y_off;

    void Start()
    {
        controller_script = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();

        scale_h = 1f;
        scale_w = 1f;
        dir = 1;
        y_off = 0f;
    }

    // Update is called once per frame
    void Update()
    {   
        //// dir ////

        dir = Input.GetAxisRaw("Horizontal") != 0f ? Input.GetAxisRaw("Horizontal") : dir;
        sr.flipX = dir == -1;

        //// squash and stretch ////

        //approach reset
        scale_w = Mathf.MoveTowards(scale_w, 1, squash_spd * Time.deltaTime);
        scale_h = Mathf.MoveTowards(scale_h, 1, squash_spd * Time.deltaTime);

        //check landing
        if(controller_script.LandingThisFrame)
        {
            scale_w = scale_max;
            scale_h = scale_min;
        }

        //check jumping
        if(controller_script.JumpingThisFrame)
        {
            scale_h = scale_max;
            scale_w = scale_min;
        }
        
        //Bounds boxBounds = box.bounds;
        //Vector2 bottom = new Vector2(boxBounds.center.x, boxBounds.center.y);
        //Debug.Log(boxBounds.center.y + boxBounds.extents.y);
        //ScaleAround(gameObject,transform.position - new Vector3(0, boxBounds.extents.y, 0), new Vector3(scale_w, scale_h, 1));
        transform.localScale = new Vector3(scale_w,scale_h,1);

        //// set animator controller bools ////

        //moving
        anim.SetBool("moving",controller_script._currentHorizontalSpeed != 0f); //Input.GetAxisRaw("Horizontal") != 0);
        

        //jumping
        anim.SetBool("jumping",controller_script.JumpingThisFrame);

        //ground
        anim.SetBool("ground",controller_script._colDown);
        
    }

    void LateUpdate()
    {
        //transform.position -= new Vector3(0f,y_off,0f);
    }
    void onPostRender()
    {
        //transform.position += new Vector3(0f,y_off,0f);
    }

    //use the scale from the bottom
    public void ScaleAround(GameObject target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 A = target.transform.localPosition;
        Vector3 B = pivot;
    
        Vector3 C = A - B; // diff from object pivot to desired pivot/origin
    
        float RS = newScale.x / target.transform.localScale.x; // relataive scale factor
    
        // calc final position post-scale
        Vector3 FP = B + C * RS;    //final position
    
        // finally, actually perform the scale/translation
        target.transform.localScale = newScale;
        target.transform.localPosition = FP;
    }
}
