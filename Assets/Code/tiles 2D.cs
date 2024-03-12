using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiles2D : MonoBehaviour
{
    //get spawn tile name
    public string getSpawnTile(Player2D player, int playerTurn)
    {
        string spawnTileName;
        if (Physics.Raycast(new Ray(player.transform.position, transform.forward), out RaycastHit hit, 1f))
        {
            spawnTileName = hit.collider.name;
            switch (playerTurn)
            {
                case 1:
                    return spawnTileName;
                case 2:
                    return spawnTileName;
                case 3:
                    return spawnTileName;
                case 4:
                    return spawnTileName;
                default:
                    return null;
            }
        }
        else
        {
            return null;
        }
    }
}
