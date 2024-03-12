using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class dice : MonoBehaviour
{   
    public static int rollDice(int minThrow, int maxThrow)
    {
        int rnd = Random.Range(minThrow, maxThrow);
        return rnd;
    }

}

