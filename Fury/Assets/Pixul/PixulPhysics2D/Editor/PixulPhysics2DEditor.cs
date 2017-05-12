using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PixulPhysics))]

public class PixelPhysics2DEditor : Editor
{
    PixulPhysics pp;
    int lineOptionsInt;
    string[] lineOptions = new string[] { "Solid Line", "Dotted Line" };
    SerializedProperty colourGradient;
    private void OnEnable()
    {
        pp = (PixulPhysics)target;
        lineOptionsInt = pp.enableSolidLine ? 0 : 1;
    }
    override public void OnInspectorGUI()
    {
        serializedObject.Update();

        colourGradient = serializedObject.FindProperty("lineColour");   
             
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Appearance", EditorStyles.boldLabel);
        lineOptionsInt = GUILayout.Toolbar(lineOptionsInt, lineOptions, EditorStyles.miniButton, GUILayout.Height(18));        

        EditorGUILayout.Space();
        pp.noOfTrajectoryPoints = EditorGUILayout.IntField(new GUIContent("No. of Points", "The number of points you want in your trajectory path (more equals smoother path)"), pp.noOfTrajectoryPoints);

        //specific properties for dotted line
        if (lineOptionsInt == 1)
        {
            pp.enableSolidLine = false;

            pp.trajectoryPointPrefab = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Trajectory Prefab", "The prefab you want to clone along your trajectory"), pp.trajectoryPointPrefab, typeof(GameObject), true);
        }

        //specific properties for solid line
        if(lineOptionsInt == 0)
        {
            pp.enableSolidLine = true;                        
            pp.cornerVerts = EditorGUILayout.IntField(new GUIContent("Corner Roundness", "Increase the number of verts at the corners of your trajectory"), pp.cornerVerts);
            pp.endVerts = EditorGUILayout.IntField(new GUIContent("End Roundness", "Increase roundness of the ends of your trajectory"), pp.endVerts);
            pp.widthCurve = EditorGUILayout.CurveField(new GUIContent("Width Curve", "Define the width of your trajectory"), pp.widthCurve, Color.red, new Rect(0, 0, 1, 1));
            EditorGUILayout.PropertyField(colourGradient, new GUIContent("Colour", "Define the colour gradient of your trajectory"));

            pp.lineMaterial = (Material)EditorGUILayout.ObjectField(new GUIContent("Material", "Apply a material to your trajectory"), pp.lineMaterial, typeof(Material), true);
            pp.textureMode = EditorGUILayout.Toggle(new GUIContent("Tiled", "Tile your material along your trajectory"), pp.textureMode == LineTextureMode.Tile) ? LineTextureMode.Tile : LineTextureMode.Stretch;

            if (pp.textureMode == LineTextureMode.Tile)
            {                               
                EditorGUILayout.HelpBox("Remember to use the Pixul shader located in the Shaders folder", MessageType.Info);
                pp.tileAmount = EditorGUILayout.Slider(new GUIContent("Tile Amount", "Set the tiling amount for your trajectory material"), pp.tileAmount, 0.1f, 10.0f);
            }                
        }

        pp.destroyAfterFire = EditorGUILayout.Toggle(new GUIContent("Destroy after firing", "Destroy your trajectory after firing"), pp.destroyAfterFire);
        if(pp.destroyAfterFire)
            pp.destroyDelay = EditorGUILayout.FloatField(new GUIContent("Destroy Delay", "Time to delay the destroy (seconds)"), pp.destroyDelay);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Physics Properties", EditorStyles.boldLabel);
        pp.m_gravity = EditorGUILayout.Vector2Field(new GUIContent("Gravity", "Controls the effect on the physics object during movement"), pp.m_gravity);
        pp.m_power = EditorGUILayout.FloatField(new GUIContent("Power", "Factor applied to the initial velocity"), pp.m_power);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Collision Properties", EditorStyles.boldLabel);
        pp.m_damping = EditorGUILayout.FloatField(new GUIContent("Damping", "Velocity will be factored by this if a collision occurs. Can be overridden using the collision properties script on the colliding object"), pp.m_damping);

        pp.m_ObstaclesLayerMask = EditorGUILayout.LayerField(new GUIContent("Obstacles Layer", "Layer that contains all the objects you wish the physics object to collide with"), pp.m_ObstaclesLayerMask);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(pp);
        }

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}
