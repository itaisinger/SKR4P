using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public int Deaths = 0;

    public void Increment()
    {
        Debug.Log("death inceremented");
        Deaths++;
    }
}