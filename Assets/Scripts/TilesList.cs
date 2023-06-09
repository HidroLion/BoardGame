using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesList : MonoBehaviour
{
    [SerializeField] Transform mainTiles;
    [SerializeField] Transform[] winTiles;

    Transform[] tiles;
    Transform[,] winLine;

    public Transform[] Tiles { get => tiles; set => tiles = value; }
    public Transform[,] WinLine { get => winLine; set => winLine = value; }

    // Start is called before the first frame update
    void Awake()
    {
        Tiles = new Transform[mainTiles.childCount];
        WinLine = new Transform[4,6];

        for (int i = 0; i < mainTiles.childCount; i++)
        {
            Tiles[i] = mainTiles.GetChild(i).transform;
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                WinLine[i, j] = winTiles[i].GetChild(j);
            }
        }
    }
}
