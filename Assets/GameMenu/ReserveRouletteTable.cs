using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[Serializable]
public class ReserveRouletteTable {

    public ReserveRouletteTable(ClientId clientId) {
        this.clientId = clientId;
    }

    public ClientId clientId;

}