using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagesMachine : MonoBehaviour
{
    //Teams Variables
    [SerializeField] PlayerController[] bluePawns;
    [SerializeField] PlayerController[] redPawns;
    [SerializeField] PlayerController[] greenPawns;
    [SerializeField] PlayerController[] yellowPawns;

    PlayerClass[] players;
    int playerTurn;
    int dice;

    private void Start()
    {
        players = new PlayerClass[4];

        players[0] = new PlayerClass(bluePawns, 0);
        players[1] = new PlayerClass(redPawns, 1);
        players[2] = new PlayerClass(greenPawns, 2);
        players[3] = new PlayerClass(yellowPawns, 3);

        for (int i = 0; i < 4; i++)
        {
            players[i].InicializePlayer();
        }

        playerTurn = Random.Range(0, 4);

#if UNITY_EDITOR
        Debug.Log(playerTurn);

        for (int i = 0; i < 4; i++)
        {
            Debug.Log(players[playerTurn].AllPawns[i].name);
        }
#endif
    }

    public void RollDice()
    {
        dice = Random.Range(0, 7);

        if(dice == 6)
        {
            if (players[playerTurn].JailPawns.Count != 0)
            {
                players[playerTurn].JailPawns.RemoveAt(players[playerTurn].JailPawns.Count - 1);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollDice();
        }
    }
}
