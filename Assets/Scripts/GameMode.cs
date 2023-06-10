using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [SerializeField] Text buttonText;
    [SerializeField] Text winnerText;
    StagesMachine machine;
    bool teamMode;

    private void Start()
    {
        machine = GetComponent<StagesMachine>();
        teamMode = false;
        buttonText.text = "Single";
        winnerText.text = " ";
    }

    public void ChangeMode()
    {
        teamMode = !teamMode;
        if (teamMode)
            buttonText.text = "Teams";
        else
            buttonText.text = "Single";
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
    private void Update()
    {
        if (teamMode)
        {
            if (machine.Players[0].WinPawns.Count == 4 && machine.Players[1].WinPawns.Count == 4)
            {
                winnerText.text = "Game Finish! Team A Wins: Blue and Red";
                machine.GameFinish = true;
            }
            if (machine.Players[2].WinPawns.Count == 4 && machine.Players[3].WinPawns.Count == 4)
            {
                winnerText.text = "Game Finish! Team A Wins: Green and Yellow";
                machine.GameFinish = true;
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if(machine.Players[i].WinPawns.Count == 4)
                {
                    winnerText.text = "Game Finish! Player " + (i + 1) + " Wins";
                    machine.GameFinish = true;
                }
            }
        }
    }
}
