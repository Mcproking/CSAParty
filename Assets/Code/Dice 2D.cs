using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice2D : MonoBehaviour
{
    public SpriteRenderer diceSprite;

    private Sprite[] dices;

    private void Start()
    {
        dices = Resources.LoadAll<Sprite>("Dice/");
        diceSprite.sprite = dices[5];
    }

    public IEnumerator rollDice(Action<int> callback)
    {
        int RDS = 0;
        for(int i = 0; i <= 20; i++)
        {
            RDS = UnityEngine.Random.Range(0, 6);
            diceSprite.sprite = dices[RDS];
            yield return new WaitForSeconds(0.05f);
        }

        // int rnd = UnityEngine.Random.Range(minThrow, maxThrow);
        int rnd = RDS + 1;
        callback(rnd);
        yield break;
    }

}
