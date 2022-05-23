using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationOld : MonoBehaviour
{
    public RectTransform textTrans;
    public TMPro.TextMeshPro textMesh;

    private float dir;
    public string base_face;
    public string air_face;

    //visual
    private float face_base_xoffset = 0.15f;
    //private float face_yoffset = 0f;
    private float face_xoffset = 0.2f;
    private bool grounded;
    private string face_str;

    public bool Grounded
    {
        get {
            return grounded;
        }
        set {
            grounded = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if(input != 0)   dir = input;

        face_xoffset = dir * face_base_xoffset;

        
        textTrans.anchoredPosition = new Vector3(face_xoffset, 0f, 0f);

        //determine text
        face_str = base_face;
        if(!grounded)
        {
            face_str = air_face;
        }

        textMesh.text = face_str;
    }

    //functions
    public void jump()
    {
        //apply jump anim       
    }

    public void turn(){}
}
