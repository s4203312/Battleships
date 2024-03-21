using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static GamePlayManager managerScript;

    public static BuildingSystem current;
    private PlaceableObject objectToPlace;
    public GameObject shipSpawnPoint;

    //Variables for UI
    public GameObject buildingPanel;
    public GameObject editingPanel;
    public GameObject arrowPanel;
    private GameObject currentButton;

    [HideInInspector] public int currentShipPlaced = 0;

    [HideInInspector] public GameObject currentShip;
    [HideInInspector] public Transform previousPos;


    //Saving ship pos variables
    [HideInInspector] public Dictionary<string, Vector3> allTiles = new Dictionary<string, Vector3>();
    List<Vector3> shipTiles = new List<Vector3>();
    private string shipName;
    [HideInInspector] public Dictionary<string, List<Vector3>> allShipPos = new Dictionary<string, List<Vector3>>();


    private void Awake()
    {
        current = this;
        managerScript = this.GetComponent<GamePlayManager>();

        CreateAllPos();
        foreach (var item in allTiles)
        {
            Debug.Log("Key " + item.Key + " Value " + item.Value);
        }
    }

    //Button functions for editing options in build mode ******Old******
    public void BeginPlaceShip(GameObject shipPrefab)
    {
        InitializeWithObject(shipPrefab);
        buildingPanel.SetActive(false);
        editingPanel.SetActive(true);

        arrowPanel.SetActive(true);
    }
    public void ConfirmPlaceShip()
    {
        if (CheckPlayerShipPos())
        {
            //Reseting the list and string for new entry
            shipTiles = new List<Vector3>();
            shipName = "";

            //Saving pos to dictionary
            RecordPlayerShipPos(objectToPlace.gameObject);
            Debug.Log(shipName);
            foreach (var item in shipTiles)
            {
                Debug.Log(item);
            }

            allShipPos.Add(shipName, shipTiles);
            foreach (var item in allShipPos)
            {
                Debug.Log("Key " + item.Key);
                foreach (var item2 in item.Value)
                {
                    Debug.Log("Value " + item2);
                }
            }

            //Removing ship from UI and telling game its been placed
            currentButton.SetActive(false);
            currentShipPlaced++;
        }
        else
        {
            Destroy(objectToPlace.gameObject);
        }

        buildingPanel.SetActive(true);
        editingPanel.SetActive(false);
        arrowPanel.SetActive(false);
    }

    //Not work like i want it too
    public void RotatePlaceShip()
    {
        objectToPlace.Rotate();
    }

    public void CancelPlaceShip()
    {
        Destroy(objectToPlace.gameObject);
        buildingPanel.SetActive(true);
        editingPanel.SetActive(false);


        arrowPanel.SetActive(false);
    }
    
    //Storing a button variable to disable a choice
    public void StoreWhichButton(GameObject button)
    {
        currentButton = button;
    }

    //Arrow Method
    public void MoveLeft()
    {
        int counter = BoundsCheck(1);
        if (counter == 1)
        {
            previousPos = currentShip.transform;
            currentShip.transform.position = currentShip.transform.position + new Vector3(-10, 0, 0);
        }
    }
    public void MoveRight()
    {
        int counter = BoundsCheck(2);
        if (counter == 1)
        {
            previousPos = currentShip.transform;
            currentShip.transform.position = currentShip.transform.position + new Vector3(10, 0, 0);
        }
    }
    public void MoveUp()
    {
        int counter = BoundsCheck(3);
        if (counter == 1)
        {
            previousPos = currentShip.transform;
            currentShip.transform.position = currentShip.transform.position + new Vector3(0, 0, 10);
        }
    }
    public void MoveDown()
    {
        int counter = BoundsCheck(4);
        if (counter == 1)
        {
            previousPos = currentShip.transform;
            currentShip.transform.position = currentShip.transform.position + new Vector3(0, 0, -10);
        }
    }

    //1,2 are left and right. 3,4 are up and down
    public int BoundsCheck(int direction)
    {
        int counter = 0;
        Vector3 inBoundsVec = new Vector3(0, 0, 0) + currentShip.transform.position;

        if (direction == 1)
        {
            if (inBoundsVec[0] >= 0f) //Check on X value
            {
                counter++;
            }
        }
        if (direction == 2)
        {
            if (inBoundsVec[0] <= 100f) //Check on X value
            {
                counter++;
            }
        }
        if (direction == 3)
        {
            if (inBoundsVec[2] <= 100f) //Check on Z value
            {
                counter++;
            }
        }
        if (direction == 4)
        {
            if (inBoundsVec[2] >= 0f) //Check on Z value
            {
                counter++;
            }
        }
        return counter;
    }

    public void CreateAllPos()
    {
        char columnLetter = 'A';
        int columnNumber = 1;

        float xVal = 0;
        float zVal = 90;

        for (int i = 0; i < 100; i++)            //A1 through to J10. 100 values
        {
            Vector3 cornerPos = new Vector3(xVal, 0, zVal);
            allTiles.Add(columnLetter + (columnNumber.ToString()), cornerPos);

            columnNumber++;
            xVal += 10f;

            if (i == 9 || i == 19 || i == 29 || i == 39 || i == 49 || i == 59 || i == 69 || i == 79 || i == 89 || i == 99)
            {
                //Reseting variables for next line
                columnLetter = Convert.ToChar((Convert.ToUInt16(columnLetter) + 1));
                columnNumber = 1;
                zVal -= 10f;
                xVal = 0;
            }
        }
    }

    public void RecordPlayerShipPos(GameObject currentShip)
    {
        Transform ship = currentShip.transform;

        for (int i = 0; i < ship.childCount; i++)
        {
            if (ship.GetChild(i).gameObject.tag == "PrefabTilePoint")
            {
                shipTiles.Add(ship.GetChild(i).transform.position);
            }
        }
        shipName = currentShip.gameObject.name.Replace("Prefab(Clone)", "");
    }

    public bool CheckPlayerShipPos()
    {
        Transform ship = currentShip.transform;

        for (int i = 0; i < ship.childCount; i++)
        {
            if (ship.GetChild(i).gameObject.tag == "PrefabTilePoint")
            {
                foreach(Vector3 usedPos in shipTiles)
                {
                    if(ship.GetChild(i).transform.position == usedPos)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private void Update()
    {
        if(currentShipPlaced == 5)
        {
            managerScript.AttackMode();
            currentShipPlaced = 0;
        }
    }


    public void InitializeWithObject(GameObject ship)
    {
        Vector3 position = shipSpawnPoint.transform.position;

        currentShip = Instantiate(ship, position, Quaternion.identity);
        objectToPlace = currentShip.GetComponent<PlaceableObject>();
    }
}
