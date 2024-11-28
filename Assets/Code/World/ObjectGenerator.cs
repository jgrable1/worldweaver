using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public GameObject ObjectToCreate;
    public int seed;
    public int numToPlace;
    // Start is called before the first frame update
    void Start()
    {
        //The following code was inspired by user "hockenmaier" on the Unity forums. We have made modifications to it that have changed the fundamentals of what hockenmaier was doing.
        //A link to the discussion where this was posted is as follows: https://discussions.unity.com/t/solved-placing-trees-with-scripts-on-terrain/659480/16

        System.Random random = new System.Random(seed);

        TerrainData thisTerrain = GetComponent<Terrain>().terrainData;

        //GameObject to be the parent
        GameObject treeParent = new GameObject("treeParent");
        RaycastHit hit;

        // For every terrain tree on the island
        for (int i = 0; i < numToPlace; i++) {
            Vector3 randomPoint = new Vector3(random.Next(0, 1000), 250, random.Next(0, 1000));
            Physics.Raycast(randomPoint, -Vector3.up, out hit);

            // Create the new tree out of the prefab
            GameObject prefabTree = Instantiate(ObjectToCreate, hit.point, Quaternion.identity, treeParent.transform); // Create a prefab tree on its pos

            //Then set the new tree to the randomized size and rotation of the terrain tree
            prefabTree.transform.rotation = Quaternion.AngleAxis(random.Next(0, 360), Vector3.up);
        }
    }
}
