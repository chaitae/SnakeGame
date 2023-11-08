using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, PlayerCollidable
{
    public void OnCollide(PlayerController playerController)
    {
        GameManager.instance.Death();
    }
}
