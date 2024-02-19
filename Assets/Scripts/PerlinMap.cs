using UnityEngine;
using System.Collections.Generic;

public class PerlinMap
{
    public int width;
    public int length;
    public string seed;
    public float frequency;
    public string islandFunc;
    public Vector2 offset;
    public int seedHashCode;
    private float Sqrt2 = Mathf.Sqrt(2);
    public PerlinMap(int _width = 16, int _length = 16, string _seed = "", float _frequency = 1.0f, string _islandFunc = null)
    {
        width = _width;
        length = _length;
        seed = _seed;
        frequency = _frequency;
        islandFunc = _islandFunc;
        
        if (seed != "")
        {
            seedHashCode = seed.GetHashCode();
            Random.InitState(seedHashCode);
        }
        
        offset = new Vector2(Random.Range(0.0f,1000.0f),Random.Range(0.0f,1000.0f));
    }

    public float CalculateSample(float x, float z)
    {
        float nx = 2.0f*x/width - 1.0f, nz =  2.0f*z/length - 1.0f;
        float elevation = Mathf.PerlinNoise(frequency*(nx+offset.x), frequency*(nz+offset.y));
        float distance;

        switch (islandFunc)
        {
            case "SquareBump":
            distance = 1 - (1-nx*nx) * (1-nz*nz);
            break;
            case "Euclidean":
            distance = Mathf.Min(1,nx*nx+nz*nz)/Sqrt2;
            break;
            default:
            return elevation;
        }
        return (elevation + 1.0f - distance)/2.0f;
    }
}
