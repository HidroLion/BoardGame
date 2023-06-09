using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SearchService;

public class PlayerController : MonoBehaviour
{
    //Serializeble Variables > Refferences
    [SerializeField] TilesList list; //List of Tiles (GameManager)
    [SerializeField] int startPosIndex; //Start Position: Array Position of the Start Tile 
    [SerializeField] int colorID; //Array Position of Colors Tiles Lists
    [SerializeField] float speed; //Movement Speed: Recomended 10

    Rigidbody2D rb2D; //Rigidbody Refference
    SpriteRenderer spriteRenderer; //Sprite Renderer Refference
    Vector3 jailPosition; //Jail Pawn Position = Start Position.

    StagesMachine gameCotroller; //Polymorphism > StageMachine
    int movesCount; //Number of Movements that the Pawn will do
    Transform[] playerRoute; //List of tiles in the Playrr Route > Depending of the colorID
    bool walking; //Pawn is Moving = Tre
    bool unlock; //Pawn is in the Table = True
    bool winner; //Pawn is in the Goal Position = True

    int currentMoves; //Number of Movements in Total (0 - 56)
    int maxMoves; //Number of Movements in each turn (1 - 6)

    bool safeZone; //Player Locked
    bool activePlayer; //Player have the turn

    public bool Walking { get => walking; set => walking = value; }
    public bool Unlock { get => unlock; set => unlock = value; }
    public bool SafeZone { get => safeZone; set => safeZone = value; }
    public bool ActivePlayer { get => activePlayer; set => activePlayer = value; }
    public bool Winner { get => winner; set => winner = value; }

    //Start Funtions
    private void Start()
    {
        currentMoves = 0;
        Unlock = false;

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        jailPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        LateStart();
    }

    //Strat Delayed for evit Missing errors
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
    public void FirstMove() //Moves the Player Until the Home Tile
    {
        transform.position = list.Tiles[startPosIndex].position;
        movesCount = startPosIndex;
        SearchPlayer();
        Unlock = true;
    }

    public void TilesMovement(int moves) //Moves the player according the Dice result
    {
        if (Unlock)
        {
            maxMoves = moves;
            Walking = true;
        }
    }

    public void Movement() //Moves the player in each turn and setup the State of the game
    {
        if (currentMoves + maxMoves <= 56)
        {
            transform.position =
                Vector3.MoveTowards
                    (transform.position, playerRoute[currentMoves + 1].position, speed * Time.deltaTime);
            if (transform.position == playerRoute[currentMoves + 1].position)
            {
                movesCount++;
                if (movesCount == 52)
                    movesCount = 0;

                currentMoves++;
                maxMoves--;

                if (maxMoves == 0)
                {
                    SearchPlayer();
                    gameCotroller.ChangeTurn();
                    Walking = false;
                }
            }
        }
    }

    //Kill and Die Funtions
    private void OnTriggerEnter2D(Collider2D collision) //Detect Collisions
    {
        if (collision.CompareTag("Safe")) //If the player Touches a Safe Zone, SafeZone is True.
        {
            SafeZone = true;
        }

        if (collision.CompareTag("Player " + colorID)) //If the player Touches anothe Pawn with same color, SafeZone is True.
        {
            SafeZone = true;
            collision.GetComponent<PlayerController>().SafeZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //Disable de Safe Zones when the players separate or leave the safe zone
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

    void SearchPlayer() //Find a player in the Same Tile to the Player in Turn to kill it
    {
        switch (colorID)
        {
            case 0:
                for (int i = 0; i < 4; i++)
                {
                    if (gameCotroller.Players[1].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[1].AllPawns[i].Dead();

                    if (gameCotroller.Players[2].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[2].AllPawns[i].Dead();

                    if (gameCotroller.Players[3].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[3].AllPawns[i].Dead();
                }
                break;

            case 1:
                for (int i = 0; i < 4; i++)
                {
                    if (gameCotroller.Players[0].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[0].AllPawns[i].Dead();

                    if (gameCotroller.Players[2].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[2].AllPawns[i].Dead();

                    if (gameCotroller.Players[3].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[3].AllPawns[i].Dead();
                }
                break;

            case 2:
                for (int i = 0; i < 4; i++)
                {
                    if (gameCotroller.Players[1].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[1].AllPawns[i].Dead();

                    if (gameCotroller.Players[0].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[0].AllPawns[i].Dead();

                    if (gameCotroller.Players[3].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[3].AllPawns[i].Dead();
                }
                break;

            case 3:
                for (int i = 0; i < 4; i++)
                {
                    if (gameCotroller.Players[1].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[1].AllPawns[i].Dead();

                    if (gameCotroller.Players[2].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[2].AllPawns[i].Dead();

                    if (gameCotroller.Players[0].AllPawns[i].movesCount == movesCount)
                        gameCotroller.Players[0].AllPawns[i].Dead();
                }
                break;
        }
    }

    public void Dead() //Player Dead (This Function is activated by other Pawn)
    {
        if (!SafeZone && Unlock)
        {
            Unlock = false;
            transform.position = jailPosition;
            currentMoves = 0;
            gameCotroller.Players[colorID].JailPawns.Add(gameObject.GetComponent<PlayerController>());

            gameCotroller.PlayerTurn--;
            if (gameCotroller.PlayerTurn == -1)
                gameCotroller.PlayerTurn = 0;

#if UNITY_EDITOR
            Debug.Log("{HD} - Kill: Pawn Dead - Player " + colorID + " Count: " + gameCotroller.Players[colorID].JailPawns.Count);
#endif
        }
    }
     
    void PawnWin() //Pawn Win Conditions
    {
        gameCotroller.Players[colorID].WinPawns.Add(gameObject.GetComponent<PlayerController>());
        gameCotroller.PlayerTurn--;
        if (gameCotroller.PlayerTurn == -1)
            gameCotroller.PlayerTurn = 0;
#if UNITY_EDITOR
        Debug.Log("{HD} - Win: Pawn Winner > " + gameCotroller.Players[colorID].WinPawns.Count + colorID);
#endif
        if (gameCotroller.Players[colorID].WinPawns.Count == 4)
        {
            Winner = true;
#if UNITY_EDITOR
            Debug.Log("{HD} - Win: Player Winner > " + colorID);
#endif
        }
    }

    public void ShowPlayer(float z)
    {
        transform.position += Vector3.forward * z;
    }

    //General Functions
    private void Update() 
    {
        if (Walking) //If Waliking, Movement is activated to Move a Pawn
        {
            Movement();
        }

        //Controls the Visibility
        if (ActivePlayer)
        {
            spriteRenderer.sortingOrder = 1;
        }
        else if (!ActivePlayer)
        {
            spriteRenderer.sortingOrder = 0;
        }

        //PAwn Win activated when the Total Moves is equal 56
        if (currentMoves == 56 && !Winner)
            PawnWin();
    }
}