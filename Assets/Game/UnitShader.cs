using UnityEngine;
using System.Collections;

public class UnitShader : MonoBehaviour {
    public Texture tex;
    public Texture normalMap;
    public GameObject[] cores;

    // Use this for initialization
    void Start () {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_AmbientCoeff", 1.0f);
        renderer.material.SetFloat("_DiffuseCoeff", 1.0f);
        renderer.material.SetFloat("_SpecularCoeff", 0.3f);
        renderer.material.SetFloat("_SpecularPower", 200.0f);
        renderer.material.mainTexture = tex;
        renderer.material.SetTexture("_NormalMapTex", normalMap);

    }
    
    // Update is called once per frame
    void Update () {
        Vector3[] corePositions = new Vector3[this.cores.Length];
        Color[] coreColours = new Color[this.cores.Length];

        this.cores = GameObject.FindGameObjectsWithTag("Core");

        GetComponent<Renderer>().material.SetInt("_NumCores", this.cores.Length);

        for (int i = 0; i < cores.Length; i++) {
            corePositions[i] = cores[i].transform.position;
            coreColours[i] = cores[i].GetComponent<Renderer>().material.color;
        }


        PassArrayToShader.Vector3(GetComponent<Renderer>().material, "_CorePositions", corePositions);
        PassArrayToShader.Color(GetComponent<Renderer>().material, "_CoreColours", coreColours);
    }
}
