using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static BuildingSystem buildingScript;
    public static SelectingChoice playerchoiceScript;
    public GameObject refGameObject;

    //Variables for UI
    public GameObject attckingPanel;

    void Awake()
    {
        buildingScript = refGameObject.GetComponent<BuildingSystem>();
        playerchoiceScript = refGameObject.GetComponent<SelectingChoice>();
    }

    public void AttackMode()
    {
        if (buildingScript.currentShipPlaced == 5)
        {
            attckingPanel.SetActive(true);
        }
    }
}
