using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]       
[RequireComponent(typeof(MeshRenderer))]       
public class GameTileRenderer : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Material material;
    private float sqrt3 = Mathf.Sqrt(3);
    private void Awake() 
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = new Mesh();
        MeshData meshData = new MeshData();
        meshFilter.mesh.vertices = meshData.vertices.ToArray();
        meshFilter.mesh.triangles = meshData.triangles.ToArray();
        meshFilter.mesh.uv = meshData.uv.ToArray();
        meshFilter.mesh.name = "Hex";
        //meshRenderer.material = material;
    }

    public void DrawMesh()
    {
        //CreateTriangles();
    }
    void CreateTriangles()
    {
        int[] triangles =  new int[]
        {
            0,6,7,
            7,1,0,
            1,7,8,
            8,2,1,
            2,8,9,
            9,3,2,
            3,9,10,
            10,4,3,
            4,10,11,
            11,5,4,
            5,11,0,
            0,11,6
        };
    }
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

            AddTopFace();
            AddSideFaces(_faces);
        }
        private void AddTopFace()
        {
            vertices = new List<Vector3>()
            {
                new Vector3(0.0f, 0.5f, 1.0f),
                new Vector3(Sqrt3/2, 0.5f, 0.5f),
                new Vector3(Sqrt3/2, 0.5f, -0.5f),
                new Vector3(0.0f, 0.5f, -1.0f),
                new Vector3(-Sqrt3/2, 0.5f, -0.5f),
                new Vector3(-Sqrt3/2, 0.5f, 0.5f)
            };
            
            triangles =  new List<int>()
            {
                0,1,5,
                1,2,5,
                2,4,5,
                2,3,4,
            };
        }

        private void AddSideFaces(bool[] faces)
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
}
