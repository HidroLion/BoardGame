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

    CircleCollider2D circleCollider; //Collider Refference
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

    float timer;

    public bool Walking { get => walking; set => walking = value; }
    public bool Unlock { get => unlock; set => unlock = value; }
    public bool SafeZone { get => safeZone; set => safeZone = value; }
    public bool ActivePlayer { get => activePlayer; set => activePlayer = value; }
    public CircleCollider2D CircleCollider { get => circleCollider; set => circleCollider = value; }
    public bool Winner { get => winner; set => winner = value; }

    //Start Funtions
    private void Start()
    {
        currentMoves = 0;
        Unlock = false;

        CircleCollider = gameObject.GetComponent<CircleCollider2D>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        jailPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        LateStart();
        timer = 0;
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
            if (maxMoves > 1)
            {
                CircleCollider.enabled = false;
            }

            //circleCollider.enabled = false;
            transform.position =
                Vector2.MoveTowards
                    (transform.position, playerRoute[currentMoves + 1].position, speed * Time.deltaTime);
            if (transform.position == playerRoute[currentMoves + 1].position)
            {
                currentMoves++;
                maxMoves--;

                if (maxMoves == 0)
                {
                    Walking = false;
                    gameCotroller.ChangeTurn();
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

        if (ActivePlayer && !SafeZone) //Is player has the turn active this colliders.
        {
            switch (colorID) //Controls when the player touches another Pawn, depending of the color.
            {
                case 0:
                    if (collision.CompareTag("Player 1") || collision.CompareTag("Player 2") || collision.CompareTag("Player 3"))
                    {
                        if (!collision.GetComponent<PlayerController>().SafeZone
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

    public void Dead() //Player Dead (This Function is activated by other Pawn)
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

    //General Functions
    private void Update() 
    {
        if (Walking) //If Waliking, Movement is activated to Move a Pawn
        {
            Movement();
        }
        else if(!CircleCollider.enabled)
            CircleCollider.enabled = true;

        //Controls the Visibility
        if (ActivePlayer)
            spriteRenderer.sortingOrder = 1;
        else if (!ActivePlayer)
            spriteRenderer.sortingOrder = 0;

        //PAwn Win activated when the Total Moves is equal 56
        if (currentMoves == 56 && !Winner)
            PawnWin();
    }
}