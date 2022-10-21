using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll) {
        var player = coll.GetComponent<PlayerMovement>();
        if (player != null) {
            player.SetRespawnPoint(transform.position);
        }
    }
}
