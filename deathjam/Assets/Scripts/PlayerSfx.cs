using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSfx : MonoBehaviour
{
    public AudioSource jumpSfx;
    //public AudioSource landSfx;


    //jump
    public void playJumpSfx()
    {
        jumpSfx.Play(0);
    }
}
