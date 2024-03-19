using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
        Debug.Log("Rotation not working with current build");
        //Could rotate just the child points seperately by 90 then back to 0 for every other rotation???
    }
}
