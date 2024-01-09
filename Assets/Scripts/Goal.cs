using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    
    Material mat;
    
    Color col;
    Color emitCol;
    
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        col = mat.GetColor("_Color");
        emitCol = mat.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            mat.SetColor("_Color", Color.green);
            mat.SetColor("_EmissionColor", Color.green);
        }
    }
    
    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            mat.SetColor("_Color", col);
            mat.SetColor("_EmissionColor", emitCol);
        }
    }
}
