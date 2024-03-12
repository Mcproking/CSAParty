using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class questiondb 
{
    public List<question> questionList;

    public questiondb()
    {
        questionList = new List<question>();
    }
}
