using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public GameObject graphics;
    public Player layingPlayer;
    public ParticleSystem hatchParticles;

    public void Appear()
    {
        graphics.SetActive(true);
    }

    public void Hatch()
    {
        hatchParticles.Play();
    }

    public void StopHatching()
    {
        hatchParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        graphics.SetActive(false);
    }
}
