using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[Serializable]
public class TableView {

    public TableView(
        string id, 
        string tableViewId, 
        string firstPlayerNick, 
        List<string> playersIds,
        int maxPlayersCount, 
        string gameType) {
        this.id = id;
        this.tableViewId = tableViewId;
        this.firstPlayerNick = firstPlayerNick;
        this.playersIds = playersIds;
        this.maxPlayersCount = maxPlayersCount;
        this.gameType = gameType;
    }

    public string id;
    public string tableViewId;
    public string firstPlayerNick;
    public List<string> playersIds;
    public int maxPlayersCount;
    public string gameType;

}