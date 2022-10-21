using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticle : MonoBehaviour
{
    [SerializeField] GameObject particle;

    public void Spawn() {
        Instantiate(particle, transform.position, Quaternion.identity);
    }
}
