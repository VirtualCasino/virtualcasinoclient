using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class BetView
{

    public BetView(string field, int value)
    {
        this.field = field;
        this.value = value;
    }

    public string field;
    public int value;
}
