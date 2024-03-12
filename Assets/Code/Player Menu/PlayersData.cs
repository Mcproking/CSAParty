using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayersData 
{
    public List<PlayerData> playerDatas;

    public PlayersData() 
    { 
        playerDatas = new List<PlayerData>();
    }
}
