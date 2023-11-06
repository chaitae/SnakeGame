using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour,PlayerCollidable
{
    public void OnCollide(PlayerController playerController)
    {
        GetComponent<BoxCollider>().enabled = false;
        playerController.GrowBody();
        GameObject.Destroy(gameObject, .3f);
        SpawnRandomManager.instance.SpawnRandomGO();
    }
}
