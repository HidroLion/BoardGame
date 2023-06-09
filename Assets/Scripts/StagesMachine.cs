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
    PlayerController pawnSelect;
    int playerTurn;
    int dice;

    [SerializeField] float maxTime;
    float timer;
    bool newTurn;
    bool diceRolled;
    int extraTurns;

    public PlayerClass[] Players { get => players; set => players = value; }

    private void Start()
    {
        diceRolled = false;
        timer = 0;
        extraTurns = 0;

        Players = new PlayerClass[4];

        Players[0] = new PlayerClass(bluePawns, 0);
        Players[1] = new PlayerClass(redPawns, 1);
        Players[2] = new PlayerClass(greenPawns, 2);
        Players[3] = new PlayerClass(yellowPawns, 3);

        for (int i = 0; i < 4; i++)
        {
            Players[i].InicializePlayer();
        }

        playerTurn = Random.Range(0, 4);
        newTurn = false;

        for (int i = 0; i < 4; i++)
        {
            Players[playerTurn].AllPawns[i].ActivePlayer = true;
        }

#if UNITY_EDITOR
        Debug.Log("{HD} - Game Started: First Player Selected > Player " + playerTurn);
#endif
    }

    private void Update()
    {
        if(!newTurn)
            timer += Time.deltaTime;

        if(timer >= maxTime && !newTurn)
        {
            timer = 0;
            newTurn = true;

#if UNITY_EDITOR
            Debug.Log("{HD} - Turn Started: Player " + playerTurn);
#endif
        }

        if (newTurn)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!diceRolled)
                {
                    dice = Random.Range(4, 6) + 1;
                    Debug.Log("{HD} - Dice Rolled: " + dice);
                    diceRolled = true;
                }
                else if (diceRolled)
                {
                    if (Players[playerTurn].JailPawns.Count != 0)
                    {
                        if (dice == 6)
                        {
                            SelectPawn(true);
                        }
                        else if (Players[playerTurn].JailPawns.Count == 4)
                        {
                            ChangeTurn();
                            diceRolled = false;
                        }
                        else if (Players[playerTurn].JailPawns.Count <= 3)
                        {
                            SelectPawn();
                        }
                    }
                    else
                    {
                        SelectPawn();
                    }
                }
            }
        }
    }

    void SelectPawn(bool unlocked)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player " + playerTurn))
            {
                pawnSelect = hit.collider.GetComponent<PlayerController>();

                if (!pawnSelect.Unlock)
                {
                    pawnSelect.FirstMove();
                    diceRolled = false;
                    newTurn = false;

                    Players[playerTurn].JailPawns.RemoveAt(Players[playerTurn].JailPawns.Count - 1);
                    Players[playerTurn].FreePawns.Add(pawnSelect);
#if UNITY_EDITOR
                    Debug.Log("{HD} - Unlock: Pawn Selected - Player " + playerTurn + " Pawn: " + pawnSelect.name);
#endif
                }
#if UNITY_EDITOR
                else
                {
                    Debug.Log("{HD} - Unlock: Pawn Selected Not Avaliable - Player " + playerTurn + " Unlocked: " + pawnSelect.name);
                }
#endif

            }
        }
    }

    void SelectPawn()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player " + playerTurn))
            {
                pawnSelect = hit.collider.GetComponent<PlayerController>();

                if (pawnSelect.Unlock)
                {
                    pawnSelect.TilesMovement(dice);
                    if(dice == 6 && extraTurns < 2)
                    {
                        diceRolled = false;
                        extraTurns++;
                    }
                    else
                    {
                        ChangeTurn();
                        extraTurns = 0;
                    }
                }
#if UNITY_EDITOR
                Debug.Log("{HD} - Move: Pawn Selected - Player " + playerTurn + " Pawn: " + pawnSelect.name);
#endif
            }
        }
    }

    void ChangeTurn()
    {
        for (int i = 0; i < 4; i++)
        {
            Players[playerTurn].AllPawns[i].ActivePlayer = false;
        }

        diceRolled = false;
        newTurn = false;

        playerTurn++;
        if (playerTurn == 4)
            playerTurn = 0;

        for (int i = 0; i < 4; i++)
        {
            Players[playerTurn].AllPawns[i].ActivePlayer = true;
        }
    }
}