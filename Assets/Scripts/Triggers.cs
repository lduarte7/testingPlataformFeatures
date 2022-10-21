using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : MonoBehaviour
{
  void OnTriggerEnter2D(Collider2D coll) {
    if (coll.gameObject.tag == "Item") 
    {
      Debug.Log("Item collected!");
      Destroy(coll.gameObject);
    }


    var player = GetComponent<PlayerMovement>();
    if (coll.gameObject.tag == "Traps") {
      Debug.Log("You died!");
      player.Die();
    }
  }

}
