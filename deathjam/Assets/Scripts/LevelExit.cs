using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private AudioSource sfx;
    private Transition transition;
    private bool endingLevel = false;

    private void Start()
    {
        sfx = GetComponent<AudioSource>();
        transition = GameObject.FindWithTag("Transition").GetComponent<Transition>();
    }

    private void Update()
    {
        if(endingLevel)
        {
            if(transition.IsDone())
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            endingLevel = true;
            transition.TransitionOut();
            sfx.Play();
        }
    }
}