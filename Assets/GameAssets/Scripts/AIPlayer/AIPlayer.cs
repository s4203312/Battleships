using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    public static SelectingChoice playerchoiceScript;
    public GameObject refGameObject;

    private string[] allTiles = { "A1", "A2", "A3", "B1", "B2", "B3", "C1", "C2", "C3" };
    private string[] neighbourTiles;

    private string[] playersShipsPos = { "A1", "A2", "A3" };
    private bool randomPick = true;


    void Awake()
    {
        playerchoiceScript = refGameObject.GetComponent<SelectingChoice>();
    }

    public void MakeDecision()
    {
        if (randomPick)
        {
            int posPick = UnityEngine.Random.Range(0, allTiles.Length);
            string tilePicked = allTiles[posPick];
            Debug.Log(tilePicked);
            foreach (string shipPos in playersShipsPos) 
            { 
                if(tilePicked == shipPos)
                {
                    Debug.Log("Hit");
                    randomPick = false;

                    char[] splitUp = tilePicked.ToCharArray();

                    
                    char x = splitUp[0];
                    char y = Convert.ToChar((Convert.ToUInt16(x) + 1));
                    char z = Convert.ToChar((char)(Convert.ToUInt16(x) - 1));

                    //int a = int.Parse(splitUp[0].ToString());
                    //char b = (char)(a + 1);
                    //char c = (char)(a - 1);

                    Debug.Log(y);

                    for(int i = 0; i < 4; i++)
                    {
                        //neighbourTiles[i] = 
                    }

                    if ((splitUp[0] == 'A') || (splitUp[0] == 'C'))
                    {

                    }
                    if ((splitUp[1] == '1') || (splitUp[1] == '3'))
                    {

                    }
                }
            }
        }
        else
        {

        }
    }
}
