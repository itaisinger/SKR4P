using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private float TargetBottom;
    [SerializeField] private float rate;

    private RectTransform trans;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<RectTransform>();
        setBottom(-200f);
    }

    // Update is called once per frame
    void Update()
    {   
        setBottom(Mathf.Lerp(trans.offsetMin.y, TargetBottom, rate));
        setBottom(Mathf.MoveTowards(trans.offsetMin.y, TargetBottom, 0.1f));
    }

    public void TransitionOut()
    {
        Debug.Log("transitioning out");
        TargetBottom = -100f;
    }

    public bool IsDone()
    {
        Debug.Log(trans.offsetMin.y - TargetBottom);
        return Mathf.Abs(trans.offsetMin.y - TargetBottom) < 5f;
    }

    private void setBottom(float bottom)
    {
        trans.offsetMin = new Vector2(trans.offsetMin.x,bottom); 
    }
}
