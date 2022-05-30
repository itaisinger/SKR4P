using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBody : MonoBehaviour
{
    public Transform dest;
    private float size = 0.5f;
    public float rate = 0.05f;

    // Update is called once per frame
    void Update()
    {
        //size = Mathf.MoveTowards(size,0f,rate * 0.5f);
        //transform.localScale = new Vector3(size,size,size);
        
        float x = Mathf.Lerp(transform.position.x, dest.position.x, rate);
        float y = Mathf.Lerp(transform.position.y, dest.position.y, rate);
        transform.position = new Vector3(x,y,0f);

        if(size == 0f)
            Destroy(gameObject);
    }

    void End()
    {
        Destroy(gameObject);
    }
}