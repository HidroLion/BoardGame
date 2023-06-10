using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text turnText;
    [SerializeField] Text turnText2;
    string turnName;

    public void UpdateUI(int playerTurn)
    {
        switch (playerTurn)
        {
            case 0:
                turnName = "Blue Turn";
                turnText.text = turnName;
                break;

            case 1:
                turnName = "Red Turn";
                turnText.text = turnName;
                break;

            case 2:
                turnName = "Green Turn";
                turnText.text = turnName;
                break;

            case 3:
                turnName = "Yellow Turn";
                turnText.text = turnName;
                break;
        }
    }

    public void ReadyText(string readyText)
    {
        turnText2.text = readyText;
    }
}
