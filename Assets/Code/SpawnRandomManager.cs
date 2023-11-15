using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpawnRandomManager : MonoBehaviour
{
    [SerializeField]
    float maxDistanceFromTarget = 2f;
    [SerializeField]
    Vector3 targetPosition;
    [SerializeField]
    GameObject spawnGameObject;
    public static SpawnRandomManager instance;
    public GameObject foodGO;

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
        foodGO.SetActive(true);
    }
    private void Start()
    {
        SpawnRandomFood();

    }

    Vector3 GetRandomPosition()
    {
        float randomAngle = Random.Range(0, 2 * Mathf.PI);
        float randomDistance = Random.Range(0, maxDistanceFromTarget);
        float randomZ = randomDistance * Mathf.Sin(randomAngle);
        float randomX = randomDistance  * Mathf.Cos(randomAngle);
        Vector3 randomPos = this.gameObject.transform.position +
            Vector3.right * randomX + Vector3.forward * randomZ;

        return randomPos;
    }
    public void SpawnRandomFood()
    {

        if (foodGO == null)
        {
            //foodGO = Instantiate(spawnGameObject);
        }
        Vector3 randomPos = GetRandomPosition();
        while(Vector3.Distance(randomPos,PlayerController.instance.transform.position) <= 4f)
        {
            randomPos = GetRandomPosition();
        }
        foodGO.transform.position = new Vector3(randomPos.x, foodGO.transform.lossyScale.y, randomPos.z);
        foodGO.SetActive(true);
    }


    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        float currRadian = 0;
        for(int i = 1; i <= 20; i++)
        {
            currRadian +=  Mathf.PI/10;
            float y2 = targetPosition.z + maxDistanceFromTarget * Mathf.Sin(currRadian);
            float x2 = targetPosition.x + maxDistanceFromTarget * Mathf.Cos(currRadian);
            //connect with previous one

            float prevRadian = currRadian-Mathf.PI/10;
            float y1 = targetPosition.z + maxDistanceFromTarget * Mathf.Sin(prevRadian);
            float x1 = targetPosition.x + maxDistanceFromTarget * Mathf.Cos(prevRadian);

            Gizmos.DrawLine(new Vector3(x1, targetPosition.y,y1), new Vector3 (x2, targetPosition.y, y2));
        }

    }
}
