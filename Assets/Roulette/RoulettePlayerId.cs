using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[Serializable]
public class RoulettePlayerId
{

    public RoulettePlayerId(string value) {
        this.value = value;
    }

    public string value;

}