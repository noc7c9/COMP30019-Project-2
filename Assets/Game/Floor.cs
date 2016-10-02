using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

    public readonly int MAX_CORES = 20;

    Material material;

    void Start() {
        material = GetComponent<Renderer>().material;
        
        material.SetFloat("_AmbientCoeff", 1.0f);
        material.SetFloat("_DiffuseCoeff", 1.0f);
        material.SetFloat("_SpecularCoeff", 20.0f);
        material.SetFloat("_SpecularPower", 200.0f);
    }

    void Update() {
        GameObject[] cores = GameObject.FindGameObjectsWithTag("Core");
        int numCores = Mathf.Min(cores.Length, MAX_CORES);
        Vector4[] corePositions = new Vector4[MAX_CORES];
        Color[] coreColours = new Color[MAX_CORES];

        if (cores.Length > MAX_CORES) {
            Debug.Log("More cores than the maximum allowable limit");
        }

        for (int i = 0; i < numCores; i++) {
            corePositions[i] = cores[i].transform.position;
            coreColours[i] = cores[i].GetComponent<Renderer>().material.color;
        }
        
        material.SetInt("_NumCores", numCores);
        PassArrayToShader.Vector(material, "_CorePositions", corePositions);
        PassArrayToShader.Color(material, "_CoreColours", coreColours);
    }
}
