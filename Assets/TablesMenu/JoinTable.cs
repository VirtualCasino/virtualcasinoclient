using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[Serializable]
public class JoinTable {

    public JoinTable(ClientId clientId, TableId tableId) {
        this.clientId = clientId;
        this.tableId = tableId;
    }

    public ClientId clientId;
    public TableId tableId;

}