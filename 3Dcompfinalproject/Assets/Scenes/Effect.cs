using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] particles;

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var particle in particles) { particle.Play(); }
    }

    public void magic_effect()
    {
        foreach (var particle in particles) { particle.Play(); }
        Camera.main.GetComponent<Animation>().Play();
    }

    public void animplay()
    {
        gameObject.GetComponent<Animation>().Play();
    }
}
