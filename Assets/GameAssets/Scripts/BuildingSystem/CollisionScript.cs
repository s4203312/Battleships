using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public Grid grid;
    private BuildingSystem buildingSystem;

    private void OnTriggerEnter()
    {
        buildingSystem = grid.GetComponent<BuildingSystem>();
        Debug.Log("Working");
        buildingSystem.currentShip.transform.position = buildingSystem.previousPos.position;
    }
}
