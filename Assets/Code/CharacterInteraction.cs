using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    float hitDistance = 1.0f;
    private void FixedUpdate()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position - Vector3.up*transform.lossyScale.y/2, transform.TransformDirection(Vector3.forward), out hit, hitDistance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.magenta);
            //get the other hit item
            PlayerCollidable tempCollidable = hit.collider.GetComponentInChildren<PlayerCollidable>();
            if (tempCollidable != null)
            {
                tempCollidable.OnCollide(GetComponent<PlayerController>());
            }
        }
    }
}
