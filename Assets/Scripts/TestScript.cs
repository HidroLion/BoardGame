using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] int fakeDice;
    [SerializeField] PlayerController playerCounter;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerCounter.FirstMove();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            playerCounter.TilesMovement(fakeDice);
        }
    }
}
