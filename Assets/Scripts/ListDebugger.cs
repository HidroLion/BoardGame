using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListDebugger : MonoBehaviour
{
    StagesMachine machine;

    [Header("Jailed Counters")]
    public int Player_1;
    public int Player_2;
    public int Player_3;
    public int Player_4;

    public List<PlayerController> jailedPawnsList1;
    public List<PlayerController> jailedPawnsList2;
    public List<PlayerController> jailedPawnsList3;
    public List<PlayerController> jailedPawnsList4;

    [Header("States")]
    public int playerTurn;
    public int extraTurns;
    public bool newTurn;

    private void Start()
    {
        machine = GetComponent<StagesMachine>();
    }

    private void Update()
    {
        Player_1 = machine.Players[0].JailPawns.Count;
        Player_2 = machine.Players[1].JailPawns.Count;
        Player_3 = machine.Players[2].JailPawns.Count;
        Player_4 = machine.Players[3].JailPawns.Count;

        jailedPawnsList1 = machine.Players[0].JailPawns;
        jailedPawnsList2 = machine.Players[1].JailPawns;
        jailedPawnsList3 = machine.Players[2].JailPawns;
        jailedPawnsList4 = machine.Players[3].JailPawns;

        playerTurn = machine.PlayerTurn;
        extraTurns = machine.ExtraTurns;
        newTurn = machine.NewTurn;
    }
}
