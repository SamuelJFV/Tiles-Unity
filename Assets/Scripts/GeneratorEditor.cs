using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(Generator))]
public class GeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Generator terrain = (Generator)target;
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Terrain"))
        {
            terrain.DestroyTerrain();
            terrain.BuildTerrain("Terrain");
        }
        if (GUILayout.Button("Clear"))
        {
            terrain.DestroyTerrain();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Height"))
        {
            terrain.DestroyTerrain();
            terrain.BuildTerrain("Height");
        }
        if (GUILayout.Button("Moisture"))
        {
            terrain.DestroyTerrain();
            terrain.BuildTerrain("Moisture");
        }
        if (GUILayout.Button("Temperature"))
        {
            terrain.DestroyTerrain();
            terrain.BuildTerrain("Temperature");
        }
        GUILayout.EndHorizontal();
    }
}
