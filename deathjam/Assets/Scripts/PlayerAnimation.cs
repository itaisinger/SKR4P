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
    private PlayerController controller_script;
    private Animator anim;

    void Start()
    {
        controller_script = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        scale_h = 1f;
        scale_w = 1f;
    }

    // Update is called once per frame
    void Update()
    {   
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
        
        transform.localScale = (new Vector3(scale_w, scale_h, 1));
    
        //// set animator controller bools ////

        //moving
        anim.SetBool("moving",controller_script._currentHorizontalSpeed != 0f); //Input.GetAxisRaw("Horizontal") != 0);
        Debug.Log(controller_script._currentHorizontalSpeed != 0f);

        
    }
}
