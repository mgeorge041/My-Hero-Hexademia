using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class TestFunctions
{
    // Create class object for testing
    public static T CreateClassObject<T>(string assetPath)
    {
        Object newObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        GameObject newGameObject = (GameObject)Object.Instantiate(newObject);
        T newT = newGameObject.GetComponentInChildren<T>();
        return newT;
    }
}
