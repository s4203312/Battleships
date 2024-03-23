using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    void Start()
    {
        transform.Rotate(new Vector3(90, 0, 0));
    }
    void Update()
    {
        transform.position -= new Vector3(0, 0.05f, 0);
    }

    //Not registering a hit??
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        Destroy(gameObject);
    }
}
