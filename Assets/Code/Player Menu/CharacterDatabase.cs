using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSelection : ScriptableObject
{
    public Character[] character;    

    public int CharacterCount
    {
        get { return character.Length; }
    }

    public Character GetCharater(int index)
    {
        return character[index];
    }
}
