using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    ObjectGenerator needRestore;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
            needRestore.RestoreTrees();
        }
    }
}
