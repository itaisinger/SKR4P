using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public float xmom, ymom;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 add = new Vector2(xmom,ymom,2);
        Vector2 pos = new Vector2(transform.position.x, transform.position.y) + add;
        Vector2 size = new Vector2(boxCollider.size.x, boxCollider.size.y);
        Debug.Log(OverlapBox(pos, size))
    }
}
