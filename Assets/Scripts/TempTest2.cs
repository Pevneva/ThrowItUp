using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTest2 : MonoBehaviour
{
    void Start()
    {
        var bounds = GetComponent<MeshFilter>().sharedMesh.bounds;
        Debug.Log("bounds : " + bounds);
    }
}
