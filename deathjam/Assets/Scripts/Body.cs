using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public float xadd, yadd;
    private BoxCollider2D boxCollider;
    private int ground_layer;
    private bool moving = true;
    private int rounds = 0;

    // Start is called before the first frame update
    void Start()
    {
        ground_layer = LayerMask.NameToLayer("Ground");
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            Vector2 add = new Vector2(xadd,yadd);
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            pos += add;
            Vector2 size = new Vector2(boxCollider.size.x, boxCollider.size.y);

            if(Physics2D.OverlapBox(pos, size, 0f, 1<<ground_layer) == null)
            {
                transform.position += new Vector3(add.x,add.y,0f);
            }
            else
            {
                xadd *= 0.5f;
                yadd *= 0.5f;
                rounds++;
            }

            if(xadd + yadd < Time.deltaTime * 0.01f)
            {
                gameObject.layer = ground_layer;
                moving = false;
            }
        }
    }

    public void setMomentum(float xadd,float yadd)
    {
        this.xadd = xadd;
        this.yadd = yadd;
    }
}