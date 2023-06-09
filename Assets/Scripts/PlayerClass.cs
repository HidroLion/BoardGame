using System.Collections;
using System.Collections.Generic;

public class PlayerClass
{  
    PlayerController[] allPawns;
    List<PlayerController> winPawns;
    List<PlayerController> jailPawns;

    int playerID;

    public PlayerClass
        (PlayerController[] allPawns, int playerID)
    {
        this.AllPawns = allPawns;
        this.PlayerID = playerID;

        winPawns = new List<PlayerController>();
        JailPawns = new List<PlayerController>();
    }

    public PlayerController[] AllPawns { get => allPawns; set => allPawns = value; }
    public List<PlayerController> WinPawns { get => winPawns; set => winPawns = value; }
    public List<PlayerController> JailPawns { get => jailPawns; set => jailPawns = value; }
    public int PlayerID { get => playerID; set => playerID = value; }

    public void InicializePlayer()
    {
        for (int i = 0; i < 4; i++)
        {
            JailPawns.Add(allPawns[i]);
        }
    }
}
