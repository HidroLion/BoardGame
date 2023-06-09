using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class PlayerController : MonoBehaviour
{
    //Serializeble Variables > Refferences
    [SerializeField] TilesList list; //List of Tiles (GameManager)
    [SerializeField] int startPosIndex; //Start Position: Array Position of the Start Tile 
    [SerializeField] int colorID; //Array Position of Colors Tiles Lists
    [SerializeField] float speed; //Movement Speed: Recomended 10

    int movesCount;
    Transform[] playerRoute;
    bool walking;
    bool unlock;

    int currentMoves;
    int maxMoves;

    public bool Walking { get => walking; set => walking = value; }
    public bool Unlock { get => unlock; set => unlock = value; }

    //Start Funtions
    private void Start()
    {
        currentMoves = 0;
        Unlock = false;
        LateStart();
    }

    void LateStart()
    {
        movesCount = startPosIndex;
        playerRoute = new Transform[57];

        for (int i = 0; i < 51; i++)
        {
            playerRoute[i] = list.Tiles[movesCount];
            movesCount++;
            if (movesCount == 52)
                movesCount = 0;
        }

        for (int i = 0; i < 6; i++)
        {
            playerRoute[i + 51] = list.WinLine[colorID, i];
        }
    }

    //Movement Functions
    public void FirstMove()
    {
        transform.position = list.Tiles[startPosIndex].position;
        movesCount = startPosIndex;
        Unlock = true;
    }

    public void TilesMovement(int moves)
    {
        if (Unlock)
        {
            maxMoves = moves;
            Walking = true;
        }
    }

    public void Movement()
    {
        if(currentMoves + maxMoves <= 56)
        {
            transform.position =
                Vector2.MoveTowards
                    (transform.position, playerRoute[currentMoves + 1].position, speed * Time.deltaTime);
            if (transform.position == playerRoute[currentMoves + 1].position)
            {
                currentMoves++;
                maxMoves--;
                if (maxMoves == 0)
                    Walking = false;
            }
        }
    }

    //General Functions
    private void Update()
    {
        if (Walking)
        {
            Movement();
        }
    }
}