using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour,PlayerCollidable
{
    public void OnCollide(PlayerController playerController)
    {
        playerController.GrowBody();
        gameObject.SetActive(false);
    }
}
