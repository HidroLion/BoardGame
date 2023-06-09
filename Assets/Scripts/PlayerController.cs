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

    Vector3 jailPosition;

    StagesMachine gameCotroller;
    int movesCount;
    Transform[] playerRoute;
    bool walking;
    bool unlock;

    int currentMoves;
    int maxMoves;

    bool safeZone;
    bool activePlayer;

    public bool Walking { get => walking; set => walking = value; }
    public bool Unlock { get => unlock; set => unlock = value; }
    public bool SafeZone { get => safeZone; set => safeZone = value; }
    public bool ActivePlayer { get => activePlayer; set => activePlayer = value; }

    //Start Funtions
    private void Start()
    {
        currentMoves = 0;
        Unlock = false;

        jailPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

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

        gameCotroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<StagesMachine>();
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

    //Kill and Die Funtions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Safe"))
        {
            SafeZone = true;
        }

        if (collision.CompareTag("Player " + colorID))
        {
            SafeZone = true;
            collision.GetComponent<PlayerController>().SafeZone = true;
        }

        switch (colorID)
        {
            case 0:
                if(collision.CompareTag("Player 1") || collision.CompareTag("Player 2") || collision.CompareTag("Player 3"))
                {
                    if(!collision.GetComponent<PlayerController>().SafeZone 
                        && !collision.GetComponent<PlayerController>().ActivePlayer)
                        collision.GetComponent<PlayerController>().Dead();
                }
                break;

            case 1:
                if (collision.CompareTag("Player 0") || collision.CompareTag("Player 2") || collision.CompareTag("Player 3"))
                {
                    if (!collision.GetComponent<PlayerController>().SafeZone
                        && !collision.GetComponent<PlayerController>().ActivePlayer)
                        collision.GetComponent<PlayerController>().Dead();
                }
                break;

            case 2:
                if (collision.CompareTag("Player 0") || collision.CompareTag("Player 1") || collision.CompareTag("Player 3"))
                {
                    if (!collision.GetComponent<PlayerController>().SafeZone
                        && !collision.GetComponent<PlayerController>().ActivePlayer)
                        collision.GetComponent<PlayerController>().Dead();
                }
                break;

            case 3:
                if (collision.CompareTag("Player 0") || collision.CompareTag("Player 1") || collision.CompareTag("Player 2"))
                {
                    if (!collision.GetComponent<PlayerController>().SafeZone
                        && !collision.GetComponent<PlayerController>().ActivePlayer)
                        collision.GetComponent<PlayerController>().Dead();
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Safe"))
        {
            SafeZone = false;
        }

        if (collision.CompareTag("Player " + colorID))
        {
            SafeZone = false;
        }
    }

    public void Dead()
    {
        Unlock = false;
        transform.position = jailPosition;
        gameCotroller.Players[colorID].JailPawns.Add(gameObject.GetComponent<PlayerController>());

#if UNITY_EDITOR
        Debug.Log("{HD} - Kill: Pawn Dead - Player " + colorID);
#endif
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