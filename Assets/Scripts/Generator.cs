using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]       
[RequireComponent(typeof(MeshRenderer))]       
public class Generator : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width = 16;
    public int length = 16;
    float tileHeight = 0.5f;

    [Range(3.0f, 6.0f)]
    public int maxHeight = 5;
    [Range(0.0f, 1.0f)]
    public float moistureLevel = 0.5f;
    [Range(0.0f, 1.0f)]
    public float polesTemperatureLevel = 0.1f;
    [Range(0.0f, 1.0f)]
    public float waterLevel = 1.0f;
    [Range(0.0f, 1.0f)]
    public float mountainLevel = 1.0f;
    [Range(0.0f, 10.0f)]
    public float frequency = 1.0f;
    public string islandFunc = "SquareBump";
    public string seed = "";
    PerlinMap heightMap;
    PerlinMap moistureMap;
    float Sqrt3 = Mathf.Sqrt(3);
    public void BuildTerrain(string type)
    {
        heightMap = new PerlinMap(width, length, seed, frequency, islandFunc);
        moistureMap = new PerlinMap(width, length, seed);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                float heightSample = heightMap.CalculateSample(x,z);
                float moistureSample = Cap(moistureLevel+moistureMap.CalculateSample(x,z)-0.5f);
                float temperatureSample = TemperatureMap(z, heightSample);
                Vector3 tilePosition = SetPosition(x,z,heightSample);
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity, gameObject.transform);
                GameTile gameTile = tile.GetComponent<GameTile>();

                Color color;

                switch (type)
                {
                    case "Terrain":
                    color = SetBiome(heightSample, moistureSample, temperatureSample);
                    break;
                    case "Height":
                    color = new Color(1.0f-heightSample, 1.0f-heightSample, 1.0f-heightSample);
                    break;
                    case "Moisture":
                    color = new Color(1.0f-moistureSample, 1.0f-moistureSample, moistureSample);
                    break;
                    case "Temperature":
                    color = new Color(1.0f-temperatureSample, 1.0f, 1.0f-temperatureSample);
                    break;
                    default:
                    color = SetBiome(heightSample, moistureSample, temperatureSample);
                    break;
                }

                gameTile.SetColor(color);
            }
        }
    }
    public void DestroyTerrain()
    {
        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
        }
    }
    public float Cap(float value)
    {
        if (value > 1.0f)
        {
            return 1.0f;
        }
        if (value <0.0f)
        {
            return 0.0f;
        }
        return value;
    }
    public float TemperatureMap(float z, float y)
    {
        z/=length;
        return (polesTemperatureLevel-1)*4.0f*(z-0.5f)*(z-0.5f)+1.0f;
    }
    public Vector3 SetPosition(int x, int z, float sample)
    {
        float y = Redestribution(sample);
        Vector3 tilePosition;

        if (z%2 == 0)
        {
            tilePosition = new Vector3(x*Sqrt3+Sqrt3/2.0f, y, z*1.5f);
        }
        else
        {
            tilePosition = new Vector3(x*Sqrt3, y, z*1.5f);
        }

        tilePosition -= new Vector3(Sqrt3*width/2.0f, 0, 1.5f*length/2.0f);
        return tilePosition;
    }
    public Color CreateColor(float r, float g, float b, float sprinkle = 0.05f)
    {
        Color randomColor = new Color(sprinkle*Random.value, sprinkle*Random.value, sprinkle*Random.value);
        return new Color(r, g, b) + randomColor;
    }
    public Color SetBiome(float heightSample, float moistureSample, float temperatureSample)
    {
        Color color;

        if (heightSample <= waterLevel-.1f)
        {
            color = CreateColor(0.0f, 0.0f, heightSample);
        }
        else if (heightSample <= waterLevel)
        {
            color = CreateColor(0.1f*heightSample, 0.1f*heightSample, heightSample);
        }
        else if (heightSample <= waterLevel+.05f)
        {
            color = CreateColor(0.75f, 0.65f, 0.40f);
        }

        else if (heightSample <= 1-mountainLevel) //Grass
        { 
            color = CreateColor(0.0f, moistureSample*(1.0f-heightSample*0.9f), 0.0f) + (1.0f-moistureSample)*CreateColor(0.75f, 0.65f, 0.40f);
        }
        else
        {
            float samplePow4 = heightSample*heightSample*heightSample*heightSample*(mountainLevel*2.0f);
            color = CreateColor(samplePow4, samplePow4, samplePow4);
        }

        color = temperatureSample*color+ new Color(1.0f-temperatureSample, 1.0f-temperatureSample, 1.0f-temperatureSample);

        return color;
    }
    public float Terrace(float value)
    {
        return tileHeight*Mathf.Ceil(MountainFunction(value)/tileHeight);
    }
    public float MountainFunction(float value)
    {
        return value*((maxHeight-tileHeight)/mountainLevel)+(maxHeight+((tileHeight-maxHeight)/mountainLevel));
    }
    public float ElevationToPow(float value, float exponent)
    {
        return Mathf.Pow(value, exponent);
    }
    public float Redestribution(float value)
    {
        if (value < waterLevel)
        {
            return tileHeight/2;
        }
        else if (value < 1-mountainLevel)
        {
            return tileHeight;
        }

        return Terrace(value);
    }
    public float ridgenoise(float value) 
    {
        return 2.0f*(0.5f - Mathf.Abs(0.5f - value));
    }
}