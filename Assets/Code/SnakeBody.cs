using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeBody : MonoBehaviour,PlayerCollidable
{
    //void Awake()
    //{
    //    GameManager.OnDeath += OnDeath;
    //}
    //void OnDeath() 
    //{ 
    //}
    public void OnCollide(PlayerController playerController)
    {
        playerController.GetComponent<Collider>().enabled = false;  
        GameManager.instance.Death();
    }
    public void SetColorandShape(Mesh mesh, Color color)
    {

        GetComponentInChildren<MeshFilter>().mesh = mesh;
        GetComponentInChildren<Renderer>().material.color = color;
        transform.GetChild(0).GetComponent<Transform>().localRotation = Random.rotation;
    }

}
