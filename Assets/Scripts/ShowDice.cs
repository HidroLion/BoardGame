using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDice : MonoBehaviour
{
    [SerializeField] StagesMachine machine;
    [SerializeField] GameObject[] dices;

    private void Update()
    {
        switch (machine.Dice - 1)
        {
            case 0:
                dices[0].SetActive(true);
                dices[1].SetActive(false);
                dices[2].SetActive(false);
                dices[3].SetActive(false);
                dices[4].SetActive(false);
                dices[5].SetActive(false);
                break;

            case 1:
                dices[0].SetActive(false);
                dices[1].SetActive(true);
                dices[2].SetActive(false);
                dices[3].SetActive(false);
                dices[4].SetActive(false);
                dices[5].SetActive(false);
                break;

            case 2:
                dices[0].SetActive(false);
                dices[1].SetActive(false);
                dices[2].SetActive(true);
                dices[3].SetActive(false);
                dices[4].SetActive(false);
                dices[5].SetActive(false);
                break;

            case 3:
                dices[0].SetActive(false);
                dices[1].SetActive(false);
                dices[2].SetActive(false);
                dices[3].SetActive(true);
                dices[4].SetActive(false);
                dices[5].SetActive(false);
                break;

            case 4:
                dices[0].SetActive(false);
                dices[1].SetActive(false);
                dices[2].SetActive(false);
                dices[3].SetActive(false);
                dices[4].SetActive(true);
                dices[5].SetActive(false);
                break;

            case 5:
                dices[0].SetActive(false);
                dices[1].SetActive(false);
                dices[2].SetActive(false);
                dices[3].SetActive(false);
                dices[4].SetActive(false);
                dices[5].SetActive(true);
                break;
        }
    }
}
