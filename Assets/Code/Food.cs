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
    Mesh generatedMesh;
    Color generatedColor;
    BoxCollider boxCollider;
    AudioSource audioSource;
    MeshFilter meshFilter;
    Renderer currRenderer;
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
        meshFilter = GetComponentInChildren<MeshFilter>();
        currRenderer = GetComponent<Renderer>();
        RandomizeShapeAndColorRotation();

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
        meshFilter.mesh = meshes[randomMeshIndex];
        currRenderer.material.color = colorPallet[randomColorIndex];
        generatedMesh = meshes[randomMeshIndex];
        generatedColor = colorPallet[randomColorIndex];

        prevColor = colorPallet[randomColorIndex];
        prevMesh = meshes[randomMeshIndex];
    }
    public void OnCollide(PlayerController playerController)
    {
        audioSource.Play();
        boxCollider.enabled = false;
        playerController.GrowBody(generatedMesh,generatedColor);
        GameManager.instance.AddScore(1);
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
