using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUse : MonoBehaviour
{

    [SerializeField]
    private World world;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetMouseButtonDown(0) && world.CanMove()){
            world.UseTool();
        }
    }
}
