using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    //Grid Variables
    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private TileBase whiteTile;

    private PlaceableObject objectToPlace;
    public GameObject shipSpawnPoint;

    //Variables for UI
    public GameObject buildingPanel;
    public GameObject editingPanel;
    public GameObject arrowPanel;

    public GameObject currentShip;
    public Transform previousPos;


    //Saving ship pos variables
    public Dictionary<string, Vector3> allTiles = new Dictionary<string, Vector3>();
    List<Vector3> shipTiles = new List<Vector3>();
    public Dictionary<string, List<Vector3>> allShipPos = new Dictionary<string, List<Vector3>>();


    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();

        CreateAllPos();
        //foreach (var item in allTiles)
        //{
        //    Debug.Log("Key " + item.Key + " Value " + item.Value);
        //}
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
        if (CanBePlaced(objectToPlace))
        {
            objectToPlace.Place();
            Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
            TakeArea(start, objectToPlace.Size);

            //Saving pos to dictionary
            string shipName = RecordPlayerShipPos(objectToPlace.gameObject);
            //foreach (var item in shipTiles)
            //{
            //    Debug.Log(item);
            //}

            allShipPos.Add(shipName, shipTiles);
            //foreach (var item in allShipPos)
            //{
            //    Debug.Log("Key " + item.Key + " Value " + item.Value);
            //}
        }
        else
        {
            Destroy(objectToPlace.gameObject);
        }
        buildingPanel.SetActive(true);
        editingPanel.SetActive(false);
        arrowPanel.SetActive(false);
    }
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
        float zVal = 40;

        for (int i = 0; i < 25; i++)            //A1 through to E5. 25 values
        {
            Vector3 cornerPos = new Vector3(xVal, 0, zVal);
            allTiles.Add(columnLetter + (columnNumber.ToString()), cornerPos);

            columnNumber++;
            xVal += 10f;

            if (i == 4 || i == 9 || i == 14 || i == 19)
            {
                //Reseting variables for next line
                columnLetter = Convert.ToChar((Convert.ToUInt16(columnLetter) + 1));
                columnNumber = 1;
                zVal -= 10f;
                xVal = 0;
            }
        }
    }

    public string RecordPlayerShipPos(GameObject currentShip)
    {
        Transform ship = currentShip.transform;

        for (int i = 0; i < ship.childCount; i++)
        {
            if (ship.GetChild(i).gameObject.tag == "PrefabTilePoint")
            {
                shipTiles.Add(ship.GetChild(i).transform.position);
            }
        }
        string shipName = currentShip.gameObject.name.Replace("Prefab(Clone)", "");
        return shipName;
    }







    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit)) 
        { 
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    public void InitializeWithObject(GameObject ship)
    {
        Vector3 position = SnapCoordinateToGrid(shipSpawnPoint.transform.position);

        currentShip = Instantiate(ship, position, Quaternion.identity);
        objectToPlace = currentShip.GetComponent<PlaceableObject>();
        currentShip.AddComponent<ObjectDrag>();
    }

    private static TileBase[] GetTileBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach(var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    private bool CanBePlaced(PlaceableObject placeableObject) 
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        TileBase[] baseArray = GetTileBlock(area, mainTilemap);

        foreach (var b in baseArray)
        {
            if(b == whiteTile)
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        mainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.x, start.y + size.y);
    }
}
