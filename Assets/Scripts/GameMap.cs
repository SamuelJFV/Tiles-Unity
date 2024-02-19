using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap
{
    public string name;
    public GameObject map;
    public int width;
    public int height;
    public List<GameTile> gameTiles;
    public Mesh mesh;
    public PerlinMap perlinMap;

    public GameMap(string _name = "GameMap", int _width = 16, int _height=16)
    {
        name = _name;
        width = _width;
        height = _height;
        gameTiles = new List<GameTile>();

        mesh = new Mesh();
        mesh.vertices = CreateVertices();
        mesh.triangles = CreateTriangles();
    }
    public void Create()
    {
        if (map is null)
        {
            map = new GameObject(name);
        }

        for (int x = 0, y = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                CreateGameTile(x,y,z);
            }
        }
        
        map.AddComponent<MeshFilter>();
        map.AddComponent<MeshRenderer>();
        map.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void CreateGameTile(int x, int y, int z)
    {
        GameObject tile = new GameObject("tile");
        map.transform.parent = tile.transform;
    }

    private Vector3[] CreateVertices()
    {
        perlinMap = new PerlinMap(16,16,"123",4);
        Vector3[] vertices = new Vector3[16*16];

        for (int i = 0, x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                // if (perlinMap.samples[x,z] > .5f)
                // {
                //     vertices[i] = new Vector3(x, 0 , z);
                //     i++;
                // }
                i++;
            }
        }
        return vertices;
    }
    private int[] CreateTriangles()
    {
        int[] triangles = new int[]{};
        return triangles;
    }

    private void OnDrawGizmos()
    {
        if (mesh.vertices == null)
        {
            return;
        }
        foreach (var item in mesh.vertices)
            {
                Gizmos.DrawSphere(item,.1f);
            }
    }
}
