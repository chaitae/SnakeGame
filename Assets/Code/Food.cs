using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Food : MonoBehaviour,PlayerCollidable
{
    static Mesh prevMesh;
    static Color prevColor;
    public Color[] colorPallet;
    public Mesh[] meshes;
    Mesh mesh;
    Color color;
    BoxCollider boxCollider;
    AudioSource audioSource;
    void Awake()
    {
        RandomizeShapeAndColorRotation();
        boxCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }
    public void RandomizeShapeAndColorRotation()
    {
        int randomColorIndex = Random.Range(0, colorPallet.Length);
        int randomMeshIndex = Random.Range(0, meshes.Length);
        int colorRolledSame = 0;

        while (colorPallet[randomColorIndex] == prevColor && colorRolledSame < 4)
        {
            randomColorIndex = Random.Range(0, colorPallet.Length);
            colorRolledSame++;
        }
        //getmesh
        GetComponentInChildren<MeshFilter>().mesh = meshes[randomMeshIndex];
        GetComponentInChildren<Renderer>().material.color = colorPallet[randomColorIndex];
        mesh = meshes[randomMeshIndex];
        color = colorPallet[randomColorIndex];

        prevColor = colorPallet[randomColorIndex];
        prevMesh = meshes[randomMeshIndex];
    }
    public void OnCollide(PlayerController playerController)
    {
        audioSource.Play();
        boxCollider.enabled = false;
        playerController.GrowBody(mesh,color);
        GameManager.instance.AddScore(1);
        //GameObject.Destroy(gameObject, .3f);
        StartCoroutine(HideSelf());
    }
    IEnumerator HideSelf()
    {
        yield return new WaitForSeconds(.3f);
        RandomizeShapeAndColorRotation();
        SpawnRandomManager.instance.SpawnRandomFood();
        boxCollider.enabled = true;

    }
}
