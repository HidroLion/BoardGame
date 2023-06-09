using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagesMachine : MonoBehaviour
{
    UIManager managerUI;

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
    bool gameFinish;

    public PlayerClass[] Players { get => players; set => players = value; }
    public int Dice { get => dice; set => dice = value; }
    public bool GameFinish { get => gameFinish; set => gameFinish = value; }
    public int PlayerTurn { get => playerTurn; set => playerTurn = value; }

    public bool NewTurn { get => newTurn; }
    public bool DiceRolled { get => diceRolled; }
    public int ExtraTurns { get => extraTurns; }

    private void Start()
    {
        managerUI = GetComponent<UIManager>();
        GameFinish = false;
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

        PlayerTurn = Random.Range(0, 4);
        newTurn = false;

        for (int i = 0; i < 4; i++)
        {
            Players[PlayerTurn].AllPawns[i].ActivePlayer = true;
        }

        managerUI.UpdateUI(PlayerTurn);
#if UNITY_EDITOR
        Debug.Log("{HD} - Game Started: First Player Selected > Player " + PlayerTurn);
#endif
    }

    private void Update()
    {
        if (!GameFinish)
        {
            if (!newTurn)
                timer += Time.deltaTime;

            if (timer >= maxTime && !newTurn)
            {
                timer = 0;
                newTurn = true;
                managerUI.UpdateUI(PlayerTurn);
                managerUI.ReadyText("Roll The Dice");
#if UNITY_EDITOR
                Debug.Log("{HD} - Turn Started: Player " + PlayerTurn);
#endif
            }

            if (newTurn)
            {
#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    dice = 6;
                    Debug.Log("{HD} - Dice Rolled: " + dice);
                    diceRolled = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    dice = 5;
                    Debug.Log("{HD} - Dice Rolled: " + dice);
                    diceRolled = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    dice = 4;
                    Debug.Log("{HD} - Dice Rolled: " + dice);
                    diceRolled = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    dice = 3;
                    Debug.Log("{HD} - Dice Rolled: " + dice);
                    diceRolled = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    dice = 2;
                    Debug.Log("{HD} - Dice Rolled: " + dice);
                    diceRolled = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    dice = 1;
                    Debug.Log("{HD} - Dice Rolled: " + dice);
                    diceRolled = true;
                }
#endif
                if (Players[PlayerTurn].WinPawns.Count == 4)
                    ChangeTurn();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!diceRolled)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Players[PlayerTurn].AllPawns[i].ActivePlayer = true;
                            Players[PlayerTurn].AllPawns[i].ShowPlayer(1);
                        }

                        dice = Random.Range(0, 6) + 1;

                        managerUI.ReadyText("Dice Rolled");
                        diceRolled = true;

                        if (Players[PlayerTurn].JailPawns.Count != 0)
                        {
                            if (dice == 6)
                            {
                                managerUI.ReadyText("Unlock or Select a Pawn");
                                SelectPawn();
                            }
                            else if (Players[PlayerTurn].JailPawns.Count == 4)
                            {
                                managerUI.ReadyText("Skip Turn");
                                ChangeTurn();
                                diceRolled = false;
                            }
                            else if (Players[PlayerTurn].JailPawns.Count <= 3)
                            {
                                managerUI.ReadyText("Select a Pawn");
                            }
                        }
                    }
                    else if (diceRolled)
                    {
                        managerUI.ReadyText("Select a Pawn");
                        SelectPawn();
                    }
                }
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
            if (hit.collider.CompareTag("Player " + PlayerTurn))
            {
                pawnSelect = hit.collider.GetComponent<PlayerController>();

                if (!pawnSelect.Unlock && dice == 6)
                {
                    pawnSelect.FirstMove();
                    if (dice == 6 && extraTurns < 3)
                    {
                        diceRolled = false;
                        extraTurns++;
                    }
                    else
                    {
                        extraTurns = 0;
                    }

                    Players[PlayerTurn].JailPawns.RemoveAt(Players[PlayerTurn].JailPawns.Count - 1);
                    managerUI.ReadyText("Pawn Unlocked... Roll Again");
#if UNITY_EDITOR
                    Debug.Log("{HD} - Unlock: Pawn Selected - Player " + PlayerTurn + " Pawn: " + pawnSelect.name);
#endif
                }
#if UNITY_EDITOR
                else
                {
                    pawnSelect.TilesMovement(dice);
                    if (dice == 6 && extraTurns < 3)
                    {
                        diceRolled = false;
                        extraTurns++;
                    }
                    else
                    {
                        extraTurns = 0;
                    }
#if UNITY_EDITOR
                    Debug.Log("{HD} - Move: Pawn Selected - Player " + PlayerTurn + " Pawn: " + pawnSelect.name);
#endif
                }
#endif
            }
        }
    }

    public void ChangeTurn()
    {
        if (dice != 6 || extraTurns == 2)
        {
            diceRolled = false;
            newTurn = false;

            UpdateTurn();
        }
    }

    void UpdateTurn()
    {
        for (int i = 0; i < 4; i++)
        {
            Players[PlayerTurn].AllPawns[i].ActivePlayer = false;
            Players[PlayerTurn].AllPawns[i].ShowPlayer(-1);
        }

        PlayerTurn++;
        if (PlayerTurn == 4)
            PlayerTurn = 0;
    }
}