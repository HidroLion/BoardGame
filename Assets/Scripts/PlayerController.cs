using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class PlayerController : MonoBehaviour
{
    [SerializeField] TilesList list;

    [SerializeField] int startPosIndex;
    [SerializeField] int colorID;

    int movesCount;
    Transform[] playerRoute;
    bool walking;

    float speed = 10;

    int currentMoves;
    int maxMoves;

    public bool Walking { get => walking; set => walking = value; }

    private void Start()
    {
        currentMoves = 0;

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

    public void FirstMove()
    {
        transform.position = list.Tiles[startPosIndex].position;
        movesCount = startPosIndex;
    }

    public void Movement()
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

    public void TilesMovement(int moves)
    {
        maxMoves = moves;
        Walking = true;
    }

    private void Update()
    {
        if (Walking)
        {
            Movement();
        }
    }
}