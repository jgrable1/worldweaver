using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public GameObject Tree;
    public BasicItem Wood;
    public World World;
    TerrainData thisTerrain;
    TreeInstance[] originalTrees;
    void Start()
    {
        //The following code was inspired by user "hockenmaier" on the Unity forums. We have made modifications to the code from them but the main purpose of the code is the same.
        //A link to the discussion where this was posted is as follows: https://discussions.unity.com/t/solved-placing-trees-with-scripts-on-terrain/659480/16

        // Grab the island's terrain data
        thisTerrain = GetComponent<Terrain>().terrainData;

        //GameObject to be the parent
        GameObject treeParent = new GameObject("treeParent");

        originalTrees = thisTerrain.treeInstances;

        // For every terrain tree on the island
        foreach (TreeInstance terrainTree in thisTerrain.treeInstances)
        {
            // Find its local position scaled by the terrain size (to find the real world position)
            Vector3 worldTreePos = Vector3.Scale(terrainTree.position, thisTerrain.size) + Terrain.activeTerrain.transform.position;

            // Create the new tree out of the prefab
            GameObject prefabTree = Instantiate(Tree, worldTreePos, Quaternion.identity, treeParent.transform); // Create a prefab tree on its pos

            //Then set the new tree to the randomized size and rotation of the terrain tree
            prefabTree.transform.localScale = new Vector3(terrainTree.widthScale, terrainTree.heightScale, terrainTree.widthScale);
            prefabTree.transform.rotation = Quaternion.AngleAxis(terrainTree.rotation, Vector3.up);
            prefabTree.GetComponent<ResourceNode>().SetResource(Wood);
            prefabTree.GetComponent<ResourceNode>().SetWorld(World);
        }

        // Delete the current list of treeInstances, that way they don't overlap with the newly created objects
        thisTerrain.treeInstances = new TreeInstance[0];
    }

    void OnApplicationQuit() {
        RestoreTrees();
    }

    // restores the TerrainData's 
    public void RestoreTrees() {
        thisTerrain.treeInstances = originalTrees;
    }
}
