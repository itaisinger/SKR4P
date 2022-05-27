using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private float TargetBottom;
    [SerializeField] private float TargetTop;
    [SerializeField] private float rate;

    private RectTransform trans;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<RectTransform>();
        setBottom(-200f);
        //TargetBottom = -200f;
        //setTop(TargetBottom);
    }

    // Update is called once per frame
    void Update()
    {   
        //set bottom
        setBottom(Mathf.Lerp(trans.offsetMin.y, TargetBottom, rate));
        setBottom(Mathf.MoveTowards(trans.offsetMin.y, TargetBottom, 0.1f));

        //set top
        //setTop(Mathf.Lerp(trans.offsetMin.x, TargetTop, rate));
        //setTop(Mathf.MoveTowards(trans.offsetMin.x, TargetTop, 0.1f));

        Debug.Log(trans.offsetMin.y);
    }

    public void TransitionOut()
    {
        Debug.Log("transitioning out");
        //TargetTop = -100f;
        //setBottom(-50f);
        TargetBottom = -50f;
    }

    public bool IsDone()
    {
        Debug.Log(trans.offsetMin.y - TargetBottom);
        return Mathf.Abs(trans.offsetMin.y - TargetBottom) < 5f;
    }

    //set borders
    private void setBottom(float bottom)
    {
        trans.offsetMin = new Vector2(trans.offsetMin.x,bottom); 
    }
    private void setTop(float top)
    {
        trans.offsetMax = new Vector2(trans.offsetMax.x, top); 
    }
}
