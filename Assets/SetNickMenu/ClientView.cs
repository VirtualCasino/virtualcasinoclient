using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;


[Serializable]
public class ClientView {

    public ClientView(string id, string clientViewId, string nick, int tokensCount) {
        this.id = id;
        this.clientViewId = clientViewId;
        this.nick = nick;
        this.tokensCount = tokensCount;
    }

    public string id;

    public string clientViewId;

    public string nick;

    public int tokensCount;

}