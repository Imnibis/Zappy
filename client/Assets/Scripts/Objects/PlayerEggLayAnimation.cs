using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerEggLayAnimation : MonoBehaviour
{
    public Transform fakeEgg;
    public ParticleSystem particleSystem;
    public Egg egg = null;
    public bool eggShouldAppear = false;

    Vector3 originalPosition;

    public void CheckIfEggShouldAppear()
    {
        if (eggShouldAppear)
            PlaceRealEgg();
    }

    public void PlaceRealEgg()
    {
        if (egg == null) {
            eggShouldAppear = true;
            return;
        }
        egg.Appear();
        eggShouldAppear = false;
        egg = null;
    }

    public void ActivateParticles()
    {
        particleSystem.Play();
    }
}
