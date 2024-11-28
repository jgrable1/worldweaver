using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public GameObject Tree;
    // Start is called before the first frame update
    void Start()
    {
        //The following code was inspired by user "hockenmaier" on the Unity forums. We have made modifications to the code from them but the main purpose of the code is the same.
        //A link to the discussion where this was posted is as follows: https://discussions.unity.com/t/solved-placing-trees-with-scripts-on-terrain/659480/16

        // Grab the island's terrain data
        TerrainData thisTerrain;
        thisTerrain = GetComponent<Terrain>().terrainData;

        //GameObject to be the parent
        GameObject treeParent = new GameObject("treeParent");

        // For every terrain tree on the island
        foreach (TreeInstance terrainTree in thisTerrain.treeInstances)
        {
            // Find its local position scaled by the terrain size (to find the real world position)
            Vector3 worldTreePos = Vector3.Scale(terrainTree.position, thisTerrain.size) + Terrain.activeTerrain.transform.position;

            // Create the new tree out of the prefab
            GameObject prefabTree = Instantiate(Tree, worldTreePos, Quaternion.identity, treeParent.transform); // Create a prefab tree on its pos

            //Then set the new tree to the randomized size and rotation of the terrain tree
            prefabTree.transform.localScale = new Vector3(terrainTree.widthScale * 50, terrainTree.heightScale * 50, terrainTree.widthScale * 50);
            prefabTree.transform.rotation = Quaternion.AngleAxis(terrainTree.rotation * 57.2958f, Vector3.up);
        }
        // Then delete all the terrain trees on the island
        List<TreeInstance> newTrees = new List<TreeInstance>(0);
        thisTerrain.treeInstances = newTrees.ToArray();
    }
}
