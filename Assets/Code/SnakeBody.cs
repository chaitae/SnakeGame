using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeBody : MonoBehaviour,PlayerCollidable
{
    static Mesh prevMesh;
    static Color prevColor;
    public Color[] colorPallet;
    public Mesh[] meshes;
    public void OnCollide(PlayerController playerController)
    {
        GameManager.instance.Death();
    }
    public void RandomizeShapeAndColorRotation()
    {
        int randomColorIndex = Random.Range(0, colorPallet.Length);
        int randomMeshIndex = Random.Range(0,meshes.Length);
        int colorRolledSame = 0;

        while(colorPallet[randomColorIndex] == prevColor && colorRolledSame < 4)
        {
            randomColorIndex = Random.Range(0, colorPallet.Length);
            colorRolledSame++;
        }
        //getmesh
        GetComponentInChildren<MeshFilter>().mesh = meshes[randomMeshIndex];
        GetComponentInChildren<Renderer>().material.color = colorPallet[randomColorIndex];
        transform.GetChild(0).GetComponent<Transform>().localRotation = Random.rotation;

        prevColor = colorPallet[randomColorIndex];
        prevMesh = meshes[randomMeshIndex];
    }
    // Start is called before the first frame update
    void Start()
    {
        RandomizeShapeAndColorRotation();
    }

}
