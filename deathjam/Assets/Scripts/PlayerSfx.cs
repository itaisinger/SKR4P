using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSfx : MonoBehaviour
{
    public AudioSource jumpSfx;
    //public AudioSource landSfx;

    //land
    //public void playLand()
    //{
        //landSfx.Play(0);
    //}

    //jump
    public void playJumpSfx()
    {
        jumpSfx.Play(0);
    }
}
