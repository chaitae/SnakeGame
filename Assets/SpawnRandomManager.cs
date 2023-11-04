using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpawnRandomManager : MonoBehaviour
{
    [SerializeField]
    float maxDistanceFromTarget = 2f;
    [SerializeField]
    GameObject targetPosition;
    [SerializeField]
    GameObject spawnGameObject;
    public static SpawnRandomManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    [ContextMenu("SpawnrandoGO")]
    public void SpawnRandomGO()
    {
        float randomAngle = Random.Range(0, 2 * Mathf.PI);
        float randomX = Random.Range(0, maxDistanceFromTarget);
        float randomZ = randomX*Mathf.Sin(randomAngle);

        GameObject spawnGO = Instantiate(spawnGameObject);
        spawnGO.transform.position = targetPosition.transform.position + Vector3.right*randomX+Vector3.forward*randomZ;
    }


    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        float currRadian = 0;
        for(int i = 1; i<=20; i++)
        {
            currRadian +=  Mathf.PI/10;
            float y2 = maxDistanceFromTarget * Mathf.Sin(currRadian);
            float x2 = maxDistanceFromTarget*Mathf.Cos(currRadian);
            //connect with previous one

            float prevRadian = currRadian-Mathf.PI/10;
            float y1 = maxDistanceFromTarget * Mathf.Sin(prevRadian);
            float x1 = maxDistanceFromTarget * Mathf.Cos(prevRadian);

            Gizmos.DrawLine(new Vector3(x1, targetPosition.transform.position.y,y1), new Vector3 (x2, targetPosition.transform.position.y, y2));
        }

    }
}
