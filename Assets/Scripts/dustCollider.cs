using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dustCollider : MonoBehaviour
{
    public ParticleSystem dust;

    void CreateDust()
    {
        dust.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ground")
        {
            CreateDust();
        }
    }
}
