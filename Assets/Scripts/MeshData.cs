using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uv;
    private float Sqrt3 = Mathf.Sqrt(3);
    public MeshData(bool[] _faces = null)
    {
        if (_faces is null)
        {
            _faces = new bool[]{true,false,false,false,false,true};
        }
        vertices = new List<Vector3>();
        triangles =  new List<int>();
    }
    public void AddTopFace(Vector3 position)
    {
        triangles.Add(0+vertices.Count);
        triangles.Add(1+vertices.Count);
        triangles.Add(5+vertices.Count);
        triangles.Add(1+vertices.Count);
        triangles.Add(2+vertices.Count);
        triangles.Add(5+vertices.Count);
        triangles.Add(2+vertices.Count);
        triangles.Add(4+vertices.Count);
        triangles.Add(5+vertices.Count);
        triangles.Add(2+vertices.Count);
        triangles.Add(3+vertices.Count);
        triangles.Add(4+vertices.Count);

        vertices.Add(new Vector3(0.0f, 0.5f, 1.0f) + position);
        vertices.Add(new Vector3(Sqrt3/2, 0.5f, 0.5f) + position);
        vertices.Add(new Vector3(Sqrt3/2, 0.5f, -0.5f) + position);
        vertices.Add(new Vector3(0.0f, 0.5f, -1.0f) + position);
        vertices.Add(new Vector3(-Sqrt3/2, 0.5f, -0.5f) + position);
        vertices.Add(new Vector3(-Sqrt3/2, 0.5f, 0.5f) + position);
    }

    public void AddSideFaces(Vector3 position, bool[] faces)
    {
        for (int i = 0; i < faces.Length; i++)
        {
            if (faces[i])
            {
                Vector3 vertex = vertices[i];
                vertex.y *= -1;
                vertices.Add(vertex);
            }
        }

        for (int i = 0, j = 0; i < faces.Length; i++)
        {
            if (faces[i])
            {
                int jBack = j-1;
                if (jBack < 0) {jBack += faces.Length;}

                triangles.Add(j);
                triangles.Add(jBack);
                triangles.Add(j+6);
                j++;
            }
        }
    }
}
