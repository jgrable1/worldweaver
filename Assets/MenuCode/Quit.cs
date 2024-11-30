using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    Terrain needRestore;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
            needRestore.GetComponent<ObjectGenerator>().RestoreTrees();
        }
    }
}
