using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectingChoice : MonoBehaviour
{
    public string[] player1ShipPositions = {"A1", "A2", "A3"};
    public string[] player2ShipPositions = {"B1", "B2", "B3"};

    public GameObject inputFeild;
    public GameObject whosTurn;

    public bool isplayer1 = true;
    public bool hasHit;

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
            foreach (var ship in player1ShipPositions)
            {
                if (inputFeild.GetComponent<TMP_InputField>().text == ship)
                {
                    Debug.Log("Hit1");
                    hasHit = true;
                }
            }
            if (!hasHit)
            {
                Debug.Log("Miss1");
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


