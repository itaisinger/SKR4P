using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSfx : MonoBehaviour
{
    public AudioSource jumpSfx;
    public AudioSource deathSfx;
    public AudioSource landSfx;

    //jump
    public void playJumpSfx()
    {
        jumpSfx.Play(0);
    }
    //death
    public void playdeathSfx()
    {
        deathSfx.Play(0);
    }
    //land
    public void playLandSfx()
    {
        landSfx.Play(0);
    }
}
