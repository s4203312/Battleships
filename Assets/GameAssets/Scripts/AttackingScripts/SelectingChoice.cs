using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectingChoice : MonoBehaviour
{
    public static BuildingSystem buildingScript;

    [HideInInspector] public Dictionary<string, Vector3> allTiles = new Dictionary<string, Vector3>();
    [HideInInspector] public Dictionary<string, List<Vector3>> playerShipPos = new Dictionary<string, List<Vector3>>();
    public string[] player1ShipPositions;
    public string[] player2ShipPositions = { "B1", "B2", "B3" };

    public GameObject inputFeild;
    public bool hasHit;
    private Vector3 guessPos;

    //Prefabs for hitting and missing
    public GameObject hit;
    public GameObject miss;

    void Awake()
    {
        buildingScript = this.GetComponent<BuildingSystem>();
        allTiles = buildingScript.allTiles;
        playerShipPos = buildingScript.allShipPos;
    }

    public void CheckForHit()
    {
        allTiles.TryGetValue(inputFeild.GetComponent<TMP_InputField>().text, out guessPos);
        foreach (var ship in playerShipPos)
        {
            foreach (var shipPos in ship.Value)
            {
                if (guessPos == shipPos)
                {
                    Debug.Log("Hit" + ship.Key + guessPos);
                    Instantiate(hit, guessPos + new Vector3(5, 10, 5), Quaternion.identity);
                    hasHit = true;
                }
            }
        }

        if (!hasHit)
        {
            Debug.Log("Miss");
            Instantiate(miss, guessPos + new Vector3(5, 10, 5), Quaternion.identity);
        }
        hasHit = false;
    }
}



