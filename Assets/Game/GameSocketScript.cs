using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class GameSocketScript : MonoBehaviour
{
    public void Awake()
    {
        Debug.Log("Game Started!");
        Screen.orientation = ScreenOrientation.Landscape;
        var tableId = PlayerPrefs.GetString("TableId");
        var wsServerAddress = "ws://localhost:8080/game";
        Debug.Log("Connecting to: " + wsServerAddress);
        using (var ws = new WebSocket (wsServerAddress)) {
            ws.OnOpen += (sender, e) => Debug.Log ("Open: " + e);
            ws.OnMessage += (sender, e) => Debug.Log ("Message: " + e.Data);
            ws.OnClose += (sender, e) => Debug.Log ("Close: " + e);
            ws.OnError += (sender, e) => Debug.Log ("Error: " + e);

            ws.ConnectAsync ();
            Debug.Log("Websocket Connected!");
        }
    }
}