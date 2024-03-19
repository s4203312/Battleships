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
    public string[] player2ShipPositions = {"B1", "B2", "B3"};

    public GameObject inputFeild;
    public GameObject whosTurn;

    public bool isplayer1 = true;
    public bool hasHit;

    private Vector3 guessPos;

    public GameObject hit;
    public GameObject miss;

    void Awake()
    {
        buildingScript = this.GetComponent<BuildingSystem>();
        allTiles = buildingScript.allTiles;
        playerShipPos = buildingScript.allShipPos;
    }

    public void Update()
    {
        if(isplayer1)
        {
            whosTurn.GetComponent<TMP_Text>().text = "Player1";
        }
        else
        {
            whosTurn.GetComponent<TMP_Text>().text = "Player2";
        }
        
    }

    public void CheckForHit()
    {
        if (isplayer1)
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


            isplayer1 = false;
            hasHit = false;
        }
        else if (!isplayer1)
        {
            foreach (var ship in player2ShipPositions)
            {
                if (inputFeild.GetComponent<TMP_InputField>().text == ship)
                {
                    Debug.Log("Hit2");
                    hasHit = true;
                }
            }
            if (!hasHit)
            {
                Debug.Log("Miss2");
            }


            isplayer1 = true;
            hasHit = false;
        }
    }
}


