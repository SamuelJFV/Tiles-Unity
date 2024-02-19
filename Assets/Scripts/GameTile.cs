using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    public int id;
    public string biome;
    public float production;
    Material material;

    public void SetColor(Color color) 
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        material.color = color;
    }
}
