using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestNPC : TemplateNPC
{
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();

        dialogue = new string[] {"Talk (E)", "Hello.", "My name is Jo Mama.", "It's so sad that Steve Jobs died of Ligma."};

        walkBounds = new float[] {0f, 5f, 27.5f, 32.5f};
    }
}