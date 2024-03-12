using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData 
{
    public string Name;
    public int SkinSelection;
    [SerializeField]
    private bool Ready;
    public string lineUp;

    public PlayerData(string Name, int SkinSelection, bool Ready, string lineUp)
    {
        this.Name = Name;
        this.SkinSelection = SkinSelection;
        this.Ready = Ready;
        this.lineUp = lineUp;
    }

    public bool getReady()
    {
        return this.Ready;
    }
}
