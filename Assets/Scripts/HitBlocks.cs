using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitBlocks : MonoBehaviour
{
    [SerializeField]
    private UnityEvent hit;

    private void OnCollisionEnter2D(Collision2D other) {
        var player = other.collider.GetComponent<PlayerMovement>();
        if (player && other.contacts[0].normal.y > 0) {
            hit.Invoke();
        }
    }
}
