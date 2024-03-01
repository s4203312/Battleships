using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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


    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
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
        currentShip.transform.position = currentShip.transform.position + new Vector3(-10, 0, 0);
    }
    public void MoveRight()
    {
        currentShip.transform.position = currentShip.transform.position + new Vector3(10, 0, 0);
    }
    public void MoveUp()
    {
        currentShip.transform.position = currentShip.transform.position + new Vector3(0, 0, 10);
    }
    public void MoveDown()
    {
        currentShip.transform.position = currentShip.transform.position + new Vector3(0, 0, -10);
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
