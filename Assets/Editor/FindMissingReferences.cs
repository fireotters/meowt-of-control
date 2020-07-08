﻿using UnityEditor;
using UnityEngine;

// Finds any missing references in a scene from the Tools menu in Unity
// Written by Lior Tal of Gamasutra https://www.gamasutra.com/blogs/LiorTal/20141208/231690/Finding_Missing_Object_References_in_Unity.php

public class FindMissingReferencesInScene : MonoBehaviour
{
    [MenuItem("Tools/Find Missing references in scene")]
    public static void FindMissingReferences()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (var go in objects)
        {
            var components = go.GetComponents<Component>();

            foreach (var c in components)
            {
                if (c == null)
                {
                    Debug.LogError("Missing script found on: " + FullObjectPath(go));
                }
                else
                {
                    SerializedObject so = new SerializedObject(c);
                    var sp = so.GetIterator();

                    while (sp.NextVisible(true))
                    {
                        if (sp.propertyType != SerializedPropertyType.ObjectReference)
                        {
                            continue;
                        }

                        if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
                        {
                            ShowError(FullObjectPath(go), sp.name);
                        }
                    }
                }
            }
        }
    }

    private static void ShowError(string objectName, string propertyName)
    {
        Debug.LogError("Missing reference found in: " + objectName + ", Property : " + propertyName);
    }

    private static string FullObjectPath(GameObject go)
    {
        return go.transform.parent == null ? go.name : FullObjectPath(go.transform.parent.gameObject) + "/" + go.name;
    }
}