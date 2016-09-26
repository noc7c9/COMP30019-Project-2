using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {
    public GameObject[] cores;

    public readonly int MAX_CORES = 20;

    // Use this for initialization
    void Start() {

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_AmbientCoeff", 1.0f);
        renderer.material.SetFloat("_DiffuseCoeff", 1.0f);
        renderer.material.SetFloat("_SpecularCoeff", 20.0f);
        renderer.material.SetFloat("_SpecularPower", 200.0f);
    }

    // Update is called once per frame
    void Update() {
        Vector3[] corePositions = new Vector3[this.cores.Length];
        Color[] coreColours = new Color[this.cores.Length];

        this.cores = GameObject.FindGameObjectsWithTag("Core");

        if (cores.Length > MAX_CORES) {
            Debug.Log("More cores than the maximum allowable limit");
        }
        else {
            GetComponent<Renderer>().material.SetInt("_NumCores", this.cores.Length);
            //Debug.Log("num cores = " + this.cores.Length);

            for (int i = 0; i < cores.Length; i++) {
                corePositions[i] = cores[i].transform.position;
                coreColours[i] = cores[i].GetComponent<Renderer>().material.color;
            }
        }


        PassArrayToShader.Vector3(GetComponent<Renderer>().material, "_CorePositions", corePositions);
        PassArrayToShader.Color(GetComponent<Renderer>().material, "_CoreColours", coreColours);
    }
}
