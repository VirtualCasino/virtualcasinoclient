using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[Serializable]
public class RegisterClient {

    public RegisterClient(Nick nick) {
        this.nick = nick;
    }

    public Nick nick;

}