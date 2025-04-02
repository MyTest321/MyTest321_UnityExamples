using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SRP0103 {

[CustomEditor(typeof(SRP0103RPAsset))]
public sealed class SRP0103_Editor : Editor
{
    bool togglegroup1 = false;

    SerializedProperty m_drawOpaqueObjects;
    SerializedProperty m_drawSkyBox;
    SerializedProperty m_drawTransparentObjects;

    void OnEnable() {
        m_drawOpaqueObjects = serializedObject.FindProperty("drawOpaqueObjects");
        m_drawSkyBox = serializedObject.FindProperty("drawSkyBox");
        m_drawTransparentObjects = serializedObject.FindProperty("drawTransparentObjects");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.color = Color.cyan;
        GUILayout.Label ("SRP0103", EditorStyles.boldLabel);
        GUILayout.Label ("This pipeline shows how to create custom inspector for the RenderPipelineAsset", EditorStyles.wordWrappedLabel);
        GUI.color = Color.white;
        GUILayout.Space(15);

        togglegroup1 = EditorGUILayout.BeginToggleGroup ("Show Settings", togglegroup1);
            EditorGUI.indentLevel++;
            m_drawOpaqueObjects.boolValue = EditorGUILayout.Toggle("Draw Opaque Objects", m_drawOpaqueObjects.boolValue);
            m_drawSkyBox.boolValue = EditorGUILayout.Toggle("Draw SkyBox Objects", m_drawSkyBox.boolValue);
            m_drawTransparentObjects.boolValue = EditorGUILayout.Toggle("Draw Transparent Objects", m_drawTransparentObjects.boolValue);
            EditorGUI.indentLevel--;
        EditorGUILayout.EndToggleGroup();

        GUILayout.Space(15);

        serializedObject.ApplyModifiedProperties();
    }
}

}