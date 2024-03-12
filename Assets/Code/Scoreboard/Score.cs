using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Score 
{
    public string name;
    public int bigPoint;
    public int smallPoint;
    
    public Score(string name, int bigPoint, int smallPoint)
    {
        this.name = name;
        this.bigPoint = bigPoint;
        this.smallPoint = smallPoint;
    }
}
