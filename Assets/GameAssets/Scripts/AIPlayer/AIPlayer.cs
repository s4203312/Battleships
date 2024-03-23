using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AIPlayer : MonoBehaviour
{
    public static BuildingSystem buildingScript;
    public static SelectingChoice playerchoiceScript;
    public GameObject refGameObject;

    private List<string> neighbourTiles;

    [HideInInspector] public Dictionary<string, Vector3> allTiles = new Dictionary<string, Vector3>();
    [HideInInspector] public Dictionary<string, List<Vector3>> playerShipPos = new Dictionary<string, List<Vector3>>();

    private bool randomPick = true;

    public bool hasHit;
    public string lastHit;
    private Vector3 guessPos;

    public GameObject hit;
    public GameObject miss;
    public GameObject missile;

    void Awake()
    {
        playerchoiceScript = refGameObject.GetComponent<SelectingChoice>();
        buildingScript = this.GetComponent<BuildingSystem>();
        allTiles = buildingScript.allTiles;
        playerShipPos = buildingScript.allShipPos;
    }

    public void MakeDecision()
    {
        //Randomly picking a tile
        if (randomPick)
        {
            int posPick = UnityEngine.Random.Range(0, allTiles.Count);
            string tilePicked = allTiles.ElementAt(posPick).Key;
            CheckForHit(tilePicked);
        }
        else
        {
            int posPick = UnityEngine.Random.Range(0, neighbourTiles.Count);
            CheckForHit(neighbourTiles[posPick]);
        }

        if (hasHit)
        {
            neighbourTiles = new List<string>();
            char letterUp = '0';
            char letterDown = '0';
            char numberLeft = '0';
            char numberRight = '0';

            char[] splitUp = lastHit.ToCharArray();
            Debug.Log(lastHit);

            //Letter manipulation
            if ((splitUp[0] == 'A'))
            {
                char x = splitUp[0];
                letterUp = Convert.ToChar((Convert.ToUInt16(x) + 1));
                Debug.Log(letterUp);
            }
            else if ((splitUp[0] == 'J'))
            {
                char x = splitUp[0];
                letterDown = Convert.ToChar((Convert.ToUInt16(x) - 1));
                Debug.Log(letterDown);
            }
            else
            {
                //Finds the neighbouring letters
                char x = splitUp[0];
                letterUp = Convert.ToChar((Convert.ToUInt16(x) + 1));
                letterDown = Convert.ToChar((Convert.ToUInt16(x) - 1));
                Debug.Log(letterUp);
                Debug.Log(letterDown);
            }


            //Number manipulation
            if ((splitUp[1] == '1') && splitUp.Length == 2)
            {
                int a = Convert.ToUInt16(splitUp[1]);
                numberRight = Convert.ToChar(a + 1);
                Debug.Log(numberRight);
            }
            //Checks if it is 10 as 10 is representaed as 1 and 0 in a char array
            else if ((splitUp[1] == '1') && splitUp.Length == 3)
            {
                numberLeft = Convert.ToChar(57);        //Ascii value for 9
                Debug.Log(numberLeft);
            }
            else
            {
                //Finds the neighbouring numbers
                int a = Convert.ToUInt16(splitUp[1]);
                numberRight = Convert.ToChar(a + 1);
                numberLeft = Convert.ToChar(a - 1);
                Debug.Log(numberRight);
                Debug.Log(numberLeft);
            }

            //Finding neighbours
            if (letterDown == '0')
            {
                string tile1 = letterUp.ToString() + splitUp[1].ToString();
                string tile2 = splitUp[0].ToString() + numberLeft.ToString();
                string tile3 = splitUp[0].ToString() + numberRight.ToString();

                neighbourTiles.Add(tile1);
                neighbourTiles.Add(tile2);
                neighbourTiles.Add(tile3);
            }
            else if (letterUp == '0')
            {
                string tile1 = letterDown.ToString() + splitUp[1].ToString();
                string tile2 = splitUp[0].ToString() + numberLeft.ToString();
                string tile3 = splitUp[0].ToString() + numberRight.ToString();

                neighbourTiles.Add(tile1);
                neighbourTiles.Add(tile2);
                neighbourTiles.Add(tile3);
            }
            else if (numberLeft == '0')
            {
                string tile1 = splitUp[0].ToString() + numberRight.ToString();
                string tile2 = letterDown.ToString() + splitUp[1].ToString();
                string tile3 = letterUp.ToString() + splitUp[1].ToString();

                neighbourTiles.Add(tile1);
                neighbourTiles.Add(tile2);
                neighbourTiles.Add(tile3);
            }
            else if (numberRight == '0')
            {
                string tile1 = splitUp[0].ToString() + numberLeft.ToString();
                string tile2 = letterDown.ToString() + splitUp[1].ToString();
                string tile3 = letterUp.ToString() + splitUp[1].ToString();

                neighbourTiles.Add(tile1);
                neighbourTiles.Add(tile2);
                neighbourTiles.Add(tile3);
            }
            else
            {
                string tile1 = letterUp.ToString() + splitUp[1].ToString();
                string tile2 = letterDown.ToString() + splitUp[1].ToString();
                string tile3 = splitUp[0].ToString() + numberLeft.ToString();
                string tile4 = splitUp[0].ToString() + numberRight.ToString();

                neighbourTiles.Add(tile1);
                neighbourTiles.Add(tile2);
                neighbourTiles.Add(tile3);
                neighbourTiles.Add(tile4);
            }
        }
    }
    

    //Checks if guess has hit player ship
    public void CheckForHit(string AIGuess)
    {
        hasHit = false;

        allTiles.TryGetValue(AIGuess, out guessPos);
        foreach (var ship in playerShipPos)
        {
            foreach (var shipPos in ship.Value)
            {
                if (guessPos == shipPos)
                {
                    Debug.Log("Hit" + ship.Key + guessPos);
                    Instantiate(hit, guessPos + new Vector3(5, 10, 5), Quaternion.identity);
                    Instantiate(missile, guessPos + new Vector3(5, 100, 5), Quaternion.identity);
                    hasHit = true;
                    randomPick = false;
                    lastHit = AIGuess;
                }
            }
        }

        if (!hasHit)
        {
            Debug.Log("Miss");
            Instantiate(miss, guessPos + new Vector3(5, 10, 5), Quaternion.identity);
        }
    }
}
