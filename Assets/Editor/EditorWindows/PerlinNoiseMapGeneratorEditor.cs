using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PerlinNoiseMapGenerator))]
public class PerlinNoiseMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PerlinNoiseMapGenerator gen = (PerlinNoiseMapGenerator) target;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Saving", EditorStyles.boldLabel);

        // should be divided by 100
        // but dividing by 107 to account for padding on sides of the editor window
        float w = Screen.width / 107f;
        string curr = GetNameOfCurrentlySelectedSettings(gen);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save as new", GUILayout.Width(w * 50))) { gen.SaveNewAsset(); }
        if (GUILayout.Button($"Override ({curr})", GUILayout.Width(w * 50))) { gen.OverrideSelectedAsset(); }
        EditorGUILayout.EndHorizontal();
    }

    private string GetNameOfCurrentlySelectedSettings(PerlinNoiseMapGenerator gen)
    {
        NoiseSetting _selected = gen.SelectedSettings;

        if (_selected == null) { return "Null"; }

        return _selected.ToString();
    }
}
