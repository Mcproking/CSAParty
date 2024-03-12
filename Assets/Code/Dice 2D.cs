using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice2D : MonoBehaviour
{
    public int minThrow = 1;
    public int maxThrow = 6;

    public IEnumerator rollDice(Action<int> callback)
    {
        int rnd = UnityEngine.Random.Range(minThrow, maxThrow);
        callback(rnd);
        yield break;
    }

}
