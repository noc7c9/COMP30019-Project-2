using UnityEngine;
using System.Collections;

// Note we're exploiting some sort of Unity "magic" whereby by appending an integer
// to a uniform array name selects the specific array element based on that integer.
//
// For more info: http://www.alanzucconi.com/2016/01/27/arrays-shaders-heatmaps-in-unity3d/
//
// source: COMP30019 Graphics and Interation: Lab 7
public class PassArrayToShader : MonoBehaviour {

    public static void Vector(Material material, string name, Vector4[] array) {
        #if UNITY_5_4_OR_NEWER

        material.SetVectorArray(name, array);

        #else

        for (int i = 0; i < array.Length; i++)
            material.SetVector(name + i.ToString(), array[i]);

        #endif
    }

    public static void Color(Material material, string name, Color[] array) {
        #if UNITY_5_4_OR_NEWER

        material.SetColorArray(name, array);

        #else

        for (int i = 0; i < array.Length; i++)
            material.SetColor(name + i.ToString(), array[i]);

        #endif
    }

}
